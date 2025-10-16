namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class UserInfoComponentC : Entity, IAwake, IDestroy
    {
        public string PlayerName { get; set; }
        public long Gold { get; set; }
        public long Diamond { get; set; }
        public long Exp { get; set; }
    }
}