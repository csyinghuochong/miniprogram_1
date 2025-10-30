namespace ET.Server
{
    [EntitySystemOf(typeof(SkillManagerComponentS))]
    [FriendOf(typeof(SkillManagerComponentS))]
    public static partial class SkillManagerComponentSSystem
    {
        [EntitySystem]
        private static void Awake(this SkillManagerComponentS self)
        {
        }

        [EntitySystem]
        private static void Update(this SkillManagerComponentS self)
        {
        }

        [EntitySystem]
        private static void Destroy(this SkillManagerComponentS self)
        {
        }
    }
}