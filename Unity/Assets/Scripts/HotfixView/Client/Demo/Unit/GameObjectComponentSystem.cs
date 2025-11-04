using Cysharp.Text;
using UnityEngine;

namespace ET.Client
{
    [NumericWatcher(SceneType.Current, NumericType.ShowHeroId)]
    public class NumericWatcher_ReloadGameObject : INumericWatcher
    {
        public void Run(Unit unit, NumbericChange args)
        {
            if (unit.Type != UnitType.Player)
            {
                return;
            }

            if (args.OldValue == args.NewValue)
            {
                return;
            }

            unit.GetComponent<GameObjectComponent>()?.ReloadGameObject();
        }
    }

    [FriendOf(typeof(GameObjectComponent))]
    [EntitySystemOf(typeof(GameObjectComponent))]
    public static partial class GameObjectComponentSystem
    {
        [EntitySystem]
        private static void Awake(this GameObjectComponent self)
        {
            self.LoadGameObject();
        }

        [EntitySystem]
        private static void Destroy(this GameObjectComponent self)
        {
            self.RecoverGameObject();
        }

        private static void RecoverGameObject(this GameObjectComponent self)
        {
            if (self.GameObject != null)
            {
                self.GameObject.transform.localScale = Vector3.one;

                if (self.GameObject.GetComponent<UnitId>() != null)
                {
                    self.GameObject.GetComponent<UnitId>().Id = 0;
                }
            }

            if (string.IsNullOrEmpty(self.UnitAssetsPath) && self.GameObject != null)
            {
                UnityEngine.Object.Destroy(self.GameObject);
            }

            if (!string.IsNullOrEmpty(self.UnitAssetsPath))
            {
                self.Root().GetComponent<GameObjectLoadComponent>().RecoverGameObject(self.UnitAssetsPath, self.GameObject);
            }

            self.GameObject = null;
        }

        private static void LoadGameObject(this GameObjectComponent self)
        {
            Unit unit = self.GetParent<Unit>();
            int unitType = unit.Type;

            switch (unitType)
            {
                case UnitType.Player:
                {
                    NumericComponentC numericComponent = unit.GetComponent<NumericComponentC>();
                    int heroId = numericComponent.GetAsInt(NumericType.ShowHeroId);

                    HeroConfig heroConfig = null;
                    if (!HeroConfigCategory.Instance.DataMap.ContainsKey(heroId))
                    {
                        Log.Warning("没有英雄上阵，默认用第一个吧");
                        heroConfig = HeroConfigCategory.Instance.DataList[0];
                    }
                    else
                    {
                        heroConfig = HeroConfigCategory.Instance.Get(heroId);
                    }

                    self.UnitAssetsPath = ABPathHelper.GetUnitPath(ABUnitType.Hero, heroConfig.HeroModelID);
                    break;
                }
                case UnitType.Hero:
                {
                    HeroConfig heroConfig = HeroConfigCategory.Instance.Get(unit.ConfigId);
                    self.UnitAssetsPath = ABPathHelper.GetUnitPath(ABUnitType.Hero, heroConfig.HeroModelID);

                    break;
                }
                case UnitType.Monster:
                {
                    MonsterConfig monsterConfig = MonsterConfigCategory.Instance.Get(unit.ConfigId);
                    self.UnitAssetsPath = ABPathHelper.GetUnitPath(ABUnitType.Monster, monsterConfig.MonsterModelID);

                    break;
                }
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
                // self.GameObject.transform.rotation = quaternion;
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
            self.GameObject.SetActive(true);

            Unit unit = self.GetParent<Unit>();
            self.UpdatePositon(unit.Position);
            UnitId unitId = self.GameObject.GetComponent<UnitId>() ?? self.GameObject.AddComponent<UnitId>();
            unitId.Id = unit.Id;
            int unitType = unit.Type;
            switch (unitType)
            {
                case UnitType.Player:
                {
                    self.GameObject.tag = TagHelper.Player;

                    unit.AddComponent<UnitBoneComponent>();
                    unit.AddComponent<UIPlayerHpComponent>();
                    unit.AddComponent<EffectViewComponent>();
                    unit.AddComponent<FsmComponent>();
                    break;
                }
                case UnitType.Hero:
                {
                    self.GameObject.tag = TagHelper.Hero;

                    unit.AddComponent<UnitBoneComponent>();
                    unit.AddComponent<UIHeroHpComponent>();
                    unit.AddComponent<EffectViewComponent>();
                    unit.AddComponent<FsmComponent>();
                    break;
                }
                case UnitType.Monster:
                {
                    self.GameObject.tag = TagHelper.Monster;

                    unit.AddComponent<UnitBoneComponent>();
                    unit.AddComponent<UIMonsterHpComponent>();
                    unit.AddComponent<EffectViewComponent>();
                    unit.AddComponent<FsmComponent>();
                    break;
                }
                default:
                    break;
            }
        }

        public static void ReloadGameObject(this GameObjectComponent self)
        {
            Unit unit = self.GetParent<Unit>();
            int unitType = unit.Type;
            switch (unitType)
            {
                case UnitType.Player:
                {
                    unit.RemoveComponent<UnitBoneComponent>();
                    unit.RemoveComponent<UIPlayerHpComponent>();
                    unit.RemoveComponent<EffectViewComponent>();
                    unit.RemoveComponent<FsmComponent>();
                    break;
                }
                case UnitType.Hero:
                {
                    unit.RemoveComponent<UnitBoneComponent>();
                    unit.RemoveComponent<UIHeroHpComponent>();
                    unit.RemoveComponent<EffectViewComponent>();
                    unit.RemoveComponent<FsmComponent>();
                    break;
                }
                case UnitType.Monster:
                {
                    unit.RemoveComponent<UnitBoneComponent>();
                    unit.RemoveComponent<UIMonsterHpComponent>();
                    unit.RemoveComponent<EffectViewComponent>();
                    unit.RemoveComponent<FsmComponent>();
                    break;
                }
            }

            self.RecoverGameObject();

            self.LoadGameObject();
        }
    }
}