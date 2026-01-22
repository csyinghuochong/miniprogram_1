using System;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ET.Client
{
    [Invoke(TimerInvokeType.UIMapBigTimer)]
    public class UIMapBigTimer : ATimer<UIMainCityMapComponent>
    {
        protected override void Run(UIMainCityMapComponent self)
        {
            try
            {
                self.OnUpdateMapAllUnit();
                self.UpdatePathPoint();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }

    [EntitySystemOf(typeof(UIMainCityMapComponent))]
    [FriendOf(typeof(UIMainCityMapComponent))]
    public static partial class UIMainCityMapComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UIMainCityMapComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.Button_Close = rc.Get<GameObject>("Button_Close").GetComponent<Button>();
            self.RawImage_Map = rc.Get<GameObject>("RawImage_Map").GetComponent<RawImage>();
            self.Transform_HeadList = rc.Get<GameObject>("Transform_HeadList").transform;
            self.Transform_HeadItem = rc.Get<GameObject>("Transform_HeadItem").transform;
            self.Transform_HeadItem.gameObject.SetActive(false);
            self.Transform_PathPointList = rc.Get<GameObject>("Transform_PathPointList").transform;
            self.Transform_PathPoint = rc.Get<GameObject>("Transform_PathPoint").transform;
            self.Transform_PathPoint.gameObject.SetActive(false);
            self.Content_UIMainCityMapNPCButton = rc.Get<GameObject>("Content_UIMainCityMapNPCButton").transform;
            self.UIMainCityMapNPCButton = rc.Get<GameObject>("UIMainCityMapNPCButton");
            self.UIMainCityMapNPCButton.SetActive(false);

            self.Button_Close.AddListener((() => { self.Root().GetComponent<UIComponent>().Remove(UIType.UIMainCityMap); }));
            self.RawImage_Map.GetComponent<EventTrigger>().AddEventTrigger(self.OnPointerDown, EventTriggerType.PointerDown);

            self.InitNpcList();
            self.LoadMapCamera().Coroutine();
        }

        [EntitySystem]
        private static void Destroy(this UIMainCityMapComponent self)
        {
            self.Root().GetComponent<TimerComponent>().Remove(ref self.Timer);
            self.UIMainCityMapNPCButtonList.Clear();
            self.UIMainCityMapNPCButton = null;
            self.LastPathTargets.Clear();
            self.CachedInterpolatedPath.Clear();
        }

        private static void InitNpcList(this UIMainCityMapComponent self)
        {
            foreach (NPCConfig npcConfig in NPCConfigCategory.Instance.DataList)
            {
                GameObject go = UnityEngine.Object.Instantiate(self.UIMainCityMapNPCButton, self.Content_UIMainCityMapNPCButton);
                UIMainCityMapNPCButton item = self.AddChild<UIMainCityMapNPCButton, GameObject>(go);
                item.UpdateInfo(npcConfig);

                go.SetActive(true);
            }
        }

        private static async ETTask LoadMapCamera(this UIMainCityMapComponent self)
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
            camera.orthographicSize = 30f; //控制地图大小
            self.ScaleRateX = self.RawImage_Map.GetComponent<RectTransform>().rect.height / (camera.orthographicSize * 2);
            self.ScaleRateY = self.RawImage_Map.GetComponent<RectTransform>().rect.height / (camera.orthographicSize * 2);

            self.OnUpdateMapAllUnit();

            self.Timer = self.Root().GetComponent<TimerComponent>().NewFrameTimer(TimerInvokeType.UIMapBigTimer, self);
        }

        private static async ETTask TakePhoto(this UIMainCityMapComponent self)
        {
            if (self.MapCamera == null)
            {
                return;
            }

            Camera camera = self.MapCamera.GetComponent<Camera>();
            camera.enabled = true;

            await self.Root().GetComponent<TimerComponent>().WaitFrameAsync();
            camera.enabled = false;
        }

        public static void OnUpdateMapAllUnit(this UIMainCityMapComponent self)
        {
            Unit mainUnit = UnitHelper.GetMyUnitFromClientScene(self.Root());

            if (mainUnit == null || self.MapCamera == null)
            {
                return;
            }

            List<EntityRef<Unit>> allUnit = mainUnit.GetParent<UnitComponent>().GetAll();

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

        public static void OnUpdateMiniMapOneUnit(this UIMainCityMapComponent self, Unit unit)
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

            if (unit.Type != UnitType.Player && unit.Type != UnitType.NPC)
            {
                return;
            }

            Vector3 vector31 = new Vector3(unit.Position.x, unit.Position.y, 0f);
            Vector3 vector32 = self.GetWordToUIPositon(vector31);
            GameObject headItem = self.GetHeadItemObj(unit.Id);
            TMP_Text text_Name = headItem.transform.Find("Text_Name").GetComponent<TMP_Text>();

            //1自己 2NPC
            string showType = "1";

            if (unit.Type == UnitType.Player)
            {
                showType = "1";
                text_Name.gameObject.SetActive(false);
            }

            if (unit.Type == UnitType.NPC)
            {
                showType = "2";
                text_Name.gameObject.SetActive(true);
                text_Name.SetText(unit.GetComponent<UnitInfoComponent>().UnitName);
            }

            List<string> headIcon = new()
            {
                "1", "2"
            };

            for (int i = 0; i < headIcon.Count; i++)
            {
                headItem.transform.Find(headIcon[i]).localPosition = headIcon[i] == showType ? Vector3.zero : new Vector3(-1000, 0, 0);
            }

            headItem.transform.localPosition = new Vector2(vector32.x, vector32.y);
        }

        private static void OnUnitUnitRemove(this UIMainCityMapComponent self, List<long> removeIds)
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

        private static Vector3 GetWordToUIPositon(this UIMainCityMapComponent self, Vector3 vector3)
        {
            GameObject mapCamera = self.MapCamera;
            vector3.x -= mapCamera.transform.position.x;
            vector3.y -= mapCamera.transform.position.y;

            vector3.x *= self.ScaleRateX;
            vector3.y *= self.ScaleRateY;
            return vector3;
        }

        private static GameObject GetHeadItemObj(this UIMainCityMapComponent self, long unitId)
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

        private static void OnPointerDown(this UIMainCityMapComponent self, PointerEventData pdata)
        {
            Scene currentScene = self.Root().CurrentScene();
            if (currentScene == null)
            {
                return;
            }

            Unit unit = UnitHelper.GetMyUnitFromCurrentScene(currentScene);
            if (unit == null)
            {
                return;
            }

            GameObject mapCamera = self.MapCamera;
            RectTransform canvas = self.RawImage_Map.transform.GetComponent<RectTransform>();
            Camera uiCamera = self.Root().GetComponent<GlobalComponent>().UICamera.GetComponent<Camera>();
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, pdata.position, uiCamera, out self.LocalPoint);

            Vector2 wordPos = new Vector3(self.LocalPoint.x / self.ScaleRateX, self.LocalPoint.y / self.ScaleRateY);

            Vector3 position = mapCamera.transform.position;
            wordPos.x += position.x;
            wordPos.y += position.y;

            MoveHelper.MoveTo(unit, wordPos);
        }

        # region 移动路径

        public static void UpdatePathPoint(this UIMainCityMapComponent self)
        {
            Unit mainUnit = UnitHelper.GetMyUnitFromClientScene(self.Root());

            if (mainUnit == null || self.MapCamera == null)
            {
                // 如果没有主角单位或相机，隐藏所有路径点
                self.HideAllPathPoints();
                return;
            }

            Move2DComponent move2DComponent = mainUnit.GetComponent<Move2DComponent>();
            if (move2DComponent == null || move2DComponent.Targets.Count == 0)
            {
                // 如果没有移动目标，隐藏所有路径点并清空缓存
                self.HideAllPathPoints();
                self.LastPathTargets.Clear();
                self.CachedInterpolatedPath.Clear();
                return;
            }

            // 检查路径是否发生了变化
            bool pathChanged = self.IsPathChanged(mainUnit.Position, move2DComponent.Targets);

            if (!pathChanged && self.CachedInterpolatedPath.Count > 0)
            {
                // 路径没变，直接使用缓存的插值路径更新显示
                self.UpdatePathPointDisplay();
                return;
            }

            // 路径发生了变化，重新计算插值路径
            self.RecalculateInterpolatedPath(mainUnit.Position, move2DComponent.Targets);

            // 更新显示
            self.UpdatePathPointDisplay();
        }

        /// <summary>
        /// 检查路径是否发生了变化
        /// </summary>
        private static bool IsPathChanged(this UIMainCityMapComponent self, Vector3 currentPosition, List<float3> targets)
        {
            // 如果缓存的目标点数量不一致，路径肯定变了
            if (self.LastPathTargets.Count != targets.Count + 1)
            {
                return true;
            }

            // 检查起点是否变化（容差 0.01）
            if (Vector3.Distance(self.LastPathTargets[0], currentPosition) > 0.01f)
            {
                return true;
            }

            // 检查每个目标点是否变化
            for (int i = 0; i < targets.Count; i++)
            {
                if (Vector3.Distance(self.LastPathTargets[i + 1], targets[i]) > 0.01f)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 重新计算插值后的完整路径
        /// </summary>
        private static void RecalculateInterpolatedPath(this UIMainCityMapComponent self, Vector3 currentPosition, List<float3> targets)
        {
            // 更新缓存的原始路径
            self.LastPathTargets.Clear();
            self.LastPathTargets.Add(currentPosition);
            for (int i = 0; i < targets.Count; i++)
            {
                self.LastPathTargets.Add(targets[i]);
            }

            // 计算插值路径
            self.CachedInterpolatedPath.Clear();

            if (self.LastPathTargets.Count < 2)
            {
                return;
            }

            float intervalDistance = 1f;
            self.CachedInterpolatedPath.Add(self.LastPathTargets[0]);

            for (int i = 1; i < self.LastPathTargets.Count; i++)
            {
                Vector3 startPoint = self.LastPathTargets[i - 1];
                Vector3 endPoint = self.LastPathTargets[i];

                float distanceBetweenPoints = Vector3.Distance(startPoint, endPoint);
                Vector3 direction = (endPoint - startPoint).normalized;
                float currentDistance = 0;

                while (currentDistance + intervalDistance <= distanceBetweenPoints)
                {
                    currentDistance += intervalDistance;
                    Vector3 newPoint = startPoint + direction * currentDistance;
                    self.CachedInterpolatedPath.Add(newPoint);
                }

                // 添加终点
                self.CachedInterpolatedPath.Add(endPoint);
            }
        }

        /// <summary>
        /// 更新路径点的显示（使用缓存的插值路径）
        /// </summary>
        private static void UpdatePathPointDisplay(this UIMainCityMapComponent self)
        {
            // 从后往前显示路径点（终点在前，起点在后）
            int showNumber = 0;
            for (int i = self.CachedInterpolatedPath.Count - 1; i >= 0; i--)
            {
                Vector3 temp = self.CachedInterpolatedPath[i];
                Vector3 uiPos = self.GetWordToUIPositon(new Vector3(temp.x, temp.y, 0f));

                GameObject go = self.GetPathPointObj(showNumber);
                go.transform.localPosition = uiPos;
                go.SetActive(true);
                showNumber++;
            }

            // 隐藏多余的路径点对象
            for (int i = showNumber; i < self.PathPointList.Count; i++)
            {
                self.PathPointList[i].SetActive(false);
            }
        }

        /// <summary>
        /// 隐藏所有路径点
        /// </summary>
        private static void HideAllPathPoints(this UIMainCityMapComponent self)
        {
            for (int i = 0; i < self.PathPointList.Count; i++)
            {
                self.PathPointList[i].SetActive(false);
            }
        }

        private static GameObject GetPathPointObj(this UIMainCityMapComponent self, int index)
        {
            if (self.PathPointList.Count > index)
            {
                return self.PathPointList[index];
            }

            GameObject go = UnityEngine.Object.Instantiate(self.Transform_PathPoint.gameObject, self.Transform_PathPointList.transform, true);
            go.SetActive(true);
            go.transform.localScale = Vector3.one;
            self.PathPointList.Add(go);
            return go;
        }

        # endregion
    }
}