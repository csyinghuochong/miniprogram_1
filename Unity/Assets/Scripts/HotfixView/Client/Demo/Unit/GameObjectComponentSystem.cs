using Cysharp.Text;
using Spine.Unity;
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
                    MapComponent mapComponent = self.Root().GetComponent<MapComponent>();
                    if (unit.MainHero && mapComponent.MapType == MapType.LocalLevel && unit.GetComponent<NumericComponentC>().GetAsInt(NumericType.BattleMode) == 1)
                    {
                        self.UnitAssetsPath = "";
                    }
                    else
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
                    }

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
                case UnitType.DropItem:
                {
                    self.UnitAssetsPath = ABPathHelper.GetUnitPath(ABUnitType.DropItem, "DropItem");

                    break;
                }
                case UnitType.NPC:
                {
                    NPCConfig npcConfig = NPCConfigCategory.Instance.Get(unit.ConfigId);
                    self.UnitAssetsPath = ABPathHelper.GetUnitPath(ABUnitType.NPC, npcConfig.Model);

                    break;
                }
                default:
                    break;
            }

            if (!string.IsNullOrEmpty(self.UnitAssetsPath))
            {
                self.Root().GetComponent<GameObjectLoadComponent>().AddLoadQueue(self.UnitAssetsPath, self.InstanceId, false, self.OnLoadGameObject);
            }
            else
            {
                unit.FinishLoad = true;
            }
        }

        public static void UpdateRotation(this GameObjectComponent self, Quaternion quaternion)
        {
            if (self.GameObject != null)
            {
                self.GameObject.transform.rotation = quaternion;
            }
        }

        public static void UpdateScaleX(this GameObjectComponent self, float scale)
        {
            if (self.GameObject != null)
            {
                if (scale != 0)
                {
                    self.GameObject.transform.localScale = new Vector3(scale > 0 ? 1 : -1, 1, 1);
                }
            }
        }

        public static void UpdatePositon(this GameObjectComponent self, Vector3 vector)
        {
            if (self.GameObject != null)
            {
                self.GameObject.transform.eulerAngles = ConfigData.ViewMode == 0 ? Vector3.zero : new Vector3(ConfigData.CameraAngle, 0, 0);

                self.GameObject.transform.position = new Vector3(vector.x, vector.y, 0);
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

            go.transform.SetParent(self.Root().GetComponent<GlobalComponent>().Unit);
            self.GameObject = go;
            self.GameObject.SetActive(true);

            Unit unit = self.GetParent<Unit>();
            unit.FinishLoad = true;
            self.UpdatePositon(unit.Position);

            UnitId unitId = self.GameObject.GetComponent<UnitId>() ?? self.GameObject.AddComponent<UnitId>();
            unitId.Id = unit.Id;

            int unitType = unit.Type;
            switch (unitType)
            {
                case UnitType.Player:
                {
                    self.GameObject.tag = TagHelper.Player;
                    LayerHelp.ChangeLayerAll(self.GameObject.transform, LayerEnum.Player);

                    if (unit.MainHero)
                    {
                        unit.AddComponent<TransformNoticeToServerComponent>();
                        unit.AddComponent<Move2DComponent>();
                        self.LoadPath().Coroutine();
                    }
                    else
                    {
                        unit.AddComponent<TransformSyncComponent>();
                    }

                    unit.AddComponent<UnitBoneComponent>();
                    unit.AddComponent<UIPlayerHpComponent>();
                    unit.AddComponent<EffectViewComponent>();
                    unit.AddComponent<FsmComponent>();
                    break;
                }
                case UnitType.Hero:
                {
                    self.GameObject.tag = TagHelper.Hero;
                    LayerHelp.ChangeLayerAll(self.GameObject.transform, LayerEnum.Hero);

                    unit.AddComponent<UnitBoneComponent>();
                    unit.AddComponent<UIHeroHpComponent>();
                    unit.AddComponent<EffectViewComponent>();
                    unit.AddComponent<FsmComponent>();
                    unit.AddComponent<TransformSyncComponent>();
                    break;
                }
                case UnitType.Monster:
                {
                    self.GameObject.tag = TagHelper.Monster;
                    LayerHelp.ChangeLayerAll(self.GameObject.transform, LayerEnum.Monster);

                    unit.AddComponent<UnitBoneComponent>();
                    unit.AddComponent<UIMonsterHpComponent>();
                    unit.AddComponent<EffectViewComponent>();
                    unit.AddComponent<FsmComponent>();
                    unit.AddComponent<TransformSyncComponent>();
                    break;
                }
                case UnitType.DropItem:
                {
                    unit.AddComponent<UIDropItemComponent>();
                    break;
                }
                case UnitType.NPC:
                {
                    LayerHelp.ChangeLayerAll(self.GameObject.transform, LayerEnum.NPC);

                    self.GameObject.name = unit.ConfigId.ToString();

                    unit.AddComponent<UnitBoneComponent>();
                    unit.AddComponent<UINPCHpComponent>();
                    break;
                }
                default:
                    break;
            }
        }

        private static async ETTask LoadPath(this GameObjectComponent self)
        {
            MapComponent mapComponent = self.Root().GetComponent<MapComponent>();
            TextAsset textAsset = await self.Root().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<TextAsset>(ABPathHelper.GetRecastPath(CommonHelp.GetMapObjName(mapComponent.MapType)));
            self.GetParent<Unit>().AddComponent<PathfindingComponent, byte[]>(textAsset.bytes);
        }
        
        public static void ReloadGameObject(this GameObjectComponent self)
        {
            // 后面给需要改变的组件加加一个接口
            return;
            
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