using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIMainSkillItem))]
    [FriendOf(typeof(UIMainSkillItem))]
    public static partial class UIMainSkillItemSystem
    {
        [EntitySystem]
        private static void Awake(this UIMainSkillItem self, GameObject gameObject)
        {
            self.GameObject = gameObject;
        }

        [EntitySystem]
        private static void Destroy(this UIMainSkillItem self)
        {
        }
    }
}