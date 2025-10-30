using System;
using Unity.Mathematics;

namespace ET.Server
{
    [EntitySystemOf(typeof(LocalLevelComponent))]
    [FriendOf(typeof(LocalLevelComponent))]
    public static partial class LocalLevelComponentSystem
    {
        [Invoke(TimerInvokeType.LocalLevelTimer)]
        public class LocalLevelTimer : ATimer<LocalLevelComponent>
        {
            protected override void Run(LocalLevelComponent self)
            {
                try
                {
                    self.Update();
                }
                catch (Exception e)
                {
                    Log.Error(e.ToString());
                }
            }
        }

        [EntitySystem]
        private static void Awake(this LocalLevelComponent self)
        {
            self.TimeInterval = 100;
        }

        [EntitySystem]
        private static void Destroy(this LocalLevelComponent self)
        {
            self.SpawnedMonsterBatchIds.Clear();
            self.SpawnedMonsterBatchIds = null;
            self.Root().GetComponent<TimerComponent>().Remove(ref self.Timer);
        }

        private static void Update(this LocalLevelComponent self)
        {
            if (self.MainUnit == null)
            {
                return;
            }

            if (self.WaitPlayerEnterBossRoom)
            {
                return;
            }

            self.SpawnTime += self.TimeInterval / 1000f + self.Scene().TimeScale;

            NumericComponentS numericComponent = self.MainUnit.GetComponent<NumericComponentS>();
            LevelConfig levelConfig = LevelConfigCategory.Instance.Get(numericComponent.GetAsInt(NumericType.CurrentLevelId));
            WaveConfig waveConfig = WaveConfigCategory.Instance.Get(levelConfig.WaveIds[numericComponent.GetAsInt(NumericType.CurrentWaveIndex) - 1]);

            foreach (int monsterBatchId in waveConfig.MonsterBatchIds)
            {
                MonsterBatchConfig monsterBatchConfig = MonsterBatchConfigCategory.Instance.Get(monsterBatchId);
                if (self.SpawnedMonsterBatchIds.Contains(monsterBatchConfig.Id))
                {
                    continue;
                }

                if (self.SpawnTime < monsterBatchConfig.SpawnTime)
                {
                    continue;
                }
                
                self.SpawnedMonsterBatchIds.Add(monsterBatchConfig.Id);
                float2 position = new float2(monsterBatchConfig.SpawnPosition.X, self.MainUnit.Position.y + monsterBatchConfig.SpawnPosition.Y);
                UnitFactory.CreateMonster(self.Scene(), monsterBatchConfig.MonsterId, position);
            }
        }

        public static void OnKillEvent(this LocalLevelComponent self, Unit unit)
        {
            if (self.MainUnit == null)
            {
                return;
            }

            if (unit.Type != UnitType.Monster)
            {
                return;
            }

            NumericComponentS numericComponent = self.MainUnit.GetComponent<NumericComponentS>();

            numericComponent.ApplyChange(NumericType.CurrentWaveKillMonsterNum, 1);

            LevelConfig levelConfig = LevelConfigCategory.Instance.Get(numericComponent.GetAsInt(NumericType.CurrentLevelId));
            WaveConfig waveConfig = WaveConfigCategory.Instance.Get(levelConfig.WaveIds[numericComponent.GetAsInt(NumericType.CurrentWaveIndex) - 1]);
            if (numericComponent.GetAsInt(NumericType.CurrentWaveKillMonsterNum) >= waveConfig.MonsterBatchIds.Length)
            {
                if (numericComponent.GetAsInt(NumericType.CurrentWaveIndex) >= levelConfig.WaveIds.Length)
                {
                    // 击败最后一波怪物 看看是继续下一关还是直接返回
                }
                else
                {
                    WaveConfig nextWaveConfig = WaveConfigCategory.Instance.Get(levelConfig.WaveIds[numericComponent.GetAsInt(NumericType.CurrentWaveIndex)]);
                    if (nextWaveConfig.HaveBoss)
                    {
                        // 等待玩家进入Boss房间
                        self.WaitPlayerEnterBossRoom = true;
                    }
                    else
                    {
                        // 开始生成下一波怪物
                        numericComponent.ApplyChange(NumericType.CurrentWaveIndex, 1);
                        numericComponent.ApplyValue(NumericType.CurrentWaveKillMonsterNum, 0);
                        self.SpawnedMonsterBatchIds.Clear();
                        self.SpawnTime = 0;
                    }
                }
            }
        }

        public static void EnterBossRoom(this LocalLevelComponent self)
        {
            if (self.MainUnit == null)
            {
                return;
            }

            if (!self.WaitPlayerEnterBossRoom)
            {
                return;
            }
            
            NumericComponentS numericComponent = self.MainUnit.GetComponent<NumericComponentS>();
            LevelConfig levelConfig = LevelConfigCategory.Instance.Get(numericComponent.GetAsInt(NumericType.CurrentLevelId));
            if (numericComponent.GetAsInt(NumericType.CurrentWaveIndex) >= levelConfig.WaveIds.Length)
            {
                return;
            }

            WaveConfig nextWaveConfig = WaveConfigCategory.Instance.Get(levelConfig.WaveIds[numericComponent.GetAsInt(NumericType.CurrentWaveIndex)]);
            if (nextWaveConfig.HaveBoss)
            {
                numericComponent.ApplyChange(NumericType.CurrentWaveIndex, 1);
                numericComponent.ApplyValue(NumericType.CurrentWaveKillMonsterNum, 0);
                self.SpawnedMonsterBatchIds.Clear();
                self.SpawnTime = 0;
                self.WaitPlayerEnterBossRoom = false;
            }
        }

        public static void GenerateLevel(this LocalLevelComponent self)
        {
            if (self.MainUnit == null)
            {
                return;
            }

            NumericComponentS numericComponent = self.MainUnit.GetComponent<NumericComponentS>();

            if (!LevelConfigCategory.Instance.DataMap.ContainsKey(numericComponent.GetAsInt(NumericType.PassedLevelId)))
            {
                // 防止后面配置表改了
                numericComponent.ApplyValue(NumericType.PassedLevelId, 0);
            }

            if (numericComponent.GetAsInt(NumericType.PassedLevelId) >= LevelConfigCategory.Instance.DataList[^1].Id)
            {
                // 已经通关最后一关
                return;
            }

            if (numericComponent.GetAsInt(NumericType.PassedLevelId) == 0)
            {
                // 第一关
                numericComponent.ApplyValue(NumericType.CurrentLevelId, LevelConfigCategory.Instance.DataList[0].Id);
            }
            else
            {
                // 下一关
                bool next = false;
                foreach (LevelConfig config in LevelConfigCategory.Instance.DataList)
                {
                    if (next)
                    {
                        numericComponent.ApplyValue(NumericType.CurrentLevelId, config.Id);
                        break;
                    }

                    if (config.Id == numericComponent.GetAsInt(NumericType.PassedLevelId))
                    {
                        next = true;
                    }
                }
            }

            LevelConfig levelConfig = LevelConfigCategory.Instance.Get(numericComponent.GetAsInt(NumericType.CurrentLevelId));

            self.SpawnedMonsterBatchIds.Clear();
            self.SpawnTime = 0;
            numericComponent.ApplyValue(NumericType.CurrentWaveIndex, 1);
            numericComponent.ApplyValue(NumericType.CurrentWaveKillMonsterNum, 0);

            self.Timer = self.Root().GetComponent<TimerComponent>().NewRepeatedTimer(self.TimeInterval, TimerInvokeType.LocalLevelTimer, self);
        }
    }
}