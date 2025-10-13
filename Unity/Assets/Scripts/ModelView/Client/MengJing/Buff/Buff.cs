using Unity.Mathematics;

namespace ET.Client
{
    public enum BuffState
    {
        None, //回收
        WaitRemove, //等待移除
        Running, //正在执行
        Finished, //Buff使命完成
    }

    public struct BuffData
    {
        //buff角度
        public int TargetAngle;
        public long BuffEndTime;
        public string Spellcaster;
        public int UnitType;
        public int UnitConfigId;
        public int SkillConfigId;
        public int BuffConfigId;
        public long UnitIdFrom;
        public float3 TargetPostion;
    }

    [ChildOf(typeof(BuffManagerComponent))]
    public class Buff : Entity, IAwake, IDestroy
    {
        public BuffData buffData { get; set; }
        public BuffState BuffState { get; set; }
        public BuffHandler BuffHandler { get; set; }
        public BuffConfig BuffConfig { get; set; }
    }
}