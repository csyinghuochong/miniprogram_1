using TMPro;
using Cysharp.Text;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIHeroAttributesComponent))]
    [FriendOf(typeof(UIHeroAttributesComponent))]
    public static partial class UIHeroAttributesComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UIHeroAttributesComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.Button_Close = rc.Get<GameObject>("Button_Close").GetComponent<Button>();
            self.Content_UIBaseAttributeItem = rc.Get<GameObject>("Content_UIBaseAttributeItem").transform;
            self.UIBaseAttributeItem = rc.Get<GameObject>("UIBaseAttributeItem");
            self.UIBaseAttributeItem.SetActive(false);
            self.Content_UIOtherAttributeItem = rc.Get<GameObject>("Content_UIOtherAttributeItem").transform;
            self.UIOtherAttributeItem = rc.Get<GameObject>("UIOtherAttributeItem");
            self.UIOtherAttributeItem.SetActive(false);

            self.Button_Close.AddListener(() => { self.Root().GetComponent<UIComponent>().Remove(UIType.UIHeroAttributes); });

        }

        [EntitySystem]
        private static void Destroy(this UIHeroAttributesComponent self)
        {
        }

        public static void UpdateAttributes(this UIHeroAttributesComponent self, long currentHeroId)
        {
            self.CurrentHeroId = currentHeroId;

            HeroComponentC heroComponent = self.Root().GetComponent<HeroComponentC>();
            Hero hero = heroComponent.GetHero(self.CurrentHeroId);

            // 基础属性
            self.ShowBaseStatItem(1, "生命", hero.NumericDic[NumericType.Base_MaxHp_Base].ToString());
            self.ShowBaseStatItem(2, "攻击", ZString.Format("{0}-{1}", hero.NumericDic[NumericType.Base_MinAct_Base], hero.NumericDic[NumericType.Base_MaxAct_Base]));
            self.ShowBaseStatItem(3, "物防", ZString.Format("{0}-{1}", hero.NumericDic[NumericType.Base_MinDef_Base], hero.NumericDic[NumericType.Base_MaxDef_Base]));
            self.ShowBaseStatItem(4, "魔防", ZString.Format("{0}-{1}", hero.NumericDic[NumericType.Base_MinAdf_Base], hero.NumericDic[NumericType.Base_MaxAdf_Base]));

            // 特殊属性
            self.ShowOtherStatItem(1, "暴击", ZString.Format("{0:0.#}%", hero.NumericDic[NumericType.Base_Cri_Base] / 10000f * 100f));
            self.ShowOtherStatItem(2, "抗暴", ZString.Format("{0:0.#}%", hero.NumericDic[NumericType.Base_ReCri_Base] / 10000f * 100f));
            self.ShowOtherStatItem(3, "闪避", ZString.Format("{0:0.#}%", hero.NumericDic[NumericType.Base_Eva_Base] / 10000f * 100f));
            self.ShowOtherStatItem(4, "命中", ZString.Format("{0:0.#}%", hero.NumericDic[NumericType.Base_Hit_Base] / 10000f * 100f));
            self.ShowOtherStatItem(5, "伤害减免", ZString.Format("{0:0.#}%", hero.NumericDic[NumericType.Base_HitDamageLessPro_Base] / 10000f * 100f));
        }

        private static void ShowBaseStatItem(this UIHeroAttributesComponent self, int index, string name, string value)
        {
            Transform item = null;
            if (self.Content_UIBaseAttributeItem.childCount <= index)
            {
                item = UnityEngine.Object.Instantiate(self.UIBaseAttributeItem, self.Content_UIBaseAttributeItem).transform;
            }
            else
            {
                item = self.Content_UIBaseAttributeItem.GetChild(index);
            }

            if (item == null)
            {
            }

            item.gameObject.SetActive(true);

            ReferenceCollector rc = item.GetComponent<ReferenceCollector>();
            rc.Get<GameObject>("Text_Name").GetComponent<TMP_Text>().SetText(name);
            rc.Get<GameObject>("Text_Value").GetComponent<TMP_Text>().SetText(value);
        }

        private static void ShowOtherStatItem(this UIHeroAttributesComponent self, int index, string name, string value)
        {
            Transform item = null;
            if (self.Content_UIOtherAttributeItem.childCount <= index)
            {
                item = UnityEngine.Object.Instantiate(self.UIOtherAttributeItem, self.Content_UIOtherAttributeItem).transform;
            }
            else
            {
                item = self.Content_UIOtherAttributeItem.GetChild(index);
            }

            item.gameObject.SetActive(true);

            ReferenceCollector rc = item.GetComponent<ReferenceCollector>();
            rc.Get<GameObject>("Text_Name").GetComponent<TMP_Text>().SetText(name);
            rc.Get<GameObject>("Text_Value").GetComponent<TMP_Text>().SetText(value);
        }
    }
}