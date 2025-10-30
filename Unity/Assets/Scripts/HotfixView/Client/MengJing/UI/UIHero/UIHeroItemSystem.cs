using Cysharp.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIHeroItem))]
    [FriendOf(typeof(UIHeroItem))]
    public static partial class UIHeroItemSystem
    {
        [EntitySystem]
        private static void Awake(this UIHeroItem self, GameObject gameObject)
        {
            self.GameObject = gameObject;

            ReferenceCollector rc = gameObject.GetComponent<ReferenceCollector>();

            self.Text_HeroName = rc.Get<GameObject>("Text_HeroName").GetComponent<TMP_Text>();
            self.Image_HeroIcon = rc.Get<GameObject>("Image_HeroIcon").GetComponent<Image>();
            self.Transform_HeroStar = rc.Get<GameObject>("Transform_HeroStar").transform;
            self.Text_HeroCombatPower = rc.Get<GameObject>("Text_HeroCombatPower").GetComponent<TMP_Text>();
            self.Button_Click = rc.Get<GameObject>("Button_Click").GetComponent<Button>();
        }

        public static async ETTask UpdateInfo(this UIHeroItem self, Hero hero)
        {
            self.HeroId = hero.Id;
            HeroConfig heroConfig = HeroConfigCategory.Instance.Get(hero.ConfigId);
            self.Text_HeroName.text = heroConfig.HeroName;
            string path = ABPathHelper.GetAtlasPath_2(ABAtlasTypes.HeroIcon, heroConfig.HeroHeadIcon);
            self.Image_HeroIcon.sprite = await self.Root().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<Sprite>(path);

            UICommonHelper.HideChild(self.Transform_HeroStar.gameObject);
            for (int i = 0; i < hero.Star; i++)
            {
                if (i < self.Transform_HeroStar.childCount)
                {
                    self.Transform_HeroStar.GetChild(i).gameObject.SetActive(true);
                }
                else
                {
                    GameObject prefab = self.Transform_HeroStar.GetChild(0).gameObject;
                    GameObject go = UnityEngine.Object.Instantiate(prefab, self.Transform_HeroStar);
                    go.SetActive(true);
                }
            }

            self.Text_HeroCombatPower.SetText(hero.NumericDic[NumericType.CombatPower]);
        }
    }
}