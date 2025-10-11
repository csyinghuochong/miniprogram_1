using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [NumericWatcher(SceneType.Current, NumericType.Now_Hp)]
    public class NumericWatcher_UpdateUIHeroHp : INumericWatcher
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

            self.Image_Hp = rc.Get<GameObject>("Image_Hp").GetComponent<Image>();
            self.Text_Hp = rc.Get<GameObject>("Text_Hp").GetComponent<TMP_Text>();
            self.Text_Name = rc.Get<GameObject>("Text_Name").GetComponent<TMP_Text>();

            GlobalComponent globalComponent = self.Root().GetComponent<GlobalComponent>();
            GameObject bloodparent = globalComponent.BloodPlayer;
            self.GameObject.transform.SetParent(bloodparent.transform);
            self.GameObject.transform.localScale = Vector3.one;

            HeadBarUI headBarUI = self.GameObject.GetComponent<HeadBarUI>();
            headBarUI.enabled = true;
            headBarUI.HeadPos = unit.GetComponent<GameObjectComponent>().GameObject.transform;
            headBarUI.HeadBar = self.GameObject;
            headBarUI.UiCamera = globalComponent.UICamera.GetComponent<Camera>();
            headBarUI.MainCamera = globalComponent.MainCamera.GetComponent<Camera>();
            headBarUI.Offset = new Vector2(0, 3f);
            headBarUI.UpdatePostion();

            self.UpdateShow();
            self.UpdateBlood();
        }

        public static void UpdateShow(this UIHeroHpComponent self)
        {
            Unit unit = self.GetParent<Unit>();
            HeroConfig heroConfig = HeroConfigCategory.Instance.Get(unit.ConfigId);
            self.Text_Name.SetText(heroConfig.HeroName);
        }

        public static void UpdateBlood(this UIHeroHpComponent self)
        {
            NumericComponentC numericComponent = self.GetParent<Unit>().GetComponent<NumericComponentC>();
            long currentHp = numericComponent.GetAsLong(NumericType.Now_Hp);
            long maxHp = numericComponent.GetAsLong(NumericType.Now_MaxHp);
            float blood = currentHp * 1f / maxHp;
            blood = Mathf.Max(blood, 0f);

            self.Image_Hp.fillAmount = blood;
            self.Text_Hp.SetText("{0}/{1}", currentHp, maxHp);
        }
    }
}