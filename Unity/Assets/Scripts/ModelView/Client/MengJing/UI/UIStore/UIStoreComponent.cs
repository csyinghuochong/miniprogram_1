using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ComponentOf(typeof(UI))]
    public class UIStoreComponent : Entity, IAwake, IDestroy
    {
        public long RefreshTime;
        public Dictionary<int, int> StoreItemList;
        public int StoreRefreshNum;

        public List<UIStoreItem> UIStoreItemList { get; set; } = new();
        
        public Button Button_Close;
        public TMP_Text Text_RefreshTime;
        public Button Button_RefreshTime;
        public Transform Content_UIStoreITem;
        public GameObject UIStoreItem;
    }
}