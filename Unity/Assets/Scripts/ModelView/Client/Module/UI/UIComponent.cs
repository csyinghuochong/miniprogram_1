using System.Collections.Generic;
using UnityEngine;

namespace ET.Client
{
    /// <summary>
    /// 管理Scene上的UI
    /// </summary>
    [ComponentOf(typeof(Scene))]
    public class UIComponent : Entity, IAwake, IDestroy
    {
        public Camera UICamera { get; set; }
        public Camera MainCamera { get; set; }

        public int ResolutionWidth { get; set; }
        public int ResolutionHeight { get; set; }
        public Dictionary<string, UI> UIs { get; set; } = new Dictionary<string, UI>();
    }
}