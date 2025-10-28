namespace ET.Server
{
    [EntitySystemOf(typeof(LocalLevelComponent))]
    [FriendOf(typeof(LocalLevelComponent))]
    public static partial class LocalLevelComponentSystem
    {
        [EntitySystem]
        private static void Awake(this LocalLevelComponent self)
        {
        }

        [EntitySystem]
        private static void Destroy(this LocalLevelComponent self)
        {
        }

        public static void GenerateLevel(this LocalLevelComponent self)
        {
            if (self.MainUnit == null)
            {
                return;
            }

            NumericComponentS numericComponent = self.MainUnit.GetComponent<NumericComponentS>();

            if (numericComponent.GetAsInt(NumericType.PassedLevelId) == 0)
            {
                self.CurrentLevelId = LevelConfigCategory.Instance.DataList[0].Id;
            }
            else
            {
                bool next = false;
                foreach (LevelConfig config in LevelConfigCategory.Instance.DataList)
                {
                    if (next)
                    {
                        self.CurrentLevelId = config.Id;
                        break;
                    }

                    if (config.Id == numericComponent.GetAsInt(NumericType.PassedLevelId))
                    {
                        next = true;
                    }
                }
            }

            if (self.CurrentLevelId == 0)
            {
                return;
            }
            
            
        }
    }
}