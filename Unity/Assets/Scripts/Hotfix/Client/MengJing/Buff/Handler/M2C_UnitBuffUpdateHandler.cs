using System.Collections.Generic;
using Unity.Mathematics;

namespace ET.Client
{
    [MessageHandler(SceneType.Demo)]
    public class M2C_UnitBuffUpdateHandler : MessageHandler<Scene, M2C_UnitBuffUpdate>
    {
        protected override async ETTask Run(Scene root, M2C_UnitBuffUpdate message)
        {
            using var _ = message;
            
            Unit unit = root.CurrentScene()?.GetComponent<UnitComponent>().Get(message.UnitId);
            if (unit == null)
            {
                return;
            }

            switch (message.BuffOperateType)
            {
                case 1: //增加
                    unit.GetComponent<BuffManagerComponentC>().BuffFactory(message);

                    EventSystem.Instance.Publish(root, new AddBuff() { Unit = unit, BuffId = message.BuffId });

                    break;
                case 2: //移除
                    unit.GetComponent<BuffManagerComponentC>().RemoveBuff(message.BuffId);

                    break;
                case 3: //重置
                    List<BuffC> buffList = unit.GetComponent<BuffManagerComponentC>().GetBuffByConfigId(message.BuffConfigId);
                    for (int i = 0; i < buffList.Count; i++)
                    {
                        buffList[i].OnReset(message.BuffEndTime);
                    }

                    break;
            }

            await ETTask.CompletedTask;
        }
    }
}