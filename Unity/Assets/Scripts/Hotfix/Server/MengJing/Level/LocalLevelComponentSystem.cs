using System;
using System.Collections.Generic;
using System.Numerics;
using Unity.Mathematics;

namespace ET.Server
{
    [EntitySystemOf(typeof(LocalLevelComponent))]
    [FriendOf(typeof(LocalLevelComponent))]
    [FriendOf(typeof(HeroComponentS))]
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
        }

        [EntitySystem]
        private static void Destroy(this LocalLevelComponent self)
        {
            self.Root().GetComponent<TimerComponent>().Remove(ref self.Timer);
            self.HeroUnitIds.Clear();
        }

        private static void Update(this LocalLevelComponent self)
        {
            long now = TimeInfo.Instance.ClientNow();
            float deltaTime = (now - self.LastUpdateTime) / 1000f * self.Scene().TimeScale;
            self.LastUpdateTime = now;

            if (self.MainUnit == null || self.LevelState == LocalLevelState.None || self.LevelState == LocalLevelState.Completed)
            {
                return;
            }

            if (self.MainUnit.GetComponent<NumericComponentS>().GetAsInt(NumericType.BattleMode) == 1)
            {
                // 玩家同步到坐标Y值最大的英雄上
                float3 maxPos = float3.zero;
                foreach (Unit u in self.Scene().GetComponent<UnitComponent>().GetAll())
                {
                    if (u.Type != UnitType.Hero)
                    {
                        continue;
                    }

                    if (maxPos.Equals(float3.zero))
                    {
                        maxPos = u.Position;
                    }

                    if (u.Position.y > maxPos.y)
                    {
                        maxPos = u.Position;
                    }
                }

                if (!maxPos.Equals(float3.zero))
                {
                    self.MainUnit.Position = maxPos;
                    self.MainUnit.Stop();
                }
            }

            // 所有状态都检测英雄存活
            if (!self.CheckHeroesAlive())
            {
                return; // 英雄全灭了,已经触发重置
            }

            switch (self.LevelState)
            {
                case LocalLevelState.Fighting:
                    self.UpdateFighting(deltaTime);
                    break;
                case LocalLevelState.WaitEnterBoss:
                    // 等待玩家手动调用EnterBossRoom()
                    break;
            }
        }

        private static void UpdateFighting(this LocalLevelComponent self, float deltaTime)
        {
            NumericComponentS numericComponent = self.MainUnit.GetComponent<NumericComponentS>();
            LevelConfig levelConfig = LevelConfigCategory.Instance.Get(numericComponent.GetAsInt(NumericType.CurrentLevelId));
            int currentWaveIndex = numericComponent.GetAsInt(NumericType.CurrentWaveIndex);

            if (currentWaveIndex > levelConfig.WaveIds.Length)
            {
                return;
            }

            WaveConfig waveConfig = WaveConfigCategory.Instance.Get(levelConfig.WaveIds[currentWaveIndex - 1]);

            // 还有怪物没生成,继续生成
            if (self.SpawnedMonsterIndex < waveConfig.MonsterSpawnInfos.Length)
            {
                self.SpawnTime += deltaTime;

                MonsterSpawnInfo monsterSpawnInfo = waveConfig.MonsterSpawnInfos[self.SpawnedMonsterIndex];
                if (self.SpawnTime >= monsterSpawnInfo.SpawnTime)
                {
                    self.SpawnedMonsterIndex++;

                    float2 position;
                    if (waveConfig.HaveBoss)
                    {
                        // Boss房间怪物生成相对玩家出生点
                        position = new float2(waveConfig.PlayerSpawnPosition.X + monsterSpawnInfo.SpawnPosition.X, waveConfig.PlayerSpawnPosition.Y + monsterSpawnInfo.SpawnPosition.Y);
                    }
                    else
                    {
                        // 循环路上怪物生成相对地图X坐标和玩家Y坐标
                        position = new float2(waveConfig.PlayerSpawnPosition.X + monsterSpawnInfo.SpawnPosition.X, self.MainUnit.Position.y + monsterSpawnInfo.SpawnPosition.Y);
                    }

                    UnitFactory.CreateMonster(self.Scene(), monsterSpawnInfo.MonsterId, position);
                }
            }
        }

        private static void GenerateHeroes(this LocalLevelComponent self)
        {
            if (self.MainUnit == null)
            {
                return;
            }

            self.HeroUnitIds.Clear();
            
            NumericComponentS numericComponent = self.MainUnit.GetComponent<NumericComponentS>();
            LevelConfig levelConfig = LevelConfigCategory.Instance.Get(numericComponent.GetAsInt(NumericType.CurrentLevelId));
            WaveConfig firstWaveConfig = WaveConfigCategory.Instance.Get(levelConfig.WaveIds[0]);

            float3 playerPosition = new float3(firstWaveConfig.PlayerSpawnPosition.X, firstWaveConfig.PlayerSpawnPosition.Y, 0);
            self.MainUnit.Position = playerPosition;
            self.MainUnit.Stop();

            // 创建英雄队列
            HeroComponentS heroComponent = self.MainUnit.GetComponent<HeroComponentS>();
            for (int i = 0; i < heroComponent.Formation.Count; i++)
            {
                Hero hero = heroComponent.GetHero(heroComponent.Formation[i]);
                if (hero == null)
                {
                    continue;
                }

                float3 position = heroComponent.GetHeroPosition(hero.Id);

                Unit heroUnit = UnitFactory.CreateHero(self.Scene(), self.MainUnit, hero, position);
                
                self.HeroUnitIds.Add(heroUnit.Id);
            }

            Log.Info($"生成了{self.HeroUnitIds.Count}个英雄");

            // 开始战斗,生成第一波怪物
            self.LevelState = LocalLevelState.Fighting;
            self.SpawnedMonsterIndex = 0;
            self.SpawnTime = 0;
        }

        private static bool CheckHeroesAlive(this LocalLevelComponent self)
        {
            if (self.HeroUnitIds.Count == 0)
            {
                return true; // 还没生成英雄
            }

            UnitComponent unitComponent = self.Scene().GetComponent<UnitComponent>();
            int aliveCount = 0;

            foreach (long heroUnitId in self.HeroUnitIds)
            {
                Unit heroUnit = unitComponent.Get(heroUnitId);
                if (heroUnit != null && heroUnit.GetComponent<NumericComponentS>().GetAsInt(NumericType.Now_Dead) == 0)
                {
                    aliveCount++;
                }
            }

            // 所有英雄都死亡
            if (aliveCount == 0)
            {
                Log.Warning("所有英雄死亡,关卡失败,重置关卡");
                // self.ResetLevel();
                return false;
            }

            return true;
        }

        public static void OnKillEvent(this LocalLevelComponent self, Unit unit)
        {
            if (self.MainUnit == null)
            {
                return;
            }

            // 只处理敌方怪物死亡
            if (unit.Type != UnitType.Monster || unit.GetBattleCamp() != (int)CampType.CampMonster1)
            {
                return;
            }

            NumericComponentS numericComponent = self.MainUnit.GetComponent<NumericComponentS>();

            // 增加击杀计数
            numericComponent.ApplyChange(NumericType.CurrentWaveKillMonsterNum, 1);

            int currentWaveIndex = numericComponent.GetAsInt(NumericType.CurrentWaveIndex);
            int currentWaveKillMonsterNum = numericComponent.GetAsInt(NumericType.CurrentWaveKillMonsterNum);

            LevelConfig levelConfig = LevelConfigCategory.Instance.Get(numericComponent.GetAsInt(NumericType.CurrentLevelId));
            WaveConfig waveConfig = WaveConfigCategory.Instance.Get(levelConfig.WaveIds[currentWaveIndex - 1]);

            // 检查当前波次是否击杀完毕
            if (currentWaveKillMonsterNum >= waveConfig.MonsterSpawnInfos.Length)
            {
                // 检查是否是最后一波
                if (currentWaveIndex >= levelConfig.WaveIds.Length)
                {
                    // 所有波次完成
                    if (waveConfig.HaveBoss)
                    {
                        // Boss被击败,传送回关卡
                        self.ReturnFromBossRoom();
                    }
                    // 进行下一关
                    self.GoToNextLevel();
                }
                else
                {
                    // 还有下一波
                    WaveConfig nextWaveConfig = WaveConfigCategory.Instance.Get(levelConfig.WaveIds[currentWaveIndex]);
                    if (nextWaveConfig.HaveBoss)
                    {
                        // 等待玩家进入Boss房间
                        self.LevelState = LocalLevelState.WaitEnterBoss;
                        Log.Info("当前波次完成,等待玩家进入Boss房间");
                    }
                    else
                    {
                        // 开始生成下一波怪物
                        self.StartNextWave();
                    }
                }
            }
        }

        private static void ResetLevel(this LocalLevelComponent self)
        {
            if (self.MainUnit == null)
            {
                return;
            }

            Log.Info("重置关卡");

            // 清除场景中所有怪物和英雄
            UnitComponent unitComponent = self.Scene().GetComponent<UnitComponent>();
            List<Unit> toRemove = new List<Unit>();

            foreach (Unit unit in unitComponent.GetAll())
            {
                if (unit.Type == UnitType.Monster || unit.Type == UnitType.Hero)
                {
                    toRemove.Add(unit);
                }
            }

            foreach (Unit unit in toRemove)
            {
                unit.GetParent<UnitComponent>().Remove(unit.Id);
            }

            self.HeroUnitIds.Clear();

            NumericComponentS numericComponent = self.MainUnit.GetComponent<NumericComponentS>();

            // 重置波次数据
            numericComponent.ApplyValue(NumericType.CurrentWaveIndex, 1);
            numericComponent.ApplyValue(NumericType.CurrentWaveKillMonsterNum, 0);

            self.SpawnedMonsterIndex = 0;
            self.SpawnTime = 0;

            // 重新生成英雄阵容
            self.GenerateHeroes();
        }

        private static void StartNextWave(this LocalLevelComponent self)
        {
            NumericComponentS numericComponent = self.MainUnit.GetComponent<NumericComponentS>();

            numericComponent.ApplyChange(NumericType.CurrentWaveIndex, 1);
            numericComponent.ApplyValue(NumericType.CurrentWaveKillMonsterNum, 0);

            self.SpawnedMonsterIndex = 0;
            self.SpawnTime = 0;
            self.LevelState = LocalLevelState.Fighting;

            Log.Info($"开始第 {numericComponent.GetAsInt(NumericType.CurrentWaveIndex)} 波怪物");
        }

        public static void EnterBossRoom(this LocalLevelComponent self)
        {
            if (self.MainUnit == null)
            {
                return;
            }

            if (self.LevelState != LocalLevelState.WaitEnterBoss)
            {
                Log.Warning($"当前状态不是等待进入Boss房间,当前状态: {self.LevelState}");
                return;
            }

            NumericComponentS numericComponent = self.MainUnit.GetComponent<NumericComponentS>();
            LevelConfig levelConfig = LevelConfigCategory.Instance.Get(numericComponent.GetAsInt(NumericType.CurrentLevelId));
            int currentWaveIndex = numericComponent.GetAsInt(NumericType.CurrentWaveIndex);

            if (currentWaveIndex >= levelConfig.WaveIds.Length)
            {
                Log.Error("波次索引超出范围");
                return;
            }

            WaveConfig nextWaveConfig = WaveConfigCategory.Instance.Get(levelConfig.WaveIds[currentWaveIndex]);
            if (!nextWaveConfig.HaveBoss)
            {
                Log.Error("下一波配置不是Boss房间");
                return;
            }

            Log.Info("传送到Boss房间");
            
            self.ClearDropItem();

            // 传送玩家到Boss房间
            self.Scene().GetComponent<CrowdComponent>().ChangePosition(self.MainUnit.DtCrowdAgentId, new float3(nextWaveConfig.PlayerSpawnPosition.X, nextWaveConfig.PlayerSpawnPosition.Y, 0));
            self.MainUnit.Stop();

            // 传送所有英雄到Boss房间
            UnitComponent unitComponent = self.Scene().GetComponent<UnitComponent>();
            foreach (long heroUnitId in self.HeroUnitIds)
            {
                Unit heroUnit = unitComponent.Get(heroUnitId);
                if (heroUnit != null && heroUnit.Type == UnitType.Hero)
                {
                    float3 offset = self.MainUnit.GetComponent<HeroComponentS>().GetHeroPosition(heroUnitId);
                    float3 spawnPos = new float3(nextWaveConfig.PlayerSpawnPosition.X, nextWaveConfig.PlayerSpawnPosition.Y, 0);
                    float3 newPosition = spawnPos + offset;
                    self.Scene().GetComponent<CrowdComponent>().ChangePosition(heroUnit.DtCrowdAgentId, new float3(newPosition.x, newPosition.y, 0));
                    heroUnit.Stop();
                }
            }

            // 开始生成Boss波次
            numericComponent.ApplyChange(NumericType.CurrentWaveIndex, 1);
            numericComponent.ApplyValue(NumericType.CurrentWaveKillMonsterNum, 0);
            self.SpawnedMonsterIndex = 0;
            self.SpawnTime = 0;
            self.LevelState = LocalLevelState.Fighting;

            Log.Info($"开始生成Boss波次: 第 {numericComponent.GetAsInt(NumericType.CurrentWaveIndex)} 波");
        }

        private static void ReturnFromBossRoom(this LocalLevelComponent self)
        {
            if (self.MainUnit == null)
            {
                return;
            }

            Log.Info("从Boss房间传送回关卡");
            
            self.ClearDropItem();

            // 传送玩家回到循环地图
            float3 levelPos = new float3(0, 100f, 0);
            self.Scene().GetComponent<CrowdComponent>().ChangePosition(self.MainUnit.DtCrowdAgentId, new float3(levelPos.x, levelPos.y, 0));
            self.MainUnit.Stop();

            // 传送所有英雄回到关卡(保持相对位置)
            UnitComponent unitComponent = self.Scene().GetComponent<UnitComponent>();
            foreach (long heroUnitId in self.HeroUnitIds)
            {
                Unit heroUnit = unitComponent.Get(heroUnitId);
                if (heroUnit != null && heroUnit.Type == UnitType.Hero)
                {
                    float3 offset = self.MainUnit.GetComponent<HeroComponentS>().GetHeroPosition(heroUnitId);
                    float3 newPosition = levelPos + offset;
                    self.Scene().GetComponent<CrowdComponent>().ChangePosition(heroUnit.DtCrowdAgentId, new float3(newPosition.x, newPosition.y, 0));
                    heroUnit.Stop();
                }
            }
        }

        private static void GoToNextLevel(this LocalLevelComponent self)
        {
            if (self.MainUnit == null)
            {
                return;
            }

            NumericComponentS numericComponent = self.MainUnit.GetComponent<NumericComponentS>();
            int currentLevelId = numericComponent.GetAsInt(NumericType.CurrentLevelId);

            Log.Info($"完成关卡 {currentLevelId}");

            // 更新已通关的关卡ID
            numericComponent.ApplyValue(NumericType.PassedLevelId, currentLevelId);

            // 检查是否已经通关所有关卡
            if (currentLevelId >= LevelConfigCategory.Instance.DataList[^1].Id)
            {
                Log.Info("恭喜!已经通关所有关卡!");
                self.LevelState = LocalLevelState.Completed;
                // TODO: 可以在这里触发通关奖励、返回主城等逻辑
                return;
            }

            // 准备下一关
            bool foundNext = false;
            foreach (LevelConfig config in LevelConfigCategory.Instance.DataList)
            {
                if (config.Id > currentLevelId)
                {
                    numericComponent.ApplyValue(NumericType.CurrentLevelId, config.Id);
                    foundNext = true;
                    Log.Info($"准备进入下一关: {config.Id} - {config.LevelName}");
                    break;
                }
            }

            if (!foundNext)
            {
                Log.Error("找不到下一关配置");
                return;
            }

            // 重置关卡数据
            numericComponent.ApplyValue(NumericType.CurrentWaveIndex, 1);
            numericComponent.ApplyValue(NumericType.CurrentWaveKillMonsterNum, 0);
            self.SpawnedMonsterIndex = 0;
            self.SpawnTime = 0;

            // 重新生成英雄(传送到新关卡并生成英雄)
            // self.GenerateHeroes();
        }

        private static void ClearDropItem(this LocalLevelComponent self)
        {
            UnitComponent unitComponent = self.Scene().GetComponent<UnitComponent>();
            List<EntityRef<Unit>> allUnit = unitComponent.GetAll();
            for (int i = allUnit.Count - 1; i >= 0; i--)
            {
                Unit u = allUnit[i];
                if (u.Type == UnitType.DropItem)
                {
                    unitComponent.Remove(u.Id);
                }
            }
        }

        public static void GenerateLevel(this LocalLevelComponent self)
        {
            if (self.MainUnit == null)
            {
                Log.Error("MainUnit为空,无法生成关卡");
                return;
            }

            NumericComponentS numericComponent = self.MainUnit.GetComponent<NumericComponentS>();

            // 防止配置表改动导致的数据异常
            if (!LevelConfigCategory.Instance.DataMap.ContainsKey(numericComponent.GetAsInt(NumericType.PassedLevelId)))
            {
                numericComponent.ApplyValue(NumericType.PassedLevelId, 0);
            }

            // 检查是否已经通关所有关卡
            if (numericComponent.GetAsInt(NumericType.PassedLevelId) >= LevelConfigCategory.Instance.DataList[^1].Id)
            {
                Log.Info("已经通关最后一关");
                return;
            }

            // 确定当前要进入的关卡
            int targetLevelId;
            if (numericComponent.GetAsInt(NumericType.PassedLevelId) == 0)
            {
                // 从第一关开始
                targetLevelId = LevelConfigCategory.Instance.DataList[0].Id;
            }
            else
            {
                // 下一关
                bool foundNext = false;
                targetLevelId = 0;
                foreach (LevelConfig config in LevelConfigCategory.Instance.DataList)
                {
                    if (config.Id > numericComponent.GetAsInt(NumericType.PassedLevelId))
                    {
                        targetLevelId = config.Id;
                        foundNext = true;
                        break;
                    }
                }
            
                if (!foundNext)
                {
                    Log.Error("找不到下一关配置");
                    return;
                }
            }

            numericComponent.ApplyValue(NumericType.CurrentLevelId, targetLevelId);
            LevelConfig levelConfig = LevelConfigCategory.Instance.Get(targetLevelId);

            Log.Info($"开始生成关卡: {levelConfig.Id} - {levelConfig.LevelName}");

            // 重置关卡数据
            self.SpawnedMonsterIndex = 0;
            self.SpawnTime = 0;
            numericComponent.ApplyValue(NumericType.CurrentWaveIndex, 1);
            numericComponent.ApplyValue(NumericType.CurrentWaveKillMonsterNum, 0);

            // 启动定时器
            if (self.Timer == 0)
            {
                self.LastUpdateTime = TimeInfo.Instance.ClientNow();
                self.Timer = self.Root().GetComponent<TimerComponent>().NewFrameTimer(TimerInvokeType.LocalLevelTimer, self);
            }

            // 生成英雄并开始战斗
            self.GenerateHeroes();

            Log.Info("关卡初始化完成,开始战斗");
        }
    }
}
