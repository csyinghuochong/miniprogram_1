using Cysharp.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [NumericWatcher(SceneType.Current, NumericType.Now_Hp)]
    public class NumericWatcher_Now_Hp_UpdateUIHeroHp : INumericWatcher
    {
        public void Run(Unit unit, NumbericChange args)
        {
            if (unit.Type != UnitType.Hero)
            {
                return;
            }

            unit.GetComponent<UIHeroHpComponent>()?.UpdateBlood();
        }
    }

    [NumericWatcher(SceneType.Current, NumericType.Now_AngerValue)]
    public class NumericWatcher_Now_AngerValue_UpdateUIHeroHp : INumericWatcher
    {
        public void Run(Unit unit, NumbericChange args)
        {
            if (unit.Type != UnitType.Hero)
            {
                return;
            }

            unit.GetComponent<UIHeroHpComponent>()?.UpdateAnger();
        }
    }

    [EntitySystemOf(typeof(UIHeroHpComponent))]
    [FriendOf(typeof(UIHeroHpComponent))]
    public static partial class UIHeroHpComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UIHeroHpComponent self)
        {
            self.HeadBarPath = ABPathHelper.GetUGUIPath("Blood/UIHeroHp");

            self.Root().GetComponent<GameObjectLoadComponent>().AddLoadQueue(self.HeadBarPath, self.InstanceId, true, self.OnLoadGameObject);
        }

        [EntitySystem]
        private static void Destroy(this UIHeroHpComponent self)
        {
            self.Root().GetComponent<GameObjectLoadComponent>().RecoverGameObject(self.HeadBarPath, self.GameObject);
            self.HeadBarPath = null;
            self.GameObject = null;
            self.Text_Name = null;
            self.Image_Hp = null;
            self.Image_Anger = null;
        }

        private static void OnLoadGameObject(this UIHeroHpComponent self, GameObject gameObject, long formId)
        {
            if (self.IsDisposed)
            {
                UnityEngine.Object.DestroyImmediate(gameObject);
                return;
            }

            self.GameObject = gameObject;
            Unit unit = self.GetParent<Unit>();
            ReferenceCollector rc = self.GameObject.GetComponent<ReferenceCollector>();

            self.Text_Name = rc.Get<GameObject>("Text_Name").GetComponent<TMP_Text>();
            self.Image_Hp = rc.Get<GameObject>("Image_Hp").GetComponent<Image>();
            self.Image_Anger = rc.Get<GameObject>("Image_Anger").GetComponent<Image>();

            GlobalComponent globalComponent = self.Root().GetComponent<GlobalComponent>();
            GameObject bloodparent = globalComponent.BloodPlayer;
            self.GameObject.transform.SetParent(bloodparent.transform);
            self.GameObject.transform.localScale = Vector3.one;

            HeadBarUI headBarUI = self.GameObject.GetComponent<HeadBarUI>();
            headBarUI.enabled = true;
            headBarUI.HeadPos = unit.GetComponent<UnitBoneComponent>().Hp;
            headBarUI.HeadBar = self.GameObject;
            headBarUI.UiCamera = globalComponent.UICamera;
            headBarUI.MainCamera = globalComponent.MainCamera;
            headBarUI.Offset = new Vector2(0, 0);
            headBarUI.UpdatePostion();

            self.UpdateShow();
            self.UpdateBlood();
            self.UpdateAnger();
        }

        public static void UpdateShow(this UIHeroHpComponent self)
        {
            Unit unit = self.GetParent<Unit>();
            HeroConfig heroConfig = HeroConfigCategory.Instance.Get(unit.ConfigId);

            UnitInfoComponent unitInfoComponent = unit.GetComponent<UnitInfoComponent>();
            self.Text_Name.text = unitInfoComponent.UnitName;
        }

        public static void UpdateBlood(this UIHeroHpComponent self)
        {
            if (self.GameObject == null)
            {
                return;
            }

            NumericComponentC numericComponent = self.GetParent<Unit>().GetComponent<NumericComponentC>();
            long currentHp = numericComponent.GetAsLong(NumericType.Now_Hp);
            long maxHp = numericComponent.GetAsLong(NumericType.Now_MaxHp);

            if (maxHp == 0)
            {
                self.Image_Hp.fillAmount = 0;
            }
            else
            {
                self.Image_Hp.fillAmount = currentHp * 1f / maxHp;
            }
        }

        public static void UpdateAnger(this UIHeroHpComponent self)
        {
            if (self.GameObject == null)
            {
                return;
            }

            NumericComponentC numericComponent = self.GetParent<Unit>().GetComponent<NumericComponentC>();
            long currentHp = numericComponent.GetAsLong(NumericType.Now_AngerValue);
            long maxHp = numericComponent.GetAsLong(NumericType.Now_MaxAngerValue);

            if (maxHp == 0)
            {
                self.Image_Anger.fillAmount = 0;
            }
            else
            {
                self.Image_Anger.fillAmount = currentHp * 1f / maxHp;
            }
        }
    }
}