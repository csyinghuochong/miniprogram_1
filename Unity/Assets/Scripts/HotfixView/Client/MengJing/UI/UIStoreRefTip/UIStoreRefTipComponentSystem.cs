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
            self.Button_Refresh = rc.Get<GameObject>("Button_Refresh").GetComponent<Button>();
            self.Button_Close = rc.Get<GameObject>("Button_Close").GetComponent<Button>();

            self.Button_Refresh.AddListener((() => { self.OnRefresh(); }));
            self.Button_Close.AddListener(() => { self.Root().GetComponent<UIComponent>().Remove(UIType.UIStoreRefTip); });
        }

        public static void OnRefresh(this UIStoreRefTipComponent self)
        {
            UI ui = self.Root().GetComponent<UIComponent>().Get(UIType.UIStore);

            if (ui == null)
            {
                return;
            }

            UIStoreComponent uiStoreComponent = ui.GetComponent<UIStoreComponent>();
            self.Root().GetComponent<UIComponent>().Remove(UIType.UIStoreRefTip);
            uiStoreComponent.OnButton_RefreshTime().Coroutine();
        }
    }
}