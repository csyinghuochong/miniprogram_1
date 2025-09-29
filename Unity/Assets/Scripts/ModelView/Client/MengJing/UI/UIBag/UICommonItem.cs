using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ChildOf]
    public class UICommonItem : Entity, IAwake<GameObject>
    {
        public long ItemId;

        public GameObject GameObject { get; set; }

        public Image Image_ItemIcon;
        public TMP_Text Text_ItemName;
        public TMP_Text Text_ItemNum;
        public Button Button_Click;
    }
}