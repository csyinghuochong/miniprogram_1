using UnityEngine;

namespace ET.Client
{
    [ChildOf(typeof(FloatingTextComponent))]
    public class FloatingText : Entity, IAwake, IDestroy
    {
        public string Text;
        public float Time;
        public string Path;
        public Transform HeadTransform;

        public GameObject GameObject;
    }
}