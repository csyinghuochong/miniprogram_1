using System;
using Cysharp.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UICommonItem))]
    [FriendOf(typeof(UICommonItem))]
    public static partial class UICommonItemSystem
    {
        [EntitySystem]
        private static void Awake(this UICommonItem self, GameObject gameObject)
        {
            self.GameObject = gameObject;

            ReferenceCollector rc = gameObject.GetComponent<ReferenceCollector>();

            self.Item = rc.Get<GameObject>("Item");
            self.Image_ItemNull = rc.Get<GameObject>("Image_ItemNull").GetComponent<Image>();
            self.Image_ItemQuality = rc.Get<GameObject>("Image_ItemQuality").GetComponent<Image>();
            self.Image_On = rc.Get<GameObject>("Image_On").GetComponent<Image>();
            self.Image_ItemIcon = rc.Get<GameObject>("Image_ItemIcon").GetComponent<Image>();
            self.Text_ItemNum = rc.Get<GameObject>("Text_ItemNum").GetComponent<TMP_Text>();
            self.Button_Click = rc.Get<GameObject>("Button_Click").GetComponent<Button>();
            self.Image_Pressed = rc.Get<GameObject>("Image_Pressed").GetComponent<Image>();
            self.EventTrigger_Click = rc.Get<GameObject>("EventTrigger_Click").GetComponent<EventTrigger>();
            self.Image_Selected = rc.Get<GameObject>("Image_Selected").GetComponent<Image>();
            self.Image_Equipped = rc.Get<GameObject>("Image_Equipped").GetComponent<Image>();

            self.Image_On.gameObject.SetActive(false);
            self.Image_Equipped.gameObject.SetActive(false);
            self.Image_Selected.gameObject.SetActive(false);

            self.Button_Click.AddListener(self.OnClick);

            self.EventTrigger_Click.AddEventTrigger(self.OnPointerDown, EventTriggerType.PointerDown);
            self.EventTrigger_Click.AddEventTrigger(self.OnBeginDrag, EventTriggerType.BeginDrag);
            self.EventTrigger_Click.AddEventTrigger(self.OnDrag, EventTriggerType.Drag);
            self.EventTrigger_Click.AddEventTrigger(self.OnPointerUp, EventTriggerType.PointerUp);
            self.EventTrigger_Click.AddEventTrigger(self.OnEndDrag, EventTriggerType.EndDrag);
        }

        private static void OnClick(this UICommonItem self)
        {
        }

        private static void OnPointerDown(this UICommonItem self, PointerEventData eventData)
        {
            self.IsDrag = false;
            self.IsPressing = true;
            self.LongPressing().Coroutine();
        }

        private static async ETTask LongPressing(this UICommonItem self)
        {
            self.PressedTime = 0;
            self.Image_Pressed.fillAmount = 0;
            long lastTime = TimeInfo.Instance.ClientNow();
            while (true)
            {
                await self.Root().GetComponent<TimerComponent>().WaitFrameAsync();

                if (self.IsDisposed)
                {
                    return;
                }

                if (self.IsDrag)
                {
                    // 提前拖动了
                    self.Image_Pressed.fillAmount = 0;
                    return;
                }

                if (!self.IsPressing)
                {
                    // 提前松开了
                    self.Image_Pressed.fillAmount = 0;
                    return;
                }

                long currentTime = TimeInfo.Instance.ClientNow();
                self.PressedTime += currentTime - lastTime;
                lastTime = currentTime;

                self.Image_Pressed.fillAmount = self.PressedTime * 1f / self.PressedTriggerTime;

                if (self.PressedTime >= self.PressedTriggerTime)
                {
                    Log.Warning("长按");
                    self.OnLongPressed?.Invoke();
                    self.Image_Pressed.fillAmount = 0;
                    self.IsPressing = false;
                    return;
                }
            }
        }

        private static void OnBeginDrag(this UICommonItem self, PointerEventData eventData)
        {
            self.IsDrag = true;

            ScrollRect scrollRect = self.GameObject.GetComponentInParent<ScrollRect>();
            if (scrollRect != null)
            {
                scrollRect.OnBeginDrag(eventData);
            }
        }

        private static void OnDrag(this UICommonItem self, PointerEventData eventData)
        {
            ScrollRect scrollRect = self.GameObject.GetComponentInParent<ScrollRect>();
            if (scrollRect != null)
            {
                scrollRect.OnDrag(eventData);
            }
        }

        private static void OnPointerUp(this UICommonItem self, PointerEventData eventData)
        {
            if (!self.IsDrag && self.IsPressing)
            {
                Log.Warning("短按");
                self.OnItemPointerUp?.Invoke();
            }

            self.IsDrag = false;
            self.IsPressing = false;
        }

        private static void OnEndDrag(this UICommonItem self, PointerEventData eventData)
        {
            ScrollRect scrollRect = self.GameObject.GetComponentInParent<ScrollRect>();
            if (scrollRect != null)
            {
                scrollRect.OnEndDrag(eventData);
            }
        }

        public static async ETTask UpdateInfo(this UICommonItem self, Item item, Action<long> onItemClick = null)
        {
            self.ItemId = item.Id;
            self.OnItemClick = onItemClick;
            ItemConfig itemConfig = ItemConfigCategory.Instance.Get(item.ConfigId);

            self.Text_ItemNum.SetText(item.Num);

            string qualityPath = ABPathHelper.GetAtlasPath_2(ABAtlasTypes.ItemQualityIcon, ZString.Format("quality{0}", itemConfig.ItemQuality));
            self.Image_ItemQuality.overrideSprite = await self.Root().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<Sprite>(qualityPath);

            string path = ABPathHelper.GetAtlasPath_2(ABAtlasTypes.ItemIcon, itemConfig.Icon);
            self.Image_ItemIcon.overrideSprite = await self.Root().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<Sprite>(path);
        }

        public static async ETTask UpdateInfo(this UICommonItem self, int itemConfigId, int num)
        {
            ItemConfig itemConfig = ItemConfigCategory.Instance.Get(itemConfigId);

            self.Text_ItemNum.SetText(num);

            string qualityPath = ABPathHelper.GetAtlasPath_2(ABAtlasTypes.ItemQualityIcon, ZString.Format("quality{0}", itemConfig.ItemQuality));
            self.Image_ItemQuality.overrideSprite = await self.Root().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<Sprite>(qualityPath);

            string path = ABPathHelper.GetAtlasPath_2(ABAtlasTypes.ItemIcon, itemConfig.Icon);
            self.Image_ItemIcon.overrideSprite = await self.Root().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<Sprite>(path);
        }

        public static void SetSelected(this UICommonItem self, long itemId)
        {
            self.Image_Selected.gameObject.SetActive(self.ItemId == itemId);
        }

        public static void SetImageOn(this UICommonItem self, long itemId)
        {
            self.Image_Selected.gameObject.SetActive(self.ItemId == itemId);
        }
    }
}