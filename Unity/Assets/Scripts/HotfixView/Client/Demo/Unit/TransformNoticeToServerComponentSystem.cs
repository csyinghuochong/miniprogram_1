namespace ET.Client
{
    [EntitySystemOf(typeof(TransformNoticeToServerComponent))]
    [FriendOf(typeof(TransformNoticeToServerComponent))]
    public static partial class TransformNoticeToServerComponentSystem
    {
        [Invoke(TimerInvokeType.TransformSyncToServer)]
        [FriendOf(typeof(TransformNoticeToServerComponent))]
        public class TransformSyncToClient : ATimer<TransformNoticeToServerComponent>
        {
            protected override void Run(TransformNoticeToServerComponent self)
            {
                C2M_NoticeUnitTransform message = C2M_NoticeUnitTransform.Create(true);

                if (self.MyUnit.Position.Equals(self.Position))
                {
                    return;
                }

                message.Position = self.MyUnit.Position;
                self.Position = message.Position;

                self.Root().GetComponent<ClientSenderComponent>().Send(message);
            }
        }

        [EntitySystem]
        private static void Awake(this TransformNoticeToServerComponent self)
        {
            self.MyUnit = self.GetParent<Unit>();
            self.Timer = self.Root().GetComponent<TimerComponent>().NewRepeatedTimer(ConfigData.TransformSyncTime, TimerInvokeType.TransformSyncToServer, self);
        }

        [EntitySystem]
        private static void Destroy(this TransformNoticeToServerComponent self)
        {
            self.Root().GetComponent<TimerComponent>().Remove(ref self.Timer);
        }
    }
}