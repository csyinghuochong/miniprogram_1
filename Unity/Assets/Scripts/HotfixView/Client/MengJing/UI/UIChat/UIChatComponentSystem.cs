using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIChatComponent))]
    [FriendOf(typeof(UIChatComponent))]
    public static partial class UIChatComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UIChatComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            
            self.Button_Close = rc.Get<GameObject>("Button_Close").GetComponent<Button>();
            self.Text_Title = rc.Get<GameObject>("Text_Title").GetComponent<TMP_Text>();
            self.Content_UIChatItem = rc.Get<GameObject>("Content_UIChatItem").transform;
            self.UIChatItem = rc.Get<GameObject>("UIChatItem");
            self.InputField_Content = rc.Get<GameObject>("InputField_Content").GetComponent<TMP_InputField>();
            self.Button_Emoji = rc.Get<GameObject>("Button_Emoji").GetComponent<Button>();
            self.Button_Send = rc.Get<GameObject>("Button_Send").GetComponent<Button>();
            self.Button_Type_World = rc.Get<GameObject>("Button_Type_World").GetComponent<Button>();
            self.Button_Type_LianMeng = rc.Get<GameObject>("Button_Type_LianMeng").GetComponent<Button>();

            self.Button_Close.AddListener(() => { self.Root().GetComponent<UIComponent>().Remove(UIType.UIChat); });
            self.Button_Type_World.onClick.AddListener(() => { self.SetShowType(0); });
            self.Button_Type_LianMeng.onClick.AddListener(() => { self.SetShowType(1); });
        }
        
        private static void SetShowType(this UIChatComponent self, int page)
        {
            self.CurrentPage = page;
            self.Button_Type_World.transform.Find("Image_On").gameObject.SetActive(page == 0);
            self.Button_Type_World.transform.Find("Image_Off").gameObject.SetActive(page != 0);
            self.Button_Type_LianMeng.transform.Find("Image_On").gameObject.SetActive(page == 1);
            self.Button_Type_LianMeng.transform.Find("Image_Off").gameObject.SetActive(page != 1);
            
            self.UpdateItemList(page);
        }

        public static void UpdateItemList(this UIChatComponent self, int page)
        {
            
        }
        
    }
}