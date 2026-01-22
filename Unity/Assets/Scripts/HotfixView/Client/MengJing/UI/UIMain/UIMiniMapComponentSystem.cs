using System.Collections.Generic;
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
            self.Transform_HeadList = rc.Get<GameObject>("Transform_HeadList").transform;
            self.Transform_HeadItem = rc.Get<GameObject>("Transform_HeadItem").transform;
            self.Transform_HeadItem.gameObject.SetActive(false);
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
                self.UpdateLevelName();
            }

            self.LoadMapCamera().Coroutine();
        }

        public static void UpdateLevelName(this UIMiniMapComponent self)
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

        private static async ETTask LoadMapCamera(this UIMiniMapComponent self)
        {
            GameObject mapCamera = GameObject.Find("Global/MapCamera");
            if (mapCamera == null)
            {
                string path = ABPathHelper.GetUnitPath("Component", "MapCamera");
                GameObject prefab = await self.Root().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<GameObject>(path);
                mapCamera = UnityEngine.Object.Instantiate(prefab, GameObject.Find("Global").transform);
                mapCamera.name = "MapCamera";
            }

            self.MapCamera = mapCamera;
            self.MapCamera.transform.position = ConfigData.MapCameraPosition;

            self.TakePhoto().Coroutine();

            Camera camera = self.MapCamera.GetComponent<Camera>();
            camera.orthographicSize = 100f; //控制地图大小
            self.ScaleRateX = self.RawImage_Map.GetComponent<RectTransform>().rect.height / (camera.orthographicSize * 2);
            self.ScaleRateY = self.RawImage_Map.GetComponent<RectTransform>().rect.height / (camera.orthographicSize * 2);

            self.OnUpdateMiniMapAllUnit();
        }

        private static async ETTask TakePhoto(this UIMiniMapComponent self)
        {
            if (self.MapCamera == null)
            {
                return;
            }

            Camera camera = self.MapCamera.GetComponent<Camera>();
            camera.enabled = true;

            self.LastMapCameraPos = self.MapCamera.transform.position;
            
            await self.Root().GetComponent<TimerComponent>().WaitFrameAsync();
            camera.enabled = false;
        }

        public static void OnUpdateMiniMapAllUnit(this UIMiniMapComponent self)
        {
            Unit mainUnit = UnitHelper.GetMyUnitFromClientScene(self.Root());

            if (mainUnit == null || self.MapCamera == null)
            {
                return;
            }

            List<EntityRef<Unit>> allUnit = mainUnit.GetParent<UnitComponent>().GetAll();

            MapComponent mapComponent = self.Root().GetComponent<MapComponent>();
            MapType mapType = mapComponent.MapType;

            Vector3 mainUnitPos = mainUnit.Position;
            Vector2 centerPos = self.GetWordToUIPositon(new Vector3(mainUnitPos.x, mainUnitPos.y, 0f));
            // if (mapType == MapType.LocalLevel)
            // {
            //     // 关卡是无限循环生成的，要不断拍照，刷新小地图
            //     if (Vector3.Distance(self.LastMapCameraPos, mainUnitPos) > 10f)
            //     {
            //         Vector3 old = self.MapCamera.transform.position;
            //         old.x = mainUnit.Position.x;
            //         old.y = mainUnit.Position.y;
            //         self.MapCamera.transform.position = old;
            //         self.TakePhoto().Coroutine();
            //     }
            // }

            self.RawImage_Map.transform.localPosition = new Vector2(centerPos.x * -1, centerPos.y * -1);
            self.Transform_HeadList.localPosition = new Vector2(centerPos.x * -1, centerPos.y * -1);

            using ListComponent<long> allIds = ListComponent<long>.Create();
            for (int i = 0; i < allUnit.Count; i++)
            {
                Unit unit = allUnit[i];
                allIds.Add(unit.Id);
                self.OnUpdateMiniMapOneUnit(unit);
            }

            using ListComponent<long> removeIds = ListComponent<long>.Create();
            foreach (var keyValuePair in self.AllPointList)
            {
                if (!allIds.Contains(keyValuePair.Key))
                {
                    removeIds.Add(keyValuePair.Key);
                }
            }

            self.OnUnitUnitRemove(removeIds);
        }

        public static void OnUpdateMiniMapOneUnit(this UIMiniMapComponent self, Unit unit)
        {
            Unit main = UnitHelper.GetMyUnitFromClientScene(self.Root());

            if (main == null)
            {
                return;
            }

            if (self.MapCamera == null)
            {
                return;
            }

            if (unit.Type != UnitType.Player && unit.Type != UnitType.Hero && unit.Type != UnitType.Monster)
            {
                return;
            }

            Vector3 vector31 = new Vector3(unit.Position.x, unit.Position.y, 0f);
            Vector3 vector32 = self.GetWordToUIPositon(vector31);
            GameObject headItem = self.GetHeadItemObj(unit.Id);

            //1自己 2敌对 3队友
            string showType = "1";

            if (unit.Type == UnitType.Player)
            {
                showType = "1";
            }

            if (unit.Type == UnitType.Monster)
            {
                showType = "2";
            }

            if (unit.Type == UnitType.Hero)
            {
                showType = "3";
            }

            List<string> headIcon = new()
            {
                "1", "2", "3"
            };

            for (int i = 0; i < headIcon.Count; i++)
            {
                headItem.transform.Find(headIcon[i]).localPosition = headIcon[i] == showType ? Vector3.zero : new Vector3(-1000, 0, 0);
            }

            headItem.transform.localPosition = new Vector2(vector32.x, vector32.y);
        }

        private static void OnUnitUnitRemove(this UIMiniMapComponent self, List<long> removeIds)
        {
            foreach (long removeId in removeIds)
            {
                GameObject icon = null;
                self.AllPointList.TryGetValue(removeId, out icon);
                if (icon != null)
                {
                    self.AllPointList.Remove(removeId);
                    icon.transform.localPosition = new Vector3(-1000, 0, 0);
                    self.CachePointList.Add(icon);
                }
            }

            foreach (var icon in self.CachePointList)
            {
                icon.transform.localPosition = new Vector3(-1000, 0, 0);
            }
        }

        private static Vector3 GetWordToUIPositon(this UIMiniMapComponent self, Vector3 vector3)
        {
            GameObject mapCamera = self.MapCamera;
            vector3.x -= mapCamera.transform.position.x;
            vector3.y -= mapCamera.transform.position.y;

            vector3.x *= self.ScaleRateX;
            vector3.y *= self.ScaleRateY;
            return vector3;
        }

        private static GameObject GetHeadItemObj(this UIMiniMapComponent self, long unitId)
        {
            GameObject icon = null;
            self.AllPointList.TryGetValue(unitId, out icon);
            if (icon != null)
            {
                return icon;
            }

            if (self.CachePointList.Count > 0)
            {
                icon = self.CachePointList[0];

                self.CachePointList.RemoveAt(0);
                self.AllPointList.Add(unitId, icon);
                return icon;
            }

            GameObject go = UnityEngine.Object.Instantiate(self.Transform_HeadItem.gameObject, self.Transform_HeadItem.parent, true);
            go.transform.localScale = Vector3.one;
            go.SetActive(true);
            self.AllPointList.Add(unitId, go);
            return go;
        }
    }
}