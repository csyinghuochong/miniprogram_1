using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ChildOf]
    public class UIRankLianMengItem : Entity, IAwake<GameObject>
    {
        public GameObject GameObject { get; set; }

        public Image Image_LianMengHead;
        public Button Button_OnLianMengHead;
        public TMP_Text Text_LianMengName;
        public TMP_Text Text_LianMengAcive;
        public TMP_Text Text_Sort;
    }
}