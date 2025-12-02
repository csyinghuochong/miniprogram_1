using UnityEngine;

namespace ET.Client
{
    [ChildOf(typeof(FloatingTextComponent))]
    public class FloatingText : Entity, IAwake, IDestroy
    {
        public string Text;
        public float Time;
        public string Path;
        public Vector3 Offset;
        public Transform HeadTransform;

        public GameObject GameObject;
    }
}