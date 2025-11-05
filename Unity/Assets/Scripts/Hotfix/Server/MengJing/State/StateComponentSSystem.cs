namespace ET.Server
{
    [EntitySystemOf(typeof(StateComponentS))]
    [FriendOf(typeof(StateComponentS))]
    public static partial class StateComponentSSystem
    {
        [EntitySystem]
        private static void Awake(this StateComponentS self)
        {
            self.CurrentStateType = StateTypeEnum.None;
            self.RigidityEndTime = 0;
        }

        [EntitySystem]
        private static void Deserialize(this StateComponentS self)
        {
            self.CurrentStateType = StateTypeEnum.None;
            self.RigidityEndTime = 0;
        }

        public static void Reset(this StateComponentS self)
        {
            self.CurrentStateType = StateTypeEnum.None;
        }

        public static void StateTypeAdd(this StateComponentS self, long nowStateType)
        {
            Unit unit = self.GetParent<Unit>();
            self.CurrentStateType = self.CurrentStateType | nowStateType;

            EventSystem.Instance.Publish(self.Scene(), new StateTypeAdd() { UnitDefend = unit, nowStateType = nowStateType });
        }

        public static void StateTypeRemove(this StateComponentS self, long nowStateType)
        {
            self.CurrentStateType = self.CurrentStateType & ~nowStateType;

            Unit unit = self.GetParent<Unit>();

            EventSystem.Instance.Publish(self.Scene(), new StateTypeRemove() { UnitDefend = unit, nowStateType = nowStateType });
        }

        public static bool StateTypeGet(this StateComponentS self, long nowStateType)
        {
            long state = self.CurrentStateType & nowStateType;

            if (state > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsStateBroadcastType(this StateComponentS self, long nowStateType)
        {
            return nowStateType == StateTypeEnum.Singing
                    || nowStateType == StateTypeEnum.OpenBox
                    || nowStateType == StateTypeEnum.Stealth
                    || nowStateType == StateTypeEnum.Hide
                    || nowStateType == StateTypeEnum.BaTi;
        }
    }
}