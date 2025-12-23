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
            self.Button_Type_PrivateChat = rc.Get<GameObject>("Button_Type_PrivateChat").GetComponent<Button>();
            self.GameObject_Emoji = rc.Get<GameObject>("GameObject_Emoji");
            self.Button_CloseEmoji = rc.Get<GameObject>("Button_CloseEmoji").GetComponent<Button>();
            self.Content_EmojiList = rc.Get<GameObject>("Content_EmojiList");

            self.GameObject_Emoji.SetActive(false);

            self.Button_Close.AddListener(() => { self.Root().GetComponent<UIComponent>().Remove(UIType.UIChat); });
            self.Button_Emoji.AddListener(() => { self.GameObject_Emoji.SetActive(true); });
            self.Button_Type_World.onClick.AddListener(() => { self.SetShowType(0); });
            self.Button_Type_LianMeng.onClick.AddListener(() => { self.SetShowType(1); });
            self.Button_Type_PrivateChat.AddListener(() => { self.SetShowType(2); });
            self.Button_CloseEmoji.AddListener(() => { self.GameObject_Emoji.SetActive(false); });
        }

        private static void SetShowType(this UIChatComponent self, int page)
        {
            self.CurrentPage = page;
            self.Button_Type_World.transform.Find("Image_On").gameObject.SetActive(page == 0);
            self.Button_Type_World.transform.Find("Image_Off").gameObject.SetActive(page != 0);
            self.Button_Type_LianMeng.transform.Find("Image_On").gameObject.SetActive(page == 1);
            self.Button_Type_LianMeng.transform.Find("Image_Off").gameObject.SetActive(page != 1);
            self.Button_Type_PrivateChat.transform.Find("Image_On").gameObject.SetActive(page == 2);
            self.Button_Type_PrivateChat.transform.Find("Image_Off").gameObject.SetActive(page != 2);

            self.UpdateItemList(page);
        }

        public static void UpdateItemList(this UIChatComponent self, int page)
        {

        }

    }
}