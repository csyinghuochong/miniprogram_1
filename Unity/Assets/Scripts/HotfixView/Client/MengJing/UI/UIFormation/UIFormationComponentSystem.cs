using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIFormationComponent))]
    [FriendOf(typeof(UIFormationComponent))]
    public static partial class UIFormationComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UIFormationComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.Button_Close = rc.Get<GameObject>("Button_Close").GetComponent<Button>();
            self.Button_Plan_1 = rc.Get<GameObject>("Button_Plan_1").GetComponent<Button>();
            self.Button_Plan_2 = rc.Get<GameObject>("Button_Plan_2").GetComponent<Button>();

            self.Button_Close.onClick.AddListener(() => { self.Root().GetComponent<UIComponent>().Remove(UIType.UIFormation); });
            self.Button_Plan_1.onClick.AddListener(() => { self.SetShowPlan(1).Coroutine(); });
            self.Button_Plan_2.onClick.AddListener(() => { self.SetShowPlan(2).Coroutine(); });
            self.UIFormationSlotItem_1 = self.AddChild<UIFormationSlotItem, GameObject>(rc.Get<GameObject>("UIFormationSlotItem_1"));
            self.UIFormationSlotItem_2 = self.AddChild<UIFormationSlotItem, GameObject>(rc.Get<GameObject>("UIFormationSlotItem_2"));
            self.UIFormationSlotItem_3 = self.AddChild<UIFormationSlotItem, GameObject>(rc.Get<GameObject>("UIFormationSlotItem_3"));
            self.UIFormationSlotItem_4 = self.AddChild<UIFormationSlotItem, GameObject>(rc.Get<GameObject>("UIFormationSlotItem_4"));
            self.UIFormationSlotItem_5 = self.AddChild<UIFormationSlotItem, GameObject>(rc.Get<GameObject>("UIFormationSlotItem_5"));

            self.SetShowPlan(self.Root().GetComponent<HeroComponentC>().CurrentFormationIndex).Coroutine();
        }

        private static async ETTask SetShowPlan(this UIFormationComponent self, int index)
        {
            if (index != self.Root().GetComponent<HeroComponentC>().CurrentFormationIndex)
            {
                int error = await HeroHelper.SetHeroCurrentFormationIndex(self.Root(), index);
                if (error != ErrorCode.ERR_Success)
                {
                    return;
                }
            }
            
            self.Button_Plan_1.transform.Find("Image_On").gameObject.SetActive(index == 1);
            self.Button_Plan_1.transform.Find("Image_Off").gameObject.SetActive(index != 1);
            self.Button_Plan_2.transform.Find("Image_On").gameObject.SetActive(index == 2);
            self.Button_Plan_2.transform.Find("Image_Off").gameObject.SetActive(index != 2);

            self.UpdateSlotItemList();
        }

        private static void UpdateSlotItemList(this UIFormationComponent self)
        {
            HeroComponentC heroComponent = self.Root().GetComponent<HeroComponentC>();
            List<long> currentFormation = heroComponent.GetFormation(heroComponent.CurrentFormationIndex);
            self.UIFormationSlotItem_1.UpdateInfo(heroComponent.GetHero(currentFormation[0])).Coroutine();
            self.UIFormationSlotItem_2.UpdateInfo(heroComponent.GetHero(currentFormation[1])).Coroutine();
            self.UIFormationSlotItem_3.UpdateInfo(heroComponent.GetHero(currentFormation[2])).Coroutine();
            self.UIFormationSlotItem_4.UpdateInfo(heroComponent.GetHero(currentFormation[3])).Coroutine();
            self.UIFormationSlotItem_5.UpdateInfo(heroComponent.GetHero(currentFormation[4])).Coroutine();
        }
    }
}