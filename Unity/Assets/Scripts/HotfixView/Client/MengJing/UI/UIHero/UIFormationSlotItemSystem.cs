using System.Collections.Generic;
using Cysharp.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIFormationSlotItem))]
    [FriendOf(typeof(UIFormationSlotItem))]
    public static partial class UIFormationSlotItemSystem
    {
        [EntitySystem]
        private static void Awake(this UIFormationSlotItem self, GameObject gameObject)
        {
            self.GameObject = gameObject;

            ReferenceCollector rc = gameObject.GetComponent<ReferenceCollector>();

            self.Transform_HeroIcon = rc.Get<GameObject>("Transform_HeroIcon").transform;
            self.Text_HeroName = rc.Get<GameObject>("Text_HeroName").GetComponent<TMP_Text>();
            self.EventTrigger_Click = rc.Get<GameObject>("EventTrigger_Click").GetComponent<EventTrigger>();

            self.EventTrigger_Click.AddEventTrigger(self.OnPointerDown, EventTriggerType.PointerDown);
            self.EventTrigger_Click.AddEventTrigger(self.OnBeginDrag, EventTriggerType.BeginDrag);
            self.EventTrigger_Click.AddEventTrigger(self.OnDrag, EventTriggerType.Drag);
            self.EventTrigger_Click.AddEventTrigger(self.OnPointerUp, EventTriggerType.PointerUp);
            self.EventTrigger_Click.AddEventTrigger(self.OnEndDrag, EventTriggerType.EndDrag);
        }

        private static void OnPointerDown(this UIFormationSlotItem self, PointerEventData pdata)
        {
        }

        private static void OnBeginDrag(this UIFormationSlotItem self, PointerEventData pdata)
        {
            if (self.HeroId == 0)
            {
                return;
            }

            if (self.Transform_HeroIcon.childCount == 0)
            {
                return;
            }

            self.CopyModelGameObject = UnityEngine.Object.Instantiate(self.Transform_HeroIcon.GetChild(0).gameObject, self.GameObject.transform.parent.parent);
            self.CopyModelGameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            
            
            self.Text_HeroName.gameObject.SetActive(false);
            self.Transform_HeroIcon.gameObject.SetActive(false);
            
            self.IsDrag = true;
        }

        private static void OnDrag(this UIFormationSlotItem self, PointerEventData pdata)
        {
            if (!self.IsDrag)
            {
                return;
            }
            
            Vector2 localPoint = new Vector2();
            RectTransform canvas = self.GameObject.transform.parent.parent.GetComponent<RectTransform>();
            Camera uiCamera = self.Root().GetComponent<GlobalComponent>().UICamera.GetComponent<Camera>();
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, pdata.position, uiCamera, out localPoint);
            self.CopyModelGameObject.transform.localPosition = localPoint;
        }

        private static void OnPointerUp(this UIFormationSlotItem self, PointerEventData pdata)
        {
            if (self.IsDrag)
            {
                return;
            }

            self.GetParent<UIHeroFormationComponent>().OnUnloadHero(self.HeroId, self.SlotIndex).Coroutine();
        }

        private static void OnEndDrag(this UIFormationSlotItem self, PointerEventData pdata)
        {
            if (!self.IsDrag)
            {
                return;
            }

            RectTransform canvas = self.GameObject.transform.parent.parent.GetComponent<RectTransform>();
            GraphicRaycaster gr = canvas.GetComponent<GraphicRaycaster>();
            List<RaycastResult> results = new List<RaycastResult>();
            gr.Raycast(pdata, results);

            for (int i = 0; i < results.Count; i++)
            {
                string name = results[i].gameObject.name;
                if (name != "EventTrigger_Click")
                {
                    continue;
                }

                name = results[i].gameObject.transform.parent.name;
                // UIFormationSlotItem_
                int index = int.Parse(name.Substring(20, name.Length - 20));

                self.GetParent<UIHeroFormationComponent>().OnSelectHero(self.HeroId, index).Coroutine();
                
                break;
            }
            
            if (self.CopyModelGameObject != null)
            {
                UnityEngine.Object.Destroy(self.CopyModelGameObject);
                self.CopyModelGameObject = null;
            }

            self.IsDrag = false;
        }

        public static async ETTask UpdateInfo(this UIFormationSlotItem self, long heroId, int slotIndex)
        {
            self.HeroId = heroId;
            self.SlotIndex = slotIndex;

            HeroComponentC heroComponentC = self.Root().GetComponent<HeroComponentC>();
            Hero hero = heroComponentC.GetHero(heroId);

            if (hero == null)
            {
                self.HeroId = 0;
                self.Text_HeroName.gameObject.SetActive(false);
                self.Transform_HeroIcon.gameObject.SetActive(false);
                return;
            }

            HeroConfig heroConfig = HeroConfigCategory.Instance.Get(hero.ConfigId);
            self.Text_HeroName.gameObject.SetActive(true);
            self.Text_HeroName.SetText(heroConfig.HeroName);
            self.Transform_HeroIcon.gameObject.SetActive(true);
            UICommonHelper.DestoryChild(self.Transform_HeroIcon.gameObject);
            string path = ABPathHelper.GetUIUnitPath(ABUnitType.Hero, heroConfig.HeroModelID);
            GameObject model = await self.Root().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<GameObject>(path);
            UnityEngine.Object.Instantiate(model, self.Transform_HeroIcon);
        }
    }
}