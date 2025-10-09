using TMPro;
using UnityEngine;
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

            self.Text_Lv = rc.Get<GameObject>("Text_Lv").GetComponent<TMP_Text>();
            self.Image_HeroIcon = rc.Get<GameObject>("Image_HeroIcon").GetComponent<Image>();
            self.Text_HeroName = rc.Get<GameObject>("Text_HeroName").GetComponent<TMP_Text>();
            self.Button_Click = rc.Get<GameObject>("Button_Click").GetComponent<Button>();
        }

        public static async ETTask UpdateInfo(this UIFormationSlotItem self, Hero hero)
        {
            if (hero == null)
            {
                self.HeroId = 0;
                self.Text_Lv.gameObject.SetActive(false);
                self.Text_HeroName.gameObject.SetActive(false);
                self.Image_HeroIcon.gameObject.SetActive(false);
                return;
            }

            self.HeroId = hero.Id;

            self.Text_Lv.gameObject.SetActive(true);
            self.Text_Lv.SetText("Lv.{0}", hero.Lv);
            HeroConfig heroConfig = HeroConfigCategory.Instance.Get(hero.ConfigId);
            self.Text_HeroName.gameObject.SetActive(true);
            self.Text_HeroName.SetText(heroConfig.HeroName);
            string path = ABPathHelper.GetAtlasPath_2(ABAtlasTypes.HeroIcon, heroConfig.HeroHeadIcon);
            self.Image_HeroIcon.gameObject.SetActive(true);
            self.Image_HeroIcon.sprite = await self.Root().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<Sprite>(path);
        }
    }
}