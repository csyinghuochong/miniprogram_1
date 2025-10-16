using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UITeamItem))]
    [FriendOf(typeof(UITeamItem))]
    public static partial class UITeamItemSystem
    {
        [EntitySystem]
        private static void Awake(this UITeamItem self, GameObject gameObject)
        {
            self.GameObject = gameObject;

            ReferenceCollector rc = gameObject.GetComponent<ReferenceCollector>();

            self.Image_HeroIcon = rc.Get<GameObject>("Image_HeroIcon").GetComponent<Image>();
            self.Text_Lv = rc.Get<GameObject>("Text_Lv").GetComponent<TMP_Text>();
            self.Text_Name = rc.Get<GameObject>("Text_Name").GetComponent<TMP_Text>();
            self.Text_CP = rc.Get<GameObject>("Text_CP").GetComponent<TMP_Text>();
            self.Button_Up = rc.Get<GameObject>("Button_Up").GetComponent<Button>();
        }

        public static async ETTask UpdateInfo(this UITeamItem self, Hero hero)
        {
            self.HeroId = hero.Id;
            HeroConfig heroConfig = HeroConfigCategory.Instance.Get(hero.ConfigId);
            string path = ABPathHelper.GetAtlasPath_2(ABAtlasTypes.HeroIcon, heroConfig.HeroHeadIcon);
            self.Image_HeroIcon.sprite = await self.Root().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<Sprite>(path);
            self.Text_Name.text = heroConfig.HeroName;
        }
    }
}