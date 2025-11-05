namespace ET.Server
{
    [EntitySystemOf(typeof(BuffS))]
    [FriendOf(typeof(BuffS))]
    public static partial class BuffSSystem
    {
        [EntitySystem]
        private static void Awake(this BuffS self)
        {
        }

        [EntitySystem]
        private static void Destroy(this BuffS self)
        {
        }

        public static void OnInit(this BuffS self, BuffData buffData, Unit from, Unit to, SkillS skill)
        {
            self.BuffData = buffData;
            self.BuffConfig = BuffConfigCategory.Instance.Get(buffData.BuffConfigId);
        }

        public static void OnUpdate(this BuffS self, float deltaTime)
        {
        }

        public static void OnFinished(this BuffS self)
        {
        }
    }
}