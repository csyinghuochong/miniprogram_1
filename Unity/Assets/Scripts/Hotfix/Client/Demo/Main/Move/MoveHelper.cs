using System.Collections.Generic;
using Unity.Mathematics;

namespace ET.Client
{
    public static partial class MoveHelper
    {
        

        // 可以多次调用，多次调用的话会取消上一次的协程
        public static async ETTask<int> MoveToAsync(this Unit unit, float3 targetPos, ETCancellationToken cancellationToken = null, bool waitmove = false)
        {
            C2M_PathfindingRequest msg = C2M_PathfindingRequest.Create();
            msg.Position = targetPos;
            unit.Root().GetComponent<ClientSenderComponent>().Send(msg);
            MoveComponent moveComponent = unit.GetComponent<MoveComponent>();
            moveComponent.WaitMove = false;
            moveComponent.WaitMode = waitmove;
            ObjectWait objectWait = unit.GetComponent<ObjectWait>();

            // 要取消上一次的移动协程
            objectWait.Notify(new Wait_UnitStop() { Error = WaitTypeError.Cancel });

            // 一直等到unit发送stop
            Wait_UnitStop waitUnitStop = await objectWait.Wait<Wait_UnitStop>(cancellationToken);
            if (cancellationToken.IsCancel())
            {
                return WaitTypeError.Cancel;
            }

            return waitUnitStop.Error;
        }
        
        // 可以多次调用，多次调用的话会取消上一次的协程
        public static async ETTask<int> MoveResultToAsync(this Unit unit, List<float3> pathlist,  ETCancellationToken cancellationToken = null, int speedRate = 100, long serverTime = 0)
        {
            C2M_PathfindingResult msg = C2M_PathfindingResult.Create();
            msg.Position = pathlist;
            msg.SpeedRate = speedRate;
            msg.ServerTime = serverTime;
            unit.Root().GetComponent<ClientSenderComponent>().Send(msg);
            ObjectWait objectWait = unit.GetComponent<ObjectWait>();

            // 要取消上一次的移动协程
            objectWait.Notify(new Wait_UnitStop() { Error = WaitTypeError.Cancel });

            // 一直等到unit发送stop
            Wait_UnitStop waitUnitStop = await objectWait.Wait<Wait_UnitStop>(cancellationToken);
            if (cancellationToken.IsCancel())
            {
                return WaitTypeError.Cancel;
            }

            return waitUnitStop.Error;
        }

        
        public static void MoveToAsync(this Unit unit, List<float3> path)
        {
            float speed = unit.GetComponent<NumericComponentC>().GetAsFloat(NumericType.Now_MoveSpeed);
            // Move2DComponent moveComponent = unit.GetComponent<Move2DComponent>();
            // moveComponent.MoveTo(path, speed);
        }

        public static void Stop(Scene root)
        {
            C2M_Stop c2MStop = C2M_Stop.Create();
            root.GetComponent<ClientSenderComponent>().Send(c2MStop);
        }
        
        public static void StopResult(Scene root, float3 position)
        {
            C2M_StopResult c2MStop = C2M_StopResult.Create();
            c2MStop.Position = position;
            root.GetComponent<ClientSenderComponent>().Send(c2MStop);
        }
    }
}