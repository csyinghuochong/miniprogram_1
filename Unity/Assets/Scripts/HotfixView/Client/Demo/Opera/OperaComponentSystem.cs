using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ET.Client
{
    public enum LayerEnum
    {
        Player,
        Monster,
        Hero,
        Map,
        MovableArea,
        NPC
    }

    [EntitySystemOf(typeof(OperaComponent))]
    [FriendOf(typeof(OperaComponent))]
    public static partial class OperaComponentSystem
    {
        [EntitySystem]
        private static void Awake(this OperaComponent self)
        {
            self.MainCamera = self.Root().GetComponent<GlobalComponent>().MainCamera;

            Init init = GameObject.Find("Global").GetComponent<Init>();
            self.EditorMode = init.EditorMode;
            self.MainUnit = UnitHelper.GetMyUnitFromClientScene(self.Root());

            self.NPCMask = LayerMask.GetMask(nameof(LayerEnum.NPC));
        }

        [EntitySystem]
        private static void Update(this OperaComponent self)
        {
            if (InputHelper.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
            {
                self.OnGetMouseButtonDown_0();
            }
        }

        private static void OnGetMouseButtonDown_0(this OperaComponent self)
        {
            if (self.EditorMode)
            {
                if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
                {
                    return;
                }
            }
            else
            {
                if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                {
                    return;
                }
            }

            if (self.IsPointerOverGameObject(Input.mousePosition) || self.CheckNpc())
            {
                return;
            }
        }

        private static bool CheckNpc(this OperaComponent self)
        {
            Ray ray = self.MainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D raycastHit2D = Physics2D.Raycast(ray.origin,
                ray.direction,
                100f,
                self.NPCMask);

            if (raycastHit2D.collider == null)
                return false;

            string objName = raycastHit2D.collider.gameObject.name;
            try
            {
                int npcId = int.Parse(objName);
                self.OnClickNpc(npcId).Coroutine();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error($"无效的npc collider: {objName}" + ex.ToString());
            }

            return false;
        }

        private static async ETTask OnClickNpc(this OperaComponent self, int npcId)
        {
            NPCConfig npcConfig = NPCConfigCategory.Instance.Get(npcId);
            if (!string.IsNullOrEmpty(npcConfig.OpenUI))
            {
                await self.Root().GetComponent<UIComponent>().Create(npcConfig.OpenUI);
            }
        }

        // 检测是否点击UI
        private static bool IsPointerOverGameObject(this OperaComponent self, Vector2 mousePosition)
        {
            //创建一个点击事件
            PointerEventData eventData = new PointerEventData(UnityEngine.EventSystems.EventSystem.current);
            eventData.position = mousePosition;
            List<RaycastResult> raycastResults = new List<RaycastResult>();
            //向点击位置发射一条射线，检测是否点击UI
            UnityEngine.EventSystems.EventSystem.current.RaycastAll(eventData, raycastResults);
            if (raycastResults.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}