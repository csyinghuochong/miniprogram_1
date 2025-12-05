using Cysharp.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIStoreRefTipComponent))]
    [FriendOf(typeof(UIStoreRefTipComponent))]
    public static partial class UIStoreRefTipComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UIStoreRefTipComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.Text_Tip = rc.Get<GameObject>("Text_Tip").GetComponent<TMP_Text>();
            self.Text_StoreRefreshNum = rc.Get<GameObject>("Text_StoreRefreshNum").GetComponent<TMP_Text>();
            self.Button_Refresh = rc.Get<GameObject>("Button_Refresh").GetComponent<Button>();
            self.Button_Close = rc.Get<GameObject>("Button_Close").GetComponent<Button>();

            self.Button_Refresh.AddListener((() => { self.OnRefresh(); }));
            self.Button_Close.AddListener(() => { self.OnClose(); });
        }

        public static void Init(this UIStoreRefTipComponent self, int num)
        {
            self.StoreRefreshNum = num;
            self.Text_StoreRefreshNum.SetTextFormat("剩余刷新次数：{0}", self.StoreRefreshNum);

            self.Text_Tip.SetTextFormat("是否花费{0}钻石刷新商店", ConfigData.StoreRefreshCost[0].ItemNum);
        }

        public static void OnRefresh(this UIStoreRefTipComponent self)
        {
            UI ui = self.Root().GetComponent<UIComponent>().Get(UIType.UIStore);
            if (ui == null)
            {
                return;
            }

            UIStoreComponent uiStoreComponent = ui.GetComponent<UIStoreComponent>();
            uiStoreComponent.StoreRefreshHandler().Coroutine();
            self.OnClose();
        }

        private static void OnClose(this UIStoreRefTipComponent self)
        {
            self.Root().GetComponent<UIComponent>().Remove(UIType.UIStoreRefTip);
        }
    }
}