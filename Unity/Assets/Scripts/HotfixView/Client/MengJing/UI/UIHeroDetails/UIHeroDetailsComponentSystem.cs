using System.Collections.Generic;
using Cysharp.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIHeroDetailsComponent))]
    [FriendOf(typeof(UIHeroDetailsComponent))]
    public static partial class UIHeroDetailsComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UIHeroDetailsComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.Button_Close = rc.Get<GameObject>("Button_Close").GetComponent<Button>();
            self.Spine_HeroModel = rc.Get<GameObject>("Spine_HeroModel").transform;
            self.Text_HeroCP = rc.Get<GameObject>("Text_HeroCP").GetComponent<TMP_Text>();
            self.Text_HeroQuality = rc.Get<GameObject>("Text_HeroQuality").GetComponent<TMP_Text>();
            self.Text_HeroName = rc.Get<GameObject>("Text_HeroName").GetComponent<TMP_Text>();
            self.Transform_HeroStar = rc.Get<GameObject>("Transform_HeroStar").transform;
            self.Transform_HeroStar.GetChild(0).gameObject.SetActive(false);
            self.Text_HeroType = rc.Get<GameObject>("Text_HeroType").GetComponent<TMP_Text>();
            self.Text_HeroLv = rc.Get<GameObject>("Text_HeroLv").GetComponent<TMP_Text>();
            self.Content_UIBaseAttributeItem = rc.Get<GameObject>("Content_UIBaseAttributeItem").transform;
            self.UIBaseAttributeItem = rc.Get<GameObject>("UIBaseAttributeItem");
            self.UIBaseAttributeItem.SetActive(false);
            self.Content_UISkillItem = rc.Get<GameObject>("Content_UISkillItem").transform;
            self.UISkillItem = rc.Get<GameObject>("UISkillItem");
            self.UISkillItem.SetActive(false);

            self.Button_Close.AddListener(() => { self.Root().GetComponent<UIComponent>().Remove(UIType.UIHeroDetails); });
        }

        [EntitySystem]
        private static void Destroy(this UIHeroDetailsComponent self)
        {
            self.UISkillItemList.Clear();
            self.UISkillItemList = null;
        }
        
        public static async ETTask UpdateHeroDetails(this UIHeroDetailsComponent self, int heroConfigId)
        {
            self.CurrentHeroConfigId = heroConfigId;

            HeroConfig heroConfig = HeroConfigCategory.Instance.Get(self.CurrentHeroConfigId);
            self.Text_HeroName.SetText(heroConfig.HeroName);
            self.Text_HeroType.SetText(heroConfig.HeroType == 1 ? "近战" : "远程");
            self.Text_HeroLv.SetText("等级：999");
            
            //满级战力
            Dictionary<int,long> numericDic = CommonHelp.CalculateHeroNumericByConfig(self.CurrentHeroConfigId, 999, 5);
            self.Text_HeroCP.SetText("战力：" + numericDic[NumericType.CombatPower]);
            
            string path = ABPathHelper.GetUIUnitPath(ABUnitType.Hero, heroConfig.HeroModelID);
            GameObject model = await self.Root().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<GameObject>(path);
            model.transform.localScale = new Vector3(3f, 3f, 1f);
            UICommonHelper.DestoryChild(self.Spine_HeroModel.gameObject);
            UnityEngine.Object.Instantiate(model, self.Spine_HeroModel);
            
            // 星级
            UICommonHelper.HideChild(self.Transform_HeroStar.gameObject);
            for (int i = 0; i < 5; i++)
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
            
            // 基础属性
            self.ShowBaseStatItem(1, 1, "生命", numericDic[NumericType.Base_MaxHp_Base].ToString());
            self.ShowBaseStatItem(2, 2, "攻击", ZString.Format("{0}-{1}", numericDic[NumericType.Base_MinAct_Base], numericDic[NumericType.Base_MaxAct_Base]));
            self.ShowBaseStatItem(3, 1, "物防", ZString.Format("{0}-{1}", numericDic[NumericType.Base_MinDef_Base], numericDic[NumericType.Base_MaxDef_Base]));
            self.ShowBaseStatItem(4, 1, "魔防", ZString.Format("{0}-{1}", numericDic[NumericType.Base_MinAdf_Base], numericDic[NumericType.Base_MaxAdf_Base]));
            
            // 技能
            while (self.UISkillItemList.Count < heroConfig.UnlockSkillInfos.Length)
            {
                GameObject go = UnityEngine.Object.Instantiate(self.UISkillItem, self.Content_UISkillItem);
                UISkillItem newItem = self.AddChild<UISkillItem, GameObject>(go);
                self.UISkillItemList.Add(newItem);
            }

            for (int i = 0; i < heroConfig.UnlockSkillInfos.Length; i++)
            {
                self.UISkillItemList[i].UpdateInfo(heroConfig.UnlockSkillInfos[i], 5).Coroutine();
                self.UISkillItemList[i].GameObject.SetActive(true);
            }

            for (int i = heroConfig.UnlockSkillInfos.Length; i < self.UISkillItemList.Count; i++)
            {
                self.UISkillItemList[i].GameObject.SetActive(false);
            }
        }
        
        private static void ShowBaseStatItem(this UIHeroDetailsComponent self, int index, int icon, string name, string value)
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
    }
}