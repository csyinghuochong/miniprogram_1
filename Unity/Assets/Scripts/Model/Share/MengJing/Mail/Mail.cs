namespace ET
{
    public enum MainState
    {
    }
    
    [ChildOf]
    public class Mail : Entity, IAwake, IDestroy, ISerializeToEntity, IDeserialize
    {
        public int State;
        public string Title;
        public string Content;
        public long Time;
        public long DeleteTime;
    }
}