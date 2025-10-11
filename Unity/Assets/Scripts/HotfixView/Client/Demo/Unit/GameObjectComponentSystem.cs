using UnityEngine;

namespace ET.Client
{
    [FriendOf(typeof(GameObjectComponent))]
    [EntitySystemOf(typeof(GameObjectComponent))]
    public static partial class GameObjectComponentSystem
    {
        [EntitySystem]
        private static void Awake(this GameObjectComponent self)
        {
            self.GameObject = null;
            self.UnitAssetsPath = string.Empty;
            self.LoadGameObject();
        }

        [EntitySystem]
        private static void Destroy(this GameObjectComponent self)
        {
            self.RecoverGameObject();
        }

        public static void RecoverGameObject(this GameObjectComponent self)
        {
            if (self.GameObject != null)
            {
                self.GameObject.transform.localScale = Vector3.one;
            }

            if (string.IsNullOrEmpty(self.UnitAssetsPath) && self.GameObject != null)
            {
                UnityEngine.Object.Destroy(self.GameObject);
            }

            if (!string.IsNullOrEmpty(self.UnitAssetsPath))
            {
                self.Root().GetComponent<GameObjectLoadComponent>().RecoverGameObject(self.UnitAssetsPath, self.GameObject);
            }

            self.GameObject.GetComponent<UnitId>().Id = 0;
            self.GameObject = null;
        }

        private static void LoadGameObject(this GameObjectComponent self)
        {
            Unit unit = self.GetParent<Unit>();
            int unitType = unit.Type;

            switch (unitType)
            {
                case UnitType.Player:
                    // self.UnitAssetsPath = ABPathHelper.GetUnitPath($"Player/1");
                    return;

                    break;
                case UnitType.Hero:
                    HeroConfig heroConfig = HeroConfigCategory.Instance.Get(unit.ConfigId);
                    self.UnitAssetsPath = ABPathHelper.GetUnitPath($"Hero/{heroConfig.HeroModelID}");

                    break;
                case UnitType.Monster:
                    MonsterConfig monsterConfig = MonsterConfigCategory.Instance.Get(unit.ConfigId);
                    self.UnitAssetsPath = ABPathHelper.GetUnitPath($"Monster/{monsterConfig.MonsterModelID}");

                    break;
                default:
                    break;
            }

            if (!string.IsNullOrEmpty(self.UnitAssetsPath))
            {
                self.Root().GetComponent<GameObjectLoadComponent>().AddLoadQueue(self.UnitAssetsPath, self.InstanceId, false, self.OnLoadGameObject);
            }
        }

        public static void UpdateRotation(this GameObjectComponent self, Quaternion quaternion)
        {
            if (self.GameObject != null)
            {
                self.GameObject.transform.rotation = quaternion;
            }
        }

        public static void UpdatePositon(this GameObjectComponent self, Vector3 vector)
        {
            if (self.GameObject != null)
            {
                self.GameObject.transform.position = vector;
            }
        }

        private static void OnLoadGameObject(this GameObjectComponent self, GameObject go, long formId)
        {
            if (self.IsDisposed)
            {
                UnityEngine.Object.Destroy(go);
                return;
            }

            if (self.GameObject != null)
            {
                Log.Error($" self.GameObject !=null:   {self.GameObject.name}    {go.name}   {self.InstanceId}   {formId}");
                return;
            }

            self.GameObject = go;
            go.transform.SetParent(self.Root().GetComponent<GlobalComponent>().Unit);

            Unit unit = self.GetParent<Unit>();
            int unitType = unit.Type;
            switch (unitType)
            {
                case UnitType.Player:
                {
                    break;
                }
                case UnitType.Hero:
                {
                    self.UpdatePositon(unit.Position);
                    UnitId unitId = self.GameObject.GetComponent<UnitId>() ?? self.GameObject.AddComponent<UnitId>();
                    unitId.Id = unit.Id;
                    unit.AddComponent<UIHeroHpComponent>();
                    unit.AddComponent<SkillManagerComponent>();
                    unit.AddComponent<AI_HeroComponent>();
                    break;
                }
                case UnitType.Monster:
                {
                    self.UpdatePositon(unit.Position);
                    UnitId unitId = self.GameObject.GetComponent<UnitId>() ?? self.GameObject.AddComponent<UnitId>();
                    unitId.Id = unit.Id;
                    unit.AddComponent<UIMonsterHpComponent>();
                    unit.AddComponent<SkillManagerComponent>();
                    break;
                }
                default:
                    break;
            }
        }
    }
}