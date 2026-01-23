using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ComponentOf(typeof(UI))]
    public class UIBattlePassComponent : Entity, IAwake, IDestroy
    {
        
        public List<UIBattlePassItem> UIBattlePassItemList { get; set; } = new();

        public Button Button_Close;
        public Transform Content_UIBattlePassItem;
        public GameObject UIBattlePassItem;
        public Button Button_GetAllReward;
    }
}