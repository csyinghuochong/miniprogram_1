using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIMiniMapComponent))]
    [FriendOf(typeof(UIMiniMapComponent))]
    public static partial class UIMiniMapComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UIMiniMapComponent self, GameObject gameObject)
        {
            self.GameObject = gameObject;
            ReferenceCollector rc = gameObject.GetComponent<ReferenceCollector>();

            self.RawImage_Map = rc.Get<GameObject>("RawImage_Map").GetComponent<RawImage>();
            self.Text_MiniMapName = rc.Get<GameObject>("Text_MiniMapName").GetComponent<TMP_Text>();
        }

        [EntitySystem]
        private static void Destroy(this UIMiniMapComponent self)
        {
        }

        public static void AfterEnterScene(this UIMiniMapComponent self, MapType mapType)
        {
            if (mapType == MapType.MainCity)
            {
                self.Text_MiniMapName.SetText("主城");
            }

            if (mapType == MapType.LocalLevel)
            {
                Unit unit = UnitHelper.GetMyUnitFromClientScene(self.Root());
                NumericComponentC numericComponent = unit.GetComponent<NumericComponentC>();
                int currentLevelId = numericComponent.GetAsInt(NumericType.CurrentLevelId);
                if (!LevelConfigCategory.Instance.DataMap.ContainsKey(currentLevelId))
                {
                    return;
                }

                LevelConfig levelConfig = LevelConfigCategory.Instance.Get(currentLevelId);

                self.Text_MiniMapName.SetText(levelConfig.LevelName);
            }

            self.LoadMapCamera().Coroutine();
        }

        private static async ETTask LoadMapCamera(this UIMiniMapComponent self)
        {
            GameObject mapCamera = GameObject.Find("Global/MapCamera");
            if (mapCamera == null)
            {
                var path = ABPathHelper.GetUnitPath("Component", "MapCamera");
                GameObject prefab = await self.Root().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<GameObject>(path);
                mapCamera = UnityEngine.Object.Instantiate(prefab, GameObject.Find("Global").transform);
                mapCamera.name = "MapCamera";
            }

            Camera camera = mapCamera.GetComponent<Camera>();
            camera.enabled = true;

            self.MapCamera = mapCamera;
        }

        public static void OnMainHeroMove(this UIMiniMapComponent self)
        {
            if (self.MapCamera == null)
            {
                return;
            }

            Unit unit = UnitHelper.GetMyUnitFromClientScene(self.Root());
            if (unit == null || self.MapCamera == null)
            {
                return;
            }

            Vector3 old = self.MapCamera.transform.position;
            old.x = unit.Position.x;
            old.y = unit.Position.y;
            self.MapCamera.transform.position = old;
        }
    }
}