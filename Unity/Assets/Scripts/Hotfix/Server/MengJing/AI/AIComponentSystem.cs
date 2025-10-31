using System;
using Unity.Mathematics;

namespace ET.Server
{
    [EntitySystemOf(typeof(AIComponent))]
    [FriendOf(typeof(AIComponent))]
    [FriendOf(typeof(AIDispatcherComponent))]
    public static partial class AIComponentSystem
    {
        [Invoke(TimerInvokeType.AITimer)]
        public class AITimer : ATimer<AIComponent>
        {
            protected override void Run(AIComponent self)
            {
                try
                {
                    self.Check();
                }
                catch (Exception e)
                {
                    Log.Error($"move timer error: {self.Id}\n{e}");
                }
            }
        }

        [EntitySystem]
        private static void Awake(this AIComponent self, int aiConfigId)
        {
            self.AIConfigId = aiConfigId;
            self.IsRetreat = false;
            self.PatrolRange = 0f;
            self.AISkillIDList.Clear();
            self.TargetPoint.Clear();
            self.TargetZhuiJi = float3.zero;
            self.SceneType = self.Scene().GetComponent<MapComponent>().MapType;
        }

        [EntitySystem]
        private static void Destroy(this AIComponent self)
        {
            self.Root().GetComponent<TimerComponent>()?.Remove(ref self.Timer);
            self.CancellationToken?.Cancel();
            self.CancellationToken = null;
            self.Current = 0;
        }

        private static void Check(this AIComponent self)
        {
            Fiber fiber = self.Fiber();
            if (self.Parent == null)
            {
                fiber.Root.GetComponent<TimerComponent>().Remove(ref self.Timer);
                return;
            }

            if (self.AIDelay >= 0)
            {
                self.AIDelay -= 500;
                return;
            }

            var oneAI = AIConfigCategory.Instance.AIConfigs[self.AIConfigId];

            foreach (AIConfig aiConfig in oneAI.Values)
            {
                AAIHandler aaiHandler = AIDispatcherComponent.Instance.Get(aiConfig.Name);

                if (aaiHandler == null)
                {
                    Log.Error($"not found aihandler: {aiConfig.Name}");
                    continue;
                }

                int ret = aaiHandler.Check(self, aiConfig);
                if (ret != 0)
                {
                    continue;
                }

                if (self.Current == aiConfig.Id)
                {
                    break;
                }

                self.Cancel(); // 取消之前的行为
                ETCancellationToken cancellationToken = new();
                self.CancellationToken = cancellationToken;
                self.Current = aiConfig.Id;

                aaiHandler.Execute(self, aiConfig, cancellationToken).Coroutine();
                return;
            }
        }

        public static void ChangeTarget(this AIComponent self, long targetId)
        {
            if (!self.IsRetreat)
            {
                self.TargetID = targetId;
            }
        }

        private static void Cancel(this AIComponent self)
        {
            self.CancellationToken?.Cancel();
            self.Current = 0;
            self.CancellationToken = null;
        }

        public static void InitMonster(this AIComponent self)
        {
            Unit unit = self.GetParent<Unit>();
            MonsterConfig monsterConfig = MonsterConfigCategory.Instance.Get(unit.ConfigId);
            NumericComponentS numericComponent = unit.GetComponent<NumericComponentS>();

            self.ChaseRange = 100;
            self.ActRange = 100;
            self.ActDistance = monsterConfig.ActDistance / 10000f;
            self.AISkillIDList.Add(monsterConfig.ActSkillID);
        }

        public static void InitHero(this AIComponent self)
        {
            Unit unit = self.GetParent<Unit>();
            HeroConfig heroConfig = HeroConfigCategory.Instance.Get(unit.ConfigId);
            NumericComponentS numericComponent = unit.GetComponent<NumericComponentS>();

            self.ChaseRange = 100;
            self.ActRange = 100;
            self.ActDistance = heroConfig.AtkDistance / 10000f;
            self.AISkillIDList.Add(heroConfig.AtkID);
        }

        public static bool HaveSkillId(this AIComponent self, int skillId)
        {
            return self.AISkillIDList.Contains(skillId);
        }

        public static int GetActSkillId(this AIComponent self)
        {
            return self.AISkillIDList[RandomHelper.RandomNumber(0, self.AISkillIDList.Count)];
        }

        public static void BeAttacking(this AIComponent self, Unit attack)
        {
            Unit main = self.GetParent<Unit>();
            if (!main.IsCanAttackUnit(attack))
            {
                return;
            }

            long serverTime = TimeHelper.ServerNow();
            if (serverTime - self.LastChangeTime < 6000)
            {
                return;
            }

            self.LastChangeTime = serverTime;
            if (main.Type == UnitType.Hero)
            {
                bool gaiLv = RandomHelper.RandFloat01() < 0.1f;
                if (self.TargetID != 0 && gaiLv)
                {
                    self.ChangeTarget(attack.Id);
                }

                if (self.TargetID == 0)
                {
                    self.ChangeTarget(attack.Id);
                }
            }
            else
            {
                //怪物
                //1.首先攻击默认攻击他的目标。
                //2.攻击时有概率转换自己为攻击目标（近战攻击10 %，远程攻击5 %）。
                //3.如果转换攻击目标，6秒内不在转换攻击目标
                float gaiLv = RandomHelper.RandFloat01();
                if (self.TargetID != 0 && self.ActDistance <= 4 && gaiLv <= 0.1f)
                {
                    self.ChangeTarget(attack.Id);
                }

                if (self.TargetID != 0 && self.ActDistance > 4 && gaiLv <= 0.05f)
                {
                    self.ChangeTarget(attack.Id);
                }

                if (self.TargetID == 0)
                {
                    self.ChangeTarget(attack.Id);
                }
            }
        }

        public static void Begin(this AIComponent self)
        {
            self.Root().GetComponent<TimerComponent>().Remove(ref self.Timer);
            NumericComponentS numericComponent = self.GetParent<Unit>().GetComponent<NumericComponentS>();
            if (numericComponent.GetAsInt(NumericType.Now_Dead) != 0)
            {
                return;
            }

            self.AIDelay -= 500;
            self.Timer = self.Root().GetComponent<TimerComponent>().NewRepeatedTimer(500, TimerInvokeType.AITimer, self);
        }

        public static void Stop(this AIComponent self)
        {
            self.TargetID = 0;
            self.Root().GetComponent<TimerComponent>().Remove(ref self.Timer);
        }

        public static void Stop_2(this AIComponent self)
        {
            self.Cancel();
            self.Current = -1;
            self.Root().GetComponent<TimerComponent>().Remove(ref self.Timer);
        }

        public static float GetActRange(this AIComponent self)
        {
            return self.ActRange;
        }
    }
}