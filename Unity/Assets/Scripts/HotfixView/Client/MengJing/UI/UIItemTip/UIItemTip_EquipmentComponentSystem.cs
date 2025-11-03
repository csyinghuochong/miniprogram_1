using System.Collections.Generic;
using Cysharp.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIItemTip_EquipmentComponent))]
    [FriendOf(typeof(UIItemTip_EquipmentComponent))]
    public static partial class UIItemTip_EquipmentComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UIItemTip_EquipmentComponent self, GameObject gameObject)
        {
            self.GameObject = gameObject;

            ReferenceCollector rc = gameObject.GetComponent<ReferenceCollector>();

            self.Text_ItemName = rc.Get<GameObject>("Text_ItemName").GetComponent<TMP_Text>();
            self.Text_ItemEquipmentType = rc.Get<GameObject>("Text_ItemEquipmentType").GetComponent<TMP_Text>();
            self.Text_Lv = rc.Get<GameObject>("Text_Lv").GetComponent<TMP_Text>();
            self.Image_CombatPowerChange = rc.Get<GameObject>("Image_CombatPowerChange").GetComponent<Image>();
            self.Image_CombatPowerChange.gameObject.SetActive(false);
            self.Text_CombatPowerChange = rc.Get<GameObject>("Text_CombatPowerChange").GetComponent<TMP_Text>();
            self.Image_CombatPowerReduction = rc.Get<GameObject>("Image_CombatPowerReduction").GetComponent<Image>();
            self.Image_CombatPowerIncrease = rc.Get<GameObject>("Image_CombatPowerIncrease").GetComponent<Image>();
            self.BaseAttributeList = rc.Get<GameObject>("BaseAttributeList").transform;
            self.UIAttributeItem = rc.Get<GameObject>("UIAttributeItem");
            self.UIAttributeItem.SetActive(false);
            self.Image_ItemQuality = rc.Get<GameObject>("Image_ItemQuality").GetComponent<Image>();
            self.Image_ItemIcon = rc.Get<GameObject>("Image_ItemIcon").GetComponent<Image>();
            self.Button_Sell = rc.Get<GameObject>("Button_Sell").GetComponent<Button>();
            self.Button_Wear = rc.Get<GameObject>("Button_Wear").GetComponent<Button>();
            self.Button_TakeOff = rc.Get<GameObject>("Button_TakeOff").GetComponent<Button>();
            self.Text_EquipHero = rc.Get<GameObject>("Text_EquipHero").GetComponent<TMP_Text>();

            self.Button_Sell.gameObject.SetActive(false);
            self.Button_Wear.gameObject.SetActive(false);
            self.Button_TakeOff.gameObject.SetActive(false);
            self.Text_EquipHero.gameObject.SetActive(false);

            self.Button_Sell.AddListener(() => { self.OnButton_Sell().Coroutine(); });
            self.Button_Wear.AddListener(self.OnButton_Wear);
            self.Button_TakeOff.AddListener(self.OnButton_TakeOff);
        }

        [EntitySystem]
        private static void Destroy(this UIItemTip_EquipmentComponent self)
        {
        }

        public static async ETTask UpdateInfo(this UIItemTip_EquipmentComponent self, UIItemTipData uiItemTipData)
        {
            self.UIItemTipData = uiItemTipData;

            InventoryComponentC inventoryComponent = self.Root().GetComponent<InventoryComponentC>();
            Item newItem = inventoryComponent.GetItem(uiItemTipData.ItemId);
            ItemConfig newItemConfig = ItemConfigCategory.Instance.Get(newItem.ConfigId);
            EquipConfig newEquipConfig = EquipConfigCategory.Instance.Get(newItemConfig.ItemEquipID);

            string type = newItemConfig.ItemSubType switch
            {
                (int)ItemEquipmentType.Toukui => "头盔",
                (int)ItemEquipmentType.Yifu => "衣服",
                (int)ItemEquipmentType.Kuzi => "裤子",
                (int)ItemEquipmentType.Xiezi => "鞋子",
                (int)ItemEquipmentType.Xianglian => "项链",
                (int)ItemEquipmentType.Wuqi => "武器",
                _ => ""
            };

            string color = newItemConfig.ItemQuality switch
            {
                1 => "#0e832a",
                2 => "#2e69c4",
                3 => "#d6bb10",
                4 => "#be5e10",
                5 => "#e200af",
                6 => "#d01a06",
            };

            Color nowColor;
            ColorUtility.TryParseHtmlString(color, out nowColor);

            self.Text_ItemName.SetText(newItemConfig.ItemName);
            self.Text_ItemName.color = nowColor;
            self.Text_ItemEquipmentType.SetText(type);
            self.Text_Lv.SetTextFormat("{0}级", newItemConfig.UseLv);

            string qualityPath = ABPathHelper.GetAtlasPath_2(ABAtlasTypes.ItemQualityIcon, ZString.Format("quality{0}", newItemConfig.ItemQuality));
            self.Image_ItemQuality.overrideSprite = await self.Root().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<Sprite>(qualityPath);

            string path = ABPathHelper.GetAtlasPath_2(ABAtlasTypes.ItemIcon, newItemConfig.Icon);
            self.Image_ItemIcon.overrideSprite = await self.Root().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<Sprite>(path);

            long actChange = 0;
            long defChange = 0;

            if (uiItemTipData.UIItemTipOpType == UIItemTipOpType.UIHero_Wear)
            {
                self.Button_Wear.gameObject.SetActive(true);

                Hero hero = self.Root().GetComponent<HeroComponentC>().GetHero(self.UIItemTipData.HeroId);
                EquipSlotType equipSlotType = CommonHelp.GetCanEquipSlot(hero.Equipments, (ItemEquipmentType)newItemConfig.ItemSubType);

                if (equipSlotType == EquipSlotType.None)
                {
                    self.Button_Wear.gameObject.SetActive(false);
                }
                else
                {
                    self.Image_CombatPowerChange.gameObject.SetActive(true);

                    List<Item> equipments = new List<Item>();

                    foreach (KeyValuePair<int, long> heroEquipment in hero.Equipments)
                    {
                        if (heroEquipment.Value != 0)
                        {
                            Item oldItem = inventoryComponent.GetItem(heroEquipment.Value);
                            if (oldItem != null && heroEquipment.Value != hero.Equipments[(int)equipSlotType])
                            {
                                equipments.Add(oldItem);
                            }
                        }
                    }

                    equipments.Add(newItem);

                    Dictionary<int, long> oldNumericDic = hero.NumericDic;
                    Dictionary<int, long> newNumericDic = CommonHelp.CalculateHeroNumeric(hero, equipments);

                    long combatPowerChange = newNumericDic[NumericType.CombatPower] - oldNumericDic[NumericType.CombatPower];
                    self.Text_CombatPowerChange.SetText(combatPowerChange);
                    self.Image_CombatPowerReduction.gameObject.SetActive(combatPowerChange < 0);
                    self.Image_CombatPowerIncrease.gameObject.SetActive(combatPowerChange > 0);

                    actChange = newNumericDic[NumericType.Base_MaxAct_Base] - oldNumericDic[NumericType.Base_MaxAct_Base];
                    defChange = newNumericDic[NumericType.Base_MaxDef_Base] - oldNumericDic[NumericType.Base_MaxDef_Base];
                }
            }

            if (uiItemTipData.UIItemTipOpType == UIItemTipOpType.UIHero_TakeOff)
            {
                self.Button_TakeOff.gameObject.SetActive(true);
                Hero hero = self.Root().GetComponent<HeroComponentC>().GetHero(self.UIItemTipData.HeroId);
                HeroConfig heroConfig = HeroConfigCategory.Instance.Get(hero.ConfigId);
                self.Text_EquipHero.gameObject.SetActive(true);
                self.Text_EquipHero.SetText(heroConfig.HeroName);
            }

            if (uiItemTipData.UIItemTipOpType == 0)
            {
                self.Button_Sell.gameObject.SetActive(true);
            }

            self.ShowBaseAttributeItem(1, "攻击", newEquipConfig.EquipMaxAct.ToString(), actChange);
            self.ShowBaseAttributeItem(2, "防御", newEquipConfig.EquipMaxDef.ToString(), defChange);
        }

        private static void ShowBaseAttributeItem(this UIItemTip_EquipmentComponent self, int index, string name, string value, long change)
        {
            GameObject baseAttributeItem = UnityEngine.Object.Instantiate(self.UIAttributeItem, self.BaseAttributeList);
            ReferenceCollector rc = baseAttributeItem.GetComponent<ReferenceCollector>();
            if (index % 2 == 0)
            {
                rc.Get<GameObject>("Image_IconDef").GetComponent<Image>().gameObject.SetActive(false);

            }
            else
            {
                rc.Get<GameObject>("Image_IconAtk").GetComponent<Image>().gameObject.SetActive(false);
                rc.Get<GameObject>("Image_BG").GetComponent<Image>().gameObject.SetActive(false);
            }

            rc.Get<GameObject>("Text_Name").GetComponent<TMP_Text>().SetText(name);
            rc.Get<GameObject>("Text_Value").GetComponent<TMP_Text>().SetText(value);
            rc.Get<GameObject>("Image_Reduction").SetActive(change < 0);
            rc.Get<GameObject>("Image_Increase").SetActive(change > 0);
            baseAttributeItem.SetActive(true);
        }

        private static async ETTask OnButton_Sell(this UIItemTip_EquipmentComponent self)
        {
            UI ui = await self.Root().GetComponent<UIComponent>().Create(UIType.UIItemSellTip);
            UIItemSellTipComponent uiItemSellTipComponent = ui.GetComponent<UIItemSellTipComponent>();
            uiItemSellTipComponent.InitUI(self.UIItemTipData);

            self.OnClose();
        }

        private static void OnButton_Wear(this UIItemTip_EquipmentComponent self)
        {
            ClientHeroHelper.SetHeroEquipment(self.Root(), 0, self.UIItemTipData.HeroId, self.UIItemTipData.ItemId).Coroutine();
            self.OnClose();
        }

        private static void OnButton_TakeOff(this UIItemTip_EquipmentComponent self)
        {
            ClientHeroHelper.SetHeroEquipment(self.Root(), 1, self.UIItemTipData.HeroId, self.UIItemTipData.ItemId).Coroutine();
            self.OnClose();
        }

        private static void OnClose(this UIItemTip_EquipmentComponent self)
        {
            self.Root().GetComponent<UIComponent>().Remove(UIType.UIItemTip);
        }
    }
}