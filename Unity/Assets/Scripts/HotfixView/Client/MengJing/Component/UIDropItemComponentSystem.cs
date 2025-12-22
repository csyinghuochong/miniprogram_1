using TMPro;
using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIDropItemComponent))]
    [FriendOf(typeof(UIDropItemComponent))]
    public static partial class UIDropItemComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UIDropItemComponent self)
        {
            self.HeadBarPath = ABPathHelper.GetUGUIPath("Blood/UIDropItem");

            self.Root().GetComponent<GameObjectLoadComponent>().AddLoadQueue(self.HeadBarPath, self.InstanceId, true, self.OnLoadGameObject);
        }

        [EntitySystem]
        private static void Destroy(this UIDropItemComponent self)
        {
            self.Root().GetComponent<GameObjectLoadComponent>().RecoverGameObject(self.HeadBarPath, self.GameObject);
            self.HeadBarPath = null;
            self.GameObject = null;
            self.Text_Name = null;
        }

        private static void OnLoadGameObject(this UIDropItemComponent self, GameObject gameObject, long formId)
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
            GameObject bloodparent = globalComponent.BloodMonster;
            self.GameObject.transform.SetParent(bloodparent.transform);
            self.GameObject.transform.localScale = Vector3.one;

            HeadBarUI headBarUI = self.GameObject.GetComponent<HeadBarUI>();
            headBarUI.enabled = true;
            headBarUI.HeadPos = unit.GetComponent<GameObjectComponent>().GameObject.transform.Find("UIPosition");
            headBarUI.HeadBar = self.GameObject;
            headBarUI.UiCamera = globalComponent.UICamera;
            headBarUI.MainCamera = globalComponent.MainCamera;
            headBarUI.Offset = new Vector2(0, 0);
            headBarUI.UpdatePostion();

            self.UpdateShow().Coroutine();
        }

        public static async ETTask UpdateShow(this UIDropItemComponent self)
        {
            Unit unit = self.GetParent<Unit>();
            NumericComponentC numericComponent = unit.GetComponent<NumericComponentC>();

            ItemConfig itemConfig = ItemConfigCategory.Instance.Get(numericComponent.GetAsInt(NumericType.DropItemId));
            self.Text_Name.text = itemConfig.ItemName;

            string path = ABPathHelper.GetAtlasPath_2(ABAtlasTypes.ItemIcon, itemConfig.Icon);
            unit.GetComponent<GameObjectComponent>().GameObject.transform.Find("DropModel").GetComponent<SpriteRenderer>().sprite =
                    await self.Root().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<Sprite>(path);
        }
    }
}