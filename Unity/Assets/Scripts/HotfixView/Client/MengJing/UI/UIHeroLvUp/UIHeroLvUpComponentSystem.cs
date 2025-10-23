using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIHeroLvUpComponent))]
    [FriendOf(typeof(UIHeroLvUpComponent))]
    public static partial class UIHeroLvUpComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UIHeroLvUpComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.Button_Close = rc.Get<GameObject>("Button_Close").GetComponent<Button>();
            self.Text_HeroName = rc.Get<GameObject>("Text_HeroName").GetComponent<TMP_Text>();
            self.Text_HeroLv = rc.Get<GameObject>("Text_HeroLv").GetComponent<TMP_Text>();
            self.Slider_HeroExp = rc.Get<GameObject>("Slider_HeroExp").GetComponent<Slider>();
            self.Text_HeroExp = rc.Get<GameObject>("Text_HeroExp").GetComponent<TMP_Text>();
            self.Content_UICommonItem = rc.Get<GameObject>("UICommonItem").transform;
            self.Text_Tip = rc.Get<GameObject>("Text_Tip").GetComponent<TMP_Text>();
            self.Button_Use_10 = rc.Get<GameObject>("Button_Use_10").GetComponent<Button>();
            self.Button_Use_1 = rc.Get<GameObject>("Button_Use_1").GetComponent<Button>();
            self.UICommonItem = rc.Get<GameObject>("UICommonItem");

            self.Button_Close.AddListener(() => { self.Root().GetComponent<UIComponent>().Remove(UIType.UIHeroLvUp); });
        }

        [EntitySystem]
        private static void Destroy(this UIHeroLvUpComponent self)
        {
        }
    }
}