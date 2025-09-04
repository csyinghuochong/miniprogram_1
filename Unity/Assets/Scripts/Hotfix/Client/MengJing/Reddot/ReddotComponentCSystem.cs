namespace ET.Client
{
    [FriendOf(typeof(ReddotComponentC))]
    [EntitySystemOf(typeof(ReddotComponentC))]
    public static partial class ReddotComponentCSystem
    {
        [EntitySystem]
        private static void Awake(this ReddotComponentC self)
        {
        }

        public static void UpdateReddont(this ReddotComponentC self, int reddotType)
        {
            bool showReddot = false;
            Unit unit = UnitHelper.GetMyUnitFromClientScene(self.Root());
            switch (reddotType)
            {
                case ReddotType.SingleRecharge:
                    break;
                default:
                    return;
            }

            if (showReddot)
            {
                self.AddReddont(reddotType);
            }
            else
            {
                self.RemoveReddont(reddotType);
            }
        }

        public static void AddReddont(this ReddotComponentC self, int reddotType)
        {
            bool have = false;
            for (int i = self.ReddontList.Count - 1; i >= 0; i--)
            {
                if (self.ReddontList[i].KeyId == reddotType)
                {
                    have = true;
                    break;
                }
            }

            if (!have)
            {
                self.ReddontList.Add(new KeyValuePair() { KeyId = reddotType, Value = "1" });
            }

            EventSystem.Instance.Publish(self.Root(), new ReddotChange() { ReddotType = reddotType, Number = 1 });
        }

        public static int GetReddot(this ReddotComponentC self, int reddotType)
        {
            for (int i = self.ReddontList.Count - 1; i >= 0; i--)
            {
                if (self.ReddontList[i].KeyId == reddotType)
                {
                    return 1;
                }
            }

            return 0;
        }

        public static void RemoveReddont(this ReddotComponentC self, int reddotType)
        {
            for (int i = self.ReddontList.Count - 1; i >= 0; i--)
            {
                if (self.ReddontList[i].KeyId == reddotType)
                {
                    self.ReddontList.RemoveAt(i);
                    break;
                }
            }

            EventSystem.Instance.Publish(self.Root(), new ReddotChange() { ReddotType = reddotType, Number = 0 });
        }
    }
}