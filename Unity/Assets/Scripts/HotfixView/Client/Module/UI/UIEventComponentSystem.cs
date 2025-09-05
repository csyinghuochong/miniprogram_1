using System;
using System.Collections.Generic;
using UnityEngine;

namespace ET.Client
{
    /// <summary>
    /// 管理所有UI GameObject 以及UI事件
    /// </summary>
    [EntitySystemOf(typeof(UIEventComponent))]
    [FriendOf(typeof(UIEventComponent))]
    public static partial class UIEventComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UIEventComponent self)
        {
            UIEventComponent.Instance = self;

            GameObject uiRoot = GameObject.Find("/Global/UI");

            self.UILayers.Add((int)UILayer.BloodRoot, GameObject.Find("/Global/UI/BloodRoot").transform);
            self.UILayers.Add((int)UILayer.NormalRoot, GameObject.Find("/Global/UI/NormalRoot").transform);
            self.UILayers.Add((int)UILayer.MidRoot, GameObject.Find("/Global/UI/MidRoot").transform);
            self.UILayers.Add((int)UILayer.FixedRoot, GameObject.Find("/Global/UI/FixedRoot").transform);
            self.UILayers.Add((int)UILayer.PopUpRoot, GameObject.Find("/Global/UI/PopUpRoot").transform);
            self.UILayers.Add((int)UILayer.OtherRoot, GameObject.Find("/Global/UI/OtherRoot").transform);

            var uiEvents = CodeTypes.Instance.GetTypes(typeof(UIEventAttribute));
            foreach (Type type in uiEvents)
            {
                object[] attrs = type.GetCustomAttributes(typeof(UIEventAttribute), false);
                if (attrs.Length == 0)
                {
                    continue;
                }

                UIEventAttribute uiEventAttribute = attrs[0] as UIEventAttribute;
                AUIEvent aUIEvent = Activator.CreateInstance(type) as AUIEvent;
                self.UIEvents.Add(uiEventAttribute.UIType, aUIEvent);
            }
        }

        [EntitySystem]
        private static void Destroy(this UIEventComponent self)
        {
            UIEventComponent.Instance = null;
            
            self.UILayers = new Dictionary<int, Transform>();
            self.UIEvents = new Dictionary<string, AUIEvent>();
        }

        public static async ETTask<UI> OnCreate(this UIEventComponent self, UIComponent uiComponent, string uiType)
        {
            try
            {
                long instanceId = self.InstanceId;
                if (!self.UIEvents.ContainsKey(uiType))
                {
                    return null;
                }

                UI ui = await self.UIEvents[uiType].OnCreate(self.Scene(), uiComponent);
                if (instanceId != self.InstanceId)
                {
                    return null;
                }

                UILayer uiLayer = ui.GameObject.GetComponent<UILayerScript>().UILayer;
                ui.GameObject.transform.SetParent(self.UILayers[(int)uiLayer]);
                ui.GameObject.transform.localPosition = Vector3.zero;
                ui.GameObject.transform.localScale = Vector3.one;

                if (ui.GameObject.GetComponent<UILayerScript>().UIType == ET.UIEnum.FullScreen)
                {
                    ui.GameObject.transform.GetComponent<RectTransform>().offsetMax = Vector2.zero;
                    ui.GameObject.transform.GetComponent<RectTransform>().offsetMin = Vector2.zero;
                }

                return ui;
            }
            catch (Exception e)
            {
                throw new Exception($"on create ui error: {uiType}", e);
            }
        }

        public static void OnRemove(this UIEventComponent self, UIComponent uiComponent, string uiType)
        {
            try
            {
                self.UIEvents[uiType].OnRemove(self.Scene(), uiComponent);
            }
            catch (Exception e)
            {
                throw new Exception($"on remove ui error: {uiType}", e);
            }
        }
    }
}