using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ChildOf]
    public class UIRankCEItem : Entity, IAwake<GameObject>
    {
        public long UnitId;
        
        public GameObject GameObject { get; set; }

        public Image Image_PlayerHead;
        public Button Button_OnPlayerHead;
        public TMP_Text Text_PlayerName;
        public TMP_Text Text_PlayerCE;
        public TMP_Text Text_Sort;
    }
}