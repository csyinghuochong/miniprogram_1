namespace ET.Server
{
    [EntitySystemOf(typeof(SkillPassiveComponent))]
    [FriendOf(typeof(SkillPassiveComponent))]
    public static partial class SkillPassiveComponentSystem
    {
        [EntitySystem]
        private static void Awake(this SkillPassiveComponent self)
        {

        }
        [EntitySystem]
        private static void Destroy(this SkillPassiveComponent self)
        {

        }
    }
}