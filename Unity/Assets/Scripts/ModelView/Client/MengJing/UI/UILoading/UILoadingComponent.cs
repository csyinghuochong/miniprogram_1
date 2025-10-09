using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

namespace ET.Client
{
    [ComponentOf(typeof(UI))]
    public class UILoadingComponent : Entity, IAwake, IUpdate, IDestroy
    {
        public float PassTime;

        public bool StartLoadAssets = false;
        public List<string> PreLoadAssets = new();
        public List<string> ReleaseAssets = new();

        public float Program;

        public Slider Slider_Progress;
        public TMP_Text Text_Progress;
    }
}