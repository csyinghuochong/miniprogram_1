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

        public static void OnInit(this SkillS self, SkillInfo skillInfo, Unit theUnitFrom)
        {
            self.SkillInfo = skillInfo;
            self.SkillConfig = SkillConfigCategory.Instance.Get(skillInfo.SkillConfigId);
            self.SkillHandler = SkillDispatcherComponentS.Instance.Get(self.SkillConfig.SkillHandler);
            self.SkillState = SkillState.Running;
            self.TheUnitFrom = theUnitFrom;
            if (skillInfo.TargetID != 0)
            {
                self.TheUnitTarget = self.Scene().GetComponent<UnitComponent>().Get(skillInfo.TargetID);
            }

            self.SkillLiveTime = self.SkillConfig.SkillLiveTime * 1f / 1000;
            self.TargetPosition = skillInfo.TargetPosition;
            self.NowPosition = self.TargetPosition;

            self.SkillHandler.OnInit(self);
        }

        public static void OnExecute(this SkillS self)
        {
            self.SkillHandler.OnExecute(self);
        }

        public static void OnUpdate(this SkillS self)
        {
            self.SkillHandler.OnUpdate(self);
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