using System.Collections.Generic;

namespace ET.Server
{ 
    /// <summary>
    /// 副本组件
    /// </summary>
    [ComponentOf(typeof(Scene))]
    public class CellDungeonComponentS : Entity, IAwake
    {
        public int ChapterId { get; set; }
        public long EnterTime { get; set; }
        public long HurtValue { get; set; }
        public CellGenerateConfig ChapterConfig { get; set; }
        public int FubenDifficulty { get; set; }

        public FubenInfo FubenInfo { get; set; } = FubenInfo.Create();
        public SonFubenInfo SonFubenInfo { get; set; } = SonFubenInfo.Create();

        public CellDungeonInfo[][] FubenCellInfoList { get; set; }

        //神秘商品
        public List<int> EnergySkills { get; set; } = new List<int>() { };
        public List<MysteryItemInfo> MysteryItemInfos  { get; set; }= new List<MysteryItemInfo>();
        public CellDungeonInfo CurrentFubenCell { get; set; }
    }   //队长
}
