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
            self.Slider_ShardNum = rc.Get<GameObject>("Slider_ShardNum").GetComponent<Slider>();
            self.Text_ShardNum = rc.Get<GameObject>("Text_ShardNum").GetComponent<TMP_Text>();
            self.Text_NotHave = rc.Get<GameObject>("Text_NotHave").GetComponent<TMP_Text>();
            self.Button_Click = rc.Get<GameObject>("Button_Click").GetComponent<Button>();
        }

        public static async ETTask UpdateInfo(this UIHeroItem self, Hero hero)
        {
            self.HeroId = hero.Id;

            self.Transform_HeroStar.gameObject.SetActive(true);
            self.Text_HeroCombatPower.gameObject.SetActive(true);
            self.Slider_ShardNum.gameObject.SetActive(false);
            self.Text_ShardNum.gameObject.SetActive(false);
            self.Text_NotHave.gameObject.SetActive(false);
            self.Button_Click.gameObject.SetActive(false);

            HeroConfig heroConfig = HeroConfigCategory.Instance.Get(hero.ConfigId);
            self.Text_HeroName.text = heroConfig.HeroName;
            string path = ABPathHelper.GetAtlasPath_2(ABAtlasTypes.HeroIcon, heroConfig.HeroHeadIcon);
            self.Image_HeroIcon.sprite = await self.Root().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<Sprite>(path);

            UICommonHelper.HideChild(self.Transform_HeroStar.gameObject);
            for (int i = 0; i < heroConfig.HeroUpStarNeed.Length; i++)
            {
                if (i < self.Transform_HeroStar.childCount)
                {
                    self.Transform_HeroStar.GetChild(i).gameObject.SetActive(true);
                    self.Transform_HeroStar.GetChild(i).GetChild(0).gameObject.SetActive(false);
                }
                else
                {
                    GameObject prefab = self.Transform_HeroStar.GetChild(0).gameObject;
                    GameObject go = UnityEngine.Object.Instantiate(prefab, self.Transform_HeroStar);
                    go.SetActive(true);
                }

                GameObject star = self.Transform_HeroStar.GetChild(i).GetChild(0).gameObject;
                star.SetActive(hero.Star > i);
            }

            self.Text_HeroCombatPower.SetText(hero.NumericDic[NumericType.CombatPower]);
        }

        public static async ETTask UpdateInfo(this UIHeroItem self, int heroConfigId)
        {
            self.Transform_HeroStar.gameObject.SetActive(false);
            self.Text_HeroCombatPower.gameObject.SetActive(false);
            self.Slider_ShardNum.gameObject.SetActive(true);
            self.Text_ShardNum.gameObject.SetActive(true);
            self.Text_NotHave.gameObject.SetActive(true);
            self.Button_Click.gameObject.SetActive(false);

            HeroConfig heroConfig = HeroConfigCategory.Instance.Get(heroConfigId);
            self.Text_HeroName.text = heroConfig.HeroName;
            string path = ABPathHelper.GetAtlasPath_2(ABAtlasTypes.HeroIcon, heroConfig.HeroHeadIcon);
            self.Image_HeroIcon.sprite = await self.Root().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<Sprite>(path);

            int itemConfigId = 0;
            int needNum = 0;
            foreach (ItemConfig config in ItemConfigCategory.Instance.DataList)
            {
                if (config.ItemSubType == ItemSubType.HeroShard)
                {
                    if (config.ItemUseParInt[0] == heroConfigId)
                    {
                        itemConfigId = config.Id;
                        needNum = config.ItemUseParInt[1];
                        break;
                    }
                }
            }

            if (itemConfigId == 0)
            {
                Log.Error(ZString.Format("没有配置英雄碎片 HeroConfigId {0} !!!", heroConfigId));
                return;
            }

            int num = self.Root().GetComponent<InventoryComponentC>().GetItemNum(itemConfigId);
            self.Slider_ShardNum.value = num / (float)needNum;
            self.Text_ShardNum.SetTextFormat("{0}/{1}", num, needNum);

            if (num >= needNum)
            {
                self.Button_Click.gameObject.SetActive(true);
                long itemId = 0;
                foreach (Item item in self.Root().GetComponent<InventoryComponentC>().GetItemsBySubType(ItemSubType.HeroShard))
                {
                    if (ItemConfigCategory.Instance.Get(item.ConfigId).ItemUseParInt[0] == heroConfigId)
                    {
                        // 随便拿一个
                        itemId = item.Id;
                        break;
                    }
                }

                self.Button_Click.AddListener(() => { self.OnHeChenHero(itemId).Coroutine(); });
            }
        }

        private static async ETTask OnHeChenHero(this UIHeroItem self, long itemId)
        {
            int error = await ClientInventoryHelper.UseItem(self.Root(), itemId);
            if (error != ErrorCode.ERR_Success)
            {
                return;
            }

            UIHeroListComponent uiHeroListComponent = self.GetParent<UIHeroListComponent>();
            uiHeroListComponent.SetShowType(uiHeroListComponent.CurrentPage);

            self.Root().GetComponent<FloatingTextComponent>().ShowTipText("合成成功");
        }
    }
}