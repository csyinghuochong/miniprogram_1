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
        }

        public static async ETTask UpdateInfo(this UIFormationSlotItem self, Hero hero, int slotIndex)
        {
            self.SlotIndex = slotIndex;

            if (hero == null)
            {
                self.HeroId = 0;
                self.Text_HeroName.gameObject.SetActive(false);
                self.Transform_HeroIcon.gameObject.SetActive(false);
                return;
            }

            self.HeroId = hero.Id;

            HeroConfig heroConfig = HeroConfigCategory.Instance.Get(hero.ConfigId);
            self.Text_HeroName.gameObject.SetActive(true);
            self.Text_HeroName.SetText(heroConfig.HeroName);
            self.Transform_HeroIcon.gameObject.SetActive(true);
            string path = ABPathHelper.GetUIUnitPath(ABUnitType.Hero, heroConfig.HeroModelID);
            GameObject model = await self.Root().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<GameObject>(path);
            UICommonHelper.DestoryChild(self.Transform_HeroIcon.gameObject);
            UnityEngine.Object.Instantiate(model, self.Transform_HeroIcon);
        }

        private static void OnClick(this UIFormationSlotItem self)
        {
            self.GetParent<UIHeroFormationComponent>().OnUnloadHero(self.HeroId, self.SlotIndex).Coroutine();
        }
    }
}