using TMPro;
using UnityEngine;
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

            self.Image_ItemIcon = rc.Get<GameObject>("Image_ItemIcon").GetComponent<Image>();
            self.Text_ItemName = rc.Get<GameObject>("Text_ItemName").GetComponent<TMP_Text>();
            self.Text_ItemNum = rc.Get<GameObject>("Text_ItemNum").GetComponent<TMP_Text>();
            self.Button_Click = rc.Get<GameObject>("Button_Click").GetComponent<Button>();

            self.Button_Click.onClick.AddListener(() => { self.OnClick().Coroutine(); });
        }

        private static async ETTask OnClick(this UICommonItem self)
        {
            UI uI = await self.Root().GetComponent<UIComponent>().Create(UIType.UIItemTip);
            if (uI != null)
            {
                uI.GetComponent<UIItemTipComponent>().UpdateInfo(self.ItemId);
            }
        }

        public static async ETTask UpdateInfo(this UICommonItem self, Item item)
        {
            self.ItemId = item.Id;
            ItemConfig itemConfig = ItemConfigCategory.Instance.Get(item.ConfigId);
            self.Text_ItemName.SetText(itemConfig.ItemName);
            self.Text_ItemNum.SetText("x{0}", item.Num);
            string path = ABPathHelper.GetAtlasPath_2(ABAtlasTypes.ItemIcon, itemConfig.Icon);
            self.Image_ItemIcon.sprite = await self.Root().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<Sprite>(path);
        }
    }
}