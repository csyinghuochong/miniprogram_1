using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ChildOf]
    public class UIRankLianMengItem : Entity, IAwake<GameObject>
    {
        private EntityRef<AllianceRank> allianceRank;
        public AllianceRank AllianceRank { get => this.allianceRank; set => this.allianceRank = value; }

        public GameObject GameObject { get; set; }

        public Image Image_LianMengHead;
        public Button Button_OnLianMengHead;
        public TMP_Text Text_LianMengName;
        public TMP_Text Text_LianMengAcive;
        public TMP_Text Text_Sort;
    }
}