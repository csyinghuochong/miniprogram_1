using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ET.Client
{
    public static class UICommonHelper
    {
        public static void AddListener(this Button button, UnityAction clickEventHandler)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(clickEventHandler);
        }

        public static void SetParent(GameObject son, GameObject parent)
        {
            if (son == null || parent == null)
                return;
            son.transform.SetParent(parent.transform);
            son.transform.localPosition = Vector3.zero;
            son.transform.localScale = Vector3.one;
        }

        public static void DestoryChild(GameObject go)
        {
            if (go == null)
                return;

            for (int i = go.transform.childCount - 1; i >= 0; i--)
            {
                UnityEngine.Object.Destroy(go.transform.GetChild(i).gameObject);
            }
        }

        public static void HideChild(GameObject go)
        {
            if (go == null)
                return;

            for (int i = go.transform.childCount - 1; i >= 0; i--)
            {
                go.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}