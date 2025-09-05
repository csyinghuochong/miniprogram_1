using UnityEngine;

namespace ET
{
    public enum UILayer
    {
        BloodRoot = 0,
        NormalRoot = 1,
        MidRoot = 2,
        FixedRoot = 3,
        PopUpRoot = 4,
        OtherRoot = 5
    }
    
    public enum UIEnum
    {
        FullScreen = 0,
        PopupUI = 1,
    }

    public class UILayerScript: MonoBehaviour
    {
        public UILayer UILayer;
        public UIEnum UIType;
        public bool HideMainUI;
        public bool ShowHuoBi;
    }
}