using Cysharp.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIBattlePassComponent))]
    [FriendOf(typeof(UIBattlePassComponent))]
    public static partial class UIBattlePassComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UIBattlePassComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.Button_Close = rc.Get<GameObject>("Button_Close").GetComponent<Button>();
            self.Content_UIBattlePassItem = rc.Get<GameObject>("Content_UIBattlePassItem").transform;
            self.UIBattlePassItem = rc.Get<GameObject>("UIBattlePassItem");
            self.UIBattlePassItem.gameObject.SetActive(false);
            self.Button_GetAllReward = rc.Get<GameObject>("Button_GetAllReward").GetComponent<Button>();

            self.AddComponent<UICommonHuoBiSetComponent, GameObject>(rc.Get<GameObject>("UICommonHuoBiSet"));
            self.Button_Close.AddListener(() => { self.Root().GetComponent<UIComponent>().Remove(UIType.UIBattlePass); });
        }

        [EntitySystem]
        private static void Destroy(this UIBattlePassComponent self)
        {
            self.UIBattlePassItemList.Clear();
            self.UIBattlePassItem = null;
        }
    }
}