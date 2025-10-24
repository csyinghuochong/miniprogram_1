using System.Collections.Generic;
using Spine.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ComponentOf(typeof(UI))]
    public class UIHeroComponent : Entity, IAwake, IDestroy
    {
        public UIHeroInfoComponent UIHeroInfoComponent { get; set; }

        public Button Button_Close;
        public Transform Transform_PanelRoot;
        public TMP_Text Text_Type_Gold;
        public TMP_Text Text_Type_Diamond;
        public Button Button_Hero;
        public Button Button_HeroList;
        public Button Button_Formation;
    }
}