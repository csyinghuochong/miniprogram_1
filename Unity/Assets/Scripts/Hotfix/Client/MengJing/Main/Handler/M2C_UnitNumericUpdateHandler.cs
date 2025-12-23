using System;

namespace ET.Client
{
    //接受属性改变消息
    [MessageHandler(SceneType.Demo)]
    public class M2C_UnitNumericUpdateHandler : MessageHandler<Scene, M2C_UnitNumericUpdate>
    {
        protected override async ETTask Run(Scene root, M2C_UnitNumericUpdate message)
        {
            using var _ = message;
            
            Scene currentScene = root.CurrentScene();
            Unit unit = currentScene.GetComponent<UnitComponent>()?.Get(message.UnitId);
            if (unit == null)
            {
                return;
            }

            //客户端的NumericComponent.Set不会抛出事件。需要自己手动抛出
            NumericComponentC numericComponent = unit.GetComponent<NumericComponentC>();
            numericComponent.ApplyValue(message.AttackId, message.NumericType, message.NewValue, message.SkillId, true, (DamageType)message.DamgeType);
            await ETTask.CompletedTask;
        }
    }
}