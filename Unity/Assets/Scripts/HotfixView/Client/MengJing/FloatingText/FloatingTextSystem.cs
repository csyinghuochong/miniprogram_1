using TMPro;
using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(FloatingText))]
    [FriendOf(typeof(FloatingText))]
    public static partial class FloatingTextSystem
    {
        [EntitySystem]
        private static void Awake(this FloatingText self)
        {
        }

        [EntitySystem]
        private static void Destroy(this FloatingText self)
        {
            self.Root().GetComponent<GameObjectLoadComponent>().RecoverGameObject(self.Path, self.GameObject);
        }

        public static void Update(this FloatingText self)
        {
            self.Time -= Time.deltaTime;

            if (self.GameObject != null && self.HeadTransform != null)
            {
                self.GameObject.transform.localPosition = self.HeadTransform.localPosition + self.Offset;
            }
        }

        public static void Init(this FloatingText self, string text, float time, string path, Vector3 offset, Transform rootTransform, Transform head = null)
        {
            self.Text = text;
            self.Time = time;
            self.Path = path;
            self.Offset = offset;
            self.RootTransform = rootTransform;
            self.HeadTransform = head;
            self.Root().GetComponent<GameObjectLoadComponent>().AddLoadQueue(path, self.InstanceId, true, self.OnLoadGameObject);
        }

        private static void OnLoadGameObject(this FloatingText self, GameObject go, long formId)
        {
            if (self.IsDisposed)
            {
                UnityEngine.Object.Destroy(go);
                return;
            }

            if (self.GameObject != null)
            {
                Log.Error($" self.GameObject !=null:   {self.GameObject.name}    {go.name}   {self.InstanceId}   {formId}");
                return;
            }

            self.GameObject = go;
            self.GameObject.transform.SetParent(self.RootTransform);
            self.GameObject.transform.Find("Text").GetComponent<TMP_Text>().SetText(self.Text);
            self.GameObject.transform.localScale = Vector3.one;
            self.GameObject.transform.localPosition = Vector3.zero;
        }
    }
}