namespace ET.Client
{
    [EntitySystemOf(typeof(StateComponentC))]
    [FriendOf(typeof(StateComponentC))]
    public static partial class StateComponentCSystem
    {
        [EntitySystem]
        private static void Awake(this StateComponentC self)
        {
            self.CurrentStateType = StateTypeEnum.None;
        }

        public static void Reset(this StateComponentC self)
        {
            self.CurrentStateType = StateTypeEnum.None;
        }

        public static void StateTypeAdd(this StateComponentC self, long nowStateType)
        {
            Unit unit = self.GetParent<Unit>();
            self.CurrentStateType = self.CurrentStateType | nowStateType;
        }

        public static void StateTypeRemove(this StateComponentC self, long nowStateType)
        {
            self.CurrentStateType = self.CurrentStateType & ~nowStateType;
        }

        public static bool StateTypeGet(this StateComponentC self, long nowStateType)
        {
            long state = (self.CurrentStateType & nowStateType);

            if (state > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}