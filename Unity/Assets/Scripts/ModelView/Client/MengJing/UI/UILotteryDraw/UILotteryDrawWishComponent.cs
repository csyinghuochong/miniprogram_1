using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ComponentOf]
    public class UILotteryDrawWishComponent : Entity, IAwake<GameObject>, IDestroy
    {
        public GameObject GameObject { get; set; }
        public long ItemId;
        
        public List<UICommonItem> UICommonItemList { get; set; } = new();
        
        public Transform Content_UICommonItem;
        public GameObject UICommonItem;
        public Button Button_Close;
        
    }
}