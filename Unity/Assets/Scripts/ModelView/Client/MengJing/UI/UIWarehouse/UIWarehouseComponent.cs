using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ComponentOf(typeof(UI))]
    public class UIWarehouseComponent : Entity, IAwake
    {
        public Button Button_Close;
        public GameObject UICommonItem;
        public Transform Content_WarehouseItem;
        public Transform Content_BagItem;
    }
}