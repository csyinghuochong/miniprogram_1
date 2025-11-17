using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIMainSkillComponent))]
    [FriendOf(typeof(UIMainSkillComponent))]
    public static partial class UIMainSkillComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UIMainSkillComponent self, GameObject gameObject)
        {
            self.GameObject = gameObject;
        }

        [EntitySystem]
        private static void Destroy(this UIMainSkillComponent self)
        {
        }
    }
}