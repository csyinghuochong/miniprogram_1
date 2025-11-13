namespace ET.Server
{
    [EntitySystemOf(typeof(StateComponentS))]
    [FriendOf(typeof(StateComponentS))]
    public static partial class StateComponentSSystem
    {
        [EntitySystem]
        private static void Awake(this StateComponentS self)
        {
            self.CurrentStateType = StateType.None;
            self.RigidityEndTime = 0;
        }

        [EntitySystem]
        private static void Deserialize(this StateComponentS self)
        {
            self.CurrentStateType = StateType.None;
            self.RigidityEndTime = 0;
        }

        public static void Reset(this StateComponentS self)
        {
            self.CurrentStateType = StateType.None;
        }

        public static void StateTypeAdd(this StateComponentS self, StateType nowStateType)
        {
            Unit unit = self.GetParent<Unit>();
            self.CurrentStateType = self.CurrentStateType | nowStateType;

            EventSystem.Instance.Publish(self.Scene(), new StateTypeAdd() { UnitDefend = unit, nowStateType = nowStateType });
        }

        public static void StateTypeRemove(this StateComponentS self, StateType nowStateType)
        {
            self.CurrentStateType = self.CurrentStateType & ~nowStateType;

            Unit unit = self.GetParent<Unit>();

            EventSystem.Instance.Publish(self.Scene(), new StateTypeRemove() { UnitDefend = unit, nowStateType = nowStateType });
        }

        public static bool StateTypeGet(this StateComponentS self, StateType nowStateType)
        {
            StateType state = self.CurrentStateType & nowStateType;

            if (state > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsStateBroadcastType(this StateComponentS self, StateType nowStateType)
        {
            // return nowStateType == StateTypeEnum.Singing;
            return true;
        }
    }
}