using System.Collections.Generic;
using UnityEngine;

namespace ET.Client
{
	
	/// <summary>
	/// 管理Scene上的UI
	/// </summary>
	[EntitySystemOf(typeof(UIComponent))]
	public static partial class UIComponentSystem
	{
		
		[EntitySystem]
		private static void Awake(this UIComponent self)
		{
			self.UICamera = GameObject.Find("Global/UICamera").GetComponent<Camera>();
			self.MainCamera = GameObject.Find("Global/MainCamera").GetComponent<Camera>();
			self.ResolutionWidth = (int)(Screen.currentResolution.width);
			self.ResolutionHeight = (int)(Screen.currentResolution.height);
		}

		[EntitySystem]
		private static void Destroy(this UIComponent self)
		{
			foreach (var field in self.UIs.Values)
			{
				field.Dispose();
			}
			self.UIs = new Dictionary<string, UI>();
		}
		
		public static async ETTask<UI> Create(this UIComponent self, string uiType)
		{
			UI ui;
			self.UIs.TryGetValue(uiType, out ui);
			if (ui != null)
				return ui;
			ui = await UIEventComponent.Instance.OnCreate(self, uiType);
			self.UIs.Add(uiType, ui);
			return ui;
		}

		public static void Remove(this UIComponent self, string uiType)
		{
			if (!self.UIs.TryGetValue(uiType, out UI ui))
			{
				return;
			}

			UIEventComponent.Instance.OnRemove(self, uiType);
			self.UIs.Remove(uiType);
			ui.Dispose();
		}

		public static UI Get(this UIComponent self, string name)
		{
			UI ui = null;
			self.UIs.TryGetValue(name, out ui);
			return ui;
		}
	}
}