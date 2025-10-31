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
            self.OnFinished();
        }

        public static void OnInit(this SkillS self, UseSkillInfo useSkillInfo, Unit theUnitFrom)
        {
            self.UseSkillInfo = useSkillInfo;
            self.SkillConfig = SkillConfigCategory.Instance.Get(useSkillInfo.SkillConfigId);
            self.SkillHandler = SkillDispatcherComponentS.Instance.Get(self.SkillConfig.SkillHandler);
            self.SkillState = SkillState.Running;
            self.TheUnitFrom = theUnitFrom;
            if (useSkillInfo.TargetId != 0)
            {
                self.TheUnitTarget = self.Scene().GetComponent<UnitComponent>().Get(useSkillInfo.TargetId);
            }

            self.SkillLiveTime = self.SkillConfig.SkillLiveTime * 1f / 1000;
            self.TargetPosition = useSkillInfo.Position;
            self.NowPosition = self.TargetPosition;

            self.SkillHandler.OnInit(self);
        }

        public static void OnExecute(this SkillS self)
        {
            self.SkillHandler.OnExecute(self);
        }

        public static void OnUpdate(this SkillS self, float deltaTime)
        {
            self.SkillHandler.OnUpdate(self, deltaTime);
        }

        public static void OnFinished(this SkillS self)
        {
            self.SkillHandler.OnFinished(self);
        }

        public static void InitSelfBuff(this SkillS self)
        {
        }
    }
}