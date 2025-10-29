using TMPro;
using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIPlayerHpComponent))]
    [FriendOf(typeof(UIPlayerHpComponent))]
    public static partial class UIPlayerHpComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UIPlayerHpComponent self)
        {
            self.HeadBarPath = ABPathHelper.GetUGUIPath("Blood/UIPlayerHp");

            self.Root().GetComponent<GameObjectLoadComponent>().AddLoadQueue(self.HeadBarPath, self.InstanceId, true, self.OnLoadGameObject);
        }

        [EntitySystem]
        private static void Destroy(this UIPlayerHpComponent self)
        {
            self.Root().GetComponent<GameObjectLoadComponent>().RecoverGameObject(self.HeadBarPath, self.GameObject);
        }

        private static void OnLoadGameObject(this UIPlayerHpComponent self, GameObject gameObject, long formId)
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

            self.Text_Name.SetText(self.GetParent<Unit>().GetComponent<UnitInfoComponent>().UnitName);
        }
    }
}