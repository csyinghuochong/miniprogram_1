using System;
using Cysharp.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIMainSkillItem))]
    [FriendOf(typeof(UIMainSkillItem))]
    public static partial class UIMainSkillItemSystem
    {
        [Invoke(TimerInvokeType.UIMainSkillItemTimer)]
        public class UIMainSkillItemTimer : ATimer<UIMainSkillItem>
        {
            protected override void Run(UIMainSkillItem self)
            {
                try
                {
                    self.UpdateIndicator();
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }

        [EntitySystem]
        private static void Awake(this UIMainSkillItem self, GameObject gameObject)
        {
            self.GameObject = gameObject;

            ReferenceCollector rc = gameObject.GetComponent<ReferenceCollector>();
            self.Image_SkillIcon = rc.Get<GameObject>("Image_SkillIcon").GetComponent<Image>();
            self.Image_SkillCd = rc.Get<GameObject>("Image_SkillCd").GetComponent<Image>();
            self.Text_SkillCd = rc.Get<GameObject>("Text_SkillCd").GetComponent<TMP_Text>();
            self.EventTrigger_Click = rc.Get<GameObject>("EventTrigger_Click").GetComponent<EventTrigger>();

            self.EventTrigger_Click.AddEventTrigger(self.OnPointerDown, EventTriggerType.PointerDown);
            self.EventTrigger_Click.AddEventTrigger(self.OnBeginDrag, EventTriggerType.BeginDrag);
            self.EventTrigger_Click.AddEventTrigger(self.OnDrag, EventTriggerType.Drag);
            self.EventTrigger_Click.AddEventTrigger(self.OnPointerUp, EventTriggerType.PointerUp);
            self.EventTrigger_Click.AddEventTrigger(self.OnEndDrag, EventTriggerType.EndDrag);
        }

        [EntitySystem]
        private static void Destroy(this UIMainSkillItem self)
        {
            self.Root().GetComponent<TimerComponent>().Remove(ref self.Timer);
        }

        public static void UpdateCD(this UIMainSkillItem self)
        {
            if (!self.GameObject.activeSelf)
            {
                return;
            }

            Unit unit = self.Root().CurrentScene().GetComponent<UnitComponent>().Get(self.UnitId);
            if (unit == null)
            {
                return;
            }

            SkillConfig skillConfig = SkillConfigCategory.Instance.Get(self.SkillId);

            SkillManagerComponentC skillManagerComponent = unit.GetComponent<SkillManagerComponentC>();
            float cd = skillManagerComponent.GetSkillCD(self.SkillId);

            if (cd <= 0)
            {
                self.Image_SkillCd.gameObject.SetActive(false);
                self.Text_SkillCd.SetText("");
            }
            else
            {
                self.Image_SkillCd.gameObject.SetActive(true);
                self.Image_SkillCd.fillAmount = cd / skillConfig.SkillCD;
                self.Text_SkillCd.SetTextFormat("{0:0.#}", cd);
            }
        }

        public static async ETTask UpdateInfo(this UIMainSkillItem self, long unitId, int skillId)
        {
            self.UnitId = unitId;
            self.SkillId = skillId;

            SkillConfig skillConfig = SkillConfigCategory.Instance.Get(skillId);

            string path = ABPathHelper.GetAtlasPath_2(ABAtlasTypes.SkillIcon, skillConfig.SkillIcon);
            self.Image_SkillIcon.overrideSprite = await self.Root().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<Sprite>(path);
        }

        private static void OnPointerDown(this UIMainSkillItem self, PointerEventData pdata)
        {
            self.IsDrag = false;

            if (self.GetParent<UIMainSkillComponent>().AutoFight)
            {
                self.Root().GetComponent<FloatingTextComponent>().ShowTipText("取消自动模式后可手动控制技能释放！！！");
                return;
            }

            self.IsDrag = true;

            if (self.Timer == 0)
            {
                self.Timer = self.Root().GetComponent<TimerComponent>().NewFrameTimer(TimerInvokeType.UIMainSkillItemTimer, self);
            }

            SkillConfig skillConfig = SkillConfigCategory.Instance.Get(self.SkillId);

            if (skillConfig.SkillTargetType == SkillTargetType.SelfPosition)
            {
                self.AssetsPath = ABPathHelper.GetSkillIndicatorPath("Skill_CommonAttack");
            }
            else
            {
                self.AssetsPath = ABPathHelper.GetSkillIndicatorPath("Skill_CommonAttack");
            }

            if (!string.IsNullOrEmpty(self.AssetsPath) && self.IndicatorGameObject == null)
            {
                self.Root().GetComponent<GameObjectLoadComponent>().AddLoadQueue(self.AssetsPath, self.InstanceId, true, self.OnLoadGameObject);
            }
        }

        private static void OnBeginDrag(this UIMainSkillItem self, PointerEventData pdata)
        {
        }

        private static void OnDrag(this UIMainSkillItem self, PointerEventData pdata)
        {
            if (self.IsDrag == false)
            {
                return;
            }

            Unit myUnit = self.Root().CurrentScene().GetComponent<UnitComponent>().Get(self.UnitId);
            if (myUnit == null)
            {
                return;
            }

            SkillConfig skillConfig = SkillConfigCategory.Instance.Get(self.SkillId);

            if (skillConfig.SkillTargetType == SkillTargetType.SelfPosition)
            {
            }
            else if (skillConfig.SkillTargetType == SkillTargetType.TargetPositon)
            {
            }
            else
            {
            }
        }

        private static void OnPointerUp(this UIMainSkillItem self, PointerEventData pdata)
        {
            self.IsDrag = false;

            self.Root().GetComponent<TimerComponent>().Remove(ref self.Timer);

            if (self.GetParent<UIMainSkillComponent>().AutoFight)
            {
                return;
            }

            if (!string.IsNullOrEmpty(self.AssetsPath) && self.IndicatorGameObject != null)
            {
                self.Root().GetComponent<GameObjectLoadComponent>().RecoverGameObject(self.AssetsPath, self.IndicatorGameObject);
                self.IndicatorGameObject = null;
            }
        }

        private static void OnEndDrag(this UIMainSkillItem self, PointerEventData pdata)
        {
        }

        private static void OnLoadGameObject(this UIMainSkillItem self, GameObject go, long formId)
        {
            if (self.IsDisposed)
            {
                UnityEngine.Object.Destroy(go);
                return;
            }

            if (self.IndicatorGameObject != null)
            {
                return;
            }

            self.IndicatorGameObject = go;
            self.IndicatorGameObject.transform.SetParent(self.Root().GetComponent<GlobalComponent>().Unit);

            SkillConfig skillConfig = SkillConfigCategory.Instance.Get(self.SkillId);

            if (skillConfig.SkillTargetType == SkillTargetType.SelfPosition)
            {
                self.IndicatorGameObject.transform.Find("Skill_Area").localScale = Vector3.one * skillConfig.DamageRange[0] * 2;
            }
            else
            {
            }
        }

        private static void UpdateIndicator(this UIMainSkillItem self)
        {
            if (self.IndicatorGameObject == null)
            {
                return;
            }

            Unit myUnit = self.Root().CurrentScene().GetComponent<UnitComponent>().Get(self.UnitId);
            if (myUnit == null)
            {
                return;
            }
            
            self.IndicatorGameObject.transform.position = myUnit.Position;
        }
    }
}