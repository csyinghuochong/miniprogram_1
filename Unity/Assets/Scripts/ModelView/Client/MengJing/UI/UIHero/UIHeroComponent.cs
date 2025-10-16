using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ComponentOf(typeof(UI))]
    public class UIHeroComponent : Entity, IAwake, IDestroy
    {
        public List<UITeamItem> UITeamItemList { get; set; } = new();

        public Transform Content_UITeamItem;
        public GameObject UITeamItem;
        public Button Button_Close;
        public Button Button_Hero;
        public Button Button_Formation;
    }
}