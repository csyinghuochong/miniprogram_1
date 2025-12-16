using Unity.Mathematics;

namespace ET.Server
{
    [EntitySystemOf(typeof(TransformNoticeToClientComponent))]
    [FriendOf(typeof(TransformNoticeToClientComponent))]
    public static partial class TransformNoticeToClientComponentSystem
    {
        [Invoke(TimerInvokeType.TransformSyncToClient)]
        [FriendOf(typeof(TransformNoticeToClientComponent))]
        public class TransformSyncToClient : ATimer<TransformNoticeToClientComponent>
        {
            protected override void Run(TransformNoticeToClientComponent self)
            {
                M2C_NoticeUnitTransformList message = M2C_NoticeUnitTransformList.Create(true);

                foreach (AOIEntity aOIEntity in self.AOIEntity.GetSeeUnits().Values)
                {
                    Unit unit = aOIEntity.Unit;

                    bool notice = false;
                    if (!self.UnitPositions.TryGetValue(unit.Id, out float3 position))
                    {
                        self.UnitPositions.Add(unit.Id, unit.Position);
                        notice = true;
                    }
                    else
                    {
                        bool areSamePosition = position.Equals(unit.Position);
                        if (!areSamePosition)
                        {
                            notice = true;
                        }
                    }

                    self.UnitPositions[unit.Id] = unit.Position;

                    if (!notice)
                    {
                        continue;
                    }

                    message.UnitIdList.Add(unit.Id);
                    message.PositionList.Add(unit.Position);
                }

                if (message.UnitIdList.Count == 0)
                {
                    message.Dispose();
                    return;
                }

                MapMessageHelper.SendToClient(self.GetParent<Unit>(), message);
            }
        }

        [EntitySystem]
        private static void Awake(this TransformNoticeToClientComponent self)
        {
            self.AOIEntity = self.GetParent<Unit>().GetComponent<AOIEntity>();
            self.Timer = self.Root().GetComponent<TimerComponent>().NewRepeatedTimer(ConfigData.TransformSyncTime, TimerInvokeType.TransformSyncToClient, self);
        }

        [EntitySystem]
        private static void Destroy(this TransformNoticeToClientComponent self)
        {
            self.Root().GetComponent<TimerComponent>().Remove(ref self.Timer);
            self.UnitPositions.Clear();
        }
    }
}