namespace ET.Server
{
    [ComponentOf(typeof(Scene))]
    public class CenterSceneComponent : Entity, IAwake, IDestroy
    {
        public long Timer;
    }
}