using TMPro;
using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(UINPCHpComponent))]
    [FriendOf(typeof(UINPCHpComponent))]
    public static partial class UINPCHpComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UINPCHpComponent self)
        {
            self.HeadBarPath = ABPathHelper.GetUGUIPath("Blood/UIPlayerHp");

            self.Root().GetComponent<GameObjectLoadComponent>().AddLoadQueue(self.HeadBarPath, self.InstanceId, true, self.OnLoadGameObject);
        }

        [EntitySystem]
        private static void Destroy(this UINPCHpComponent self)
        {
            self.Root().GetComponent<GameObjectLoadComponent>().RecoverGameObject(self.HeadBarPath, self.GameObject);
            self.HeadBarPath = null;
            self.GameObject = null;
            self.Text_Name = null;
        }

        private static void OnLoadGameObject(this UINPCHpComponent self, GameObject gameObject, long formId)
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
            headBarUI.HeadPos = unit.GetComponent<UnitBoneComponent>().Hp;
            headBarUI.HeadBar = self.GameObject;
            headBarUI.UiCamera = globalComponent.UICamera;
            headBarUI.MainCamera = globalComponent.MainCamera;
            headBarUI.Offset = new Vector2(0, 0);
            headBarUI.UpdatePostion();

            NPCConfig npcConfig = NPCConfigCategory.Instance.Get(unit.ConfigId);

            self.Text_Name.SetText(unit.GetComponent<UnitInfoComponent>().UnitName);
        }
    }
}