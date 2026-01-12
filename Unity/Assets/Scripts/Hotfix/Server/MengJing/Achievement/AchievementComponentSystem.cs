namespace ET.Server
{
    [Event(SceneType.Map)]
    public class TriggerAchievement_Notify : AEvent<Scene, TriggerAchievement>
    {
        protected override async ETTask Run(Scene scene, TriggerAchievement args)
        {
            args.Unit.GetComponent<AchievementComponent>()?.TriggerEvent(args.TargetType, args.TargetId, args.TargetValue, true);

            await ETTask.CompletedTask;
        }
    }

    [FriendOf(typeof(AchievementComponent))]
    [FriendOf(typeof(HeroComponent))]
    [EntitySystemOf(typeof(AchievementComponent))]
    public static partial class AchievementComponentSystem
    {
        [EntitySystem]
        private static void Awake(this AchievementComponent self)
        {
        }

        [EntitySystem]
        private static void Destroy(this AchievementComponent self)
        {
            self.ReceivedAchievementRewardIds.Clear();
            self.AchievementList.Clear();
        }

        [EntitySystem]
        private static void Deserialize(this AchievementComponent self)
        {
            foreach (Entity entity in self.Children.Values)
            {
                if (entity is Achievement achievement)
                {
                    self.AchievementList.Add(achievement);
                }
            }
        }

        public static void OnLogin(this AchievementComponent self)
        {
            foreach (AchievementConfig achievementConfig in AchievementConfigCategory.Instance.DataList)
            {
                bool exist = false;
                foreach (Achievement achievement in self.AchievementList)
                {
                    if (achievement.ConfigId == achievementConfig.Id)
                    {
                        exist = true;
                        break;
                    }
                }

                if (exist)
                {
                    continue;
                }

                Achievement newAchievement = self.AddChild<Achievement>();
                newAchievement.ConfigId = achievementConfig.Id;
                self.AchievementList.Add(newAchievement);
            }

            self.TriggerEvent(AchievementTargetType.HaveHeroId, 0, 0, false);
            self.TriggerEvent(AchievementTargetType.HaveHeroValue, 0, 0, false);
            self.TriggerEvent(AchievementTargetType.HeroLv, 0, 0, false);
        }

        public static void TriggerEvent(this AchievementComponent self, AchievementTargetType targetType, int targetId, int targetValue, bool notice)
        {
            HeroComponent heroComponent = self.GetParent<Unit>().GetComponent<HeroComponent>();

            using ListComponent<AchievementInfo> achievementInfoList = ListComponent<AchievementInfo>.Create();
            foreach (Achievement achievement in self.AchievementList)
            {
                AchievementConfig achievementConfig = AchievementConfigCategory.Instance.Get(achievement.ConfigId);
                if (achievementConfig.AchievementTargetType != targetType)
                {
                    continue;
                }

                if (achievement.IsCompleted != 0)
                {
                    continue;
                }

                bool update = false;
                switch (targetType)
                {
                    case AchievementTargetType.HaveHeroId:
                    {
                        if (heroComponent.GetHeroByConfigId(achievementConfig.TargetID) != null)
                        {
                            achievement.IsCompleted = 1;
                            achievement.Progress = achievementConfig.TargetValue;

                            update = true;
                        }

                        break;
                    }
                    case AchievementTargetType.HaveHeroValue:
                    {
                        int count = heroComponent.Heros.Count;

                        if (count != achievement.Progress)
                        {
                            update = true;
                        }

                        achievement.Progress = count;

                        if (achievement.Progress >= achievementConfig.TargetValue)
                        {
                            achievement.IsCompleted = 1;
                            achievement.Progress = achievementConfig.TargetValue;
                        }

                        break;
                    }
                    case AchievementTargetType.HeroLv:
                    {
                        int count = 0;
                        foreach (Hero hero in heroComponent.Heros)
                        {
                            if (hero.Lv >= achievementConfig.TargetID)
                            {
                                count++;
                            }
                        }

                        if (count != achievement.Progress)
                        {
                            update = true;
                        }

                        achievement.Progress = count;

                        if (achievement.Progress >= achievementConfig.TargetValue)
                        {
                            achievement.IsCompleted = 1;
                            achievement.Progress = achievementConfig.TargetValue;
                        }

                        break;
                    }
                }

                if (update)
                {
                    achievementInfoList.Add(achievement.ToMessage());
                }
            }

            if (achievementInfoList.Count > 0 && notice)
            {
                M2C_AchievementUpdate message = M2C_AchievementUpdate.Create();
                foreach (AchievementInfo achievementInfo in achievementInfoList)
                {
                    message.AchievementInfoList.Add(achievementInfo);
                }

                MapMessageHelper.SendToClient(self.GetParent<Unit>(), message);
            }
        }

        public static int GetCurrentPoint(this AchievementComponent self)
        {
            int point = 0;
            foreach (Achievement achievement in self.AchievementList)
            {
                if (achievement.IsCompleted != 0)
                {
                    AchievementConfig achievementConfig = AchievementConfigCategory.Instance.Get(achievement.ConfigId);
                    point += achievementConfig.RewardPoints;
                }
            }

            return point;
        }
    }
}