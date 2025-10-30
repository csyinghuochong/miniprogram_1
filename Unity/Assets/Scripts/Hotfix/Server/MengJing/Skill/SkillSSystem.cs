namespace ET.Server
{
    [EntitySystemOf(typeof(SkillS))]
    [FriendOf(typeof(SkillS))]
    public static partial class SkillSSystem
    {
        [EntitySystem]
        private static void Awake(this SkillS self)
        {
        }

        [EntitySystem]
        private static void Destroy(this SkillS self)
        {
        }
    }
}