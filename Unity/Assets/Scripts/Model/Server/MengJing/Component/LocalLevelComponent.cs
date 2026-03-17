using System.Collections.Generic;
using Unity.Mathematics;

namespace ET.Server
{
    public enum LocalLevelState
    {
        None,               // 未开始
        Fighting,           // 战斗中(生成怪物+战斗)
        WaitEnterBoss,      // 等待玩家进入Boss房间
        BattleFailure,      // 英雄全部死亡，战斗失败
        Completed,          // 全部关卡完成
    }

    [ComponentOf(typeof(Scene))]
    public class LocalLevelComponent : Entity, IAwake, IDestroy
    {
        public long Timer;
        public long LastUpdateTime;

        private EntityRef<Unit> mainUnit;
        public Unit MainUnit { get => this.mainUnit; set => this.mainUnit = value; }

        public LocalLevelState LevelState;

        public int SpawnedMonsterIndex;
        public float SpawnTime;

        // 记录生成的英雄Unit,用于检测英雄死亡
        public List<long> HeroUnitIds = new();
    }
}