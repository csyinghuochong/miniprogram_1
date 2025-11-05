using System.Collections.Generic;
using Unity.Mathematics;

namespace ET.Client
{
    [MessageHandler(SceneType.Demo)]
    public class M2C_UnitBuffUpdateHandler : MessageHandler<Scene, M2C_UnitBuffUpdate>
    {
        protected override async ETTask Run(Scene root, M2C_UnitBuffUpdate message)
        {
            Unit msgUnitBelongTo = root.CurrentScene()?.GetComponent<UnitComponent>().Get(message.UnitIdBelongTo);
            if (msgUnitBelongTo == null)
            {
                return;
            }

            switch (message.BuffOperateType)
            {
                case 1: //增加
                    BuffData buffData = new BuffData();
                    buffData.TargetAngle = 0;
                    buffData.BuffConfigId = message.BuffID;
                    buffData.Spellcaster = message.Spellcaster;
                    buffData.BuffEndTime = message.BuffEndTime;
                    buffData.UnitType = message.UnitType;
                    buffData.UnitConfigId = message.UnitConfigId;
                    buffData.SkillConfigId = message.SkillId;
                    buffData.UnitIdFrom = message.UnitIdFrom;
                    buffData.TargetPostion = new float3(message.TargetPostion[0], message.TargetPostion[1], message.TargetPostion[2]);
                    msgUnitBelongTo.GetComponent<BuffManagerComponentC>().BuffFactory(buffData);

                    EventSystem.Instance.Publish(root, new AddBuff() { Unit = msgUnitBelongTo, BuffId = message.BuffID });

                    break;
                case 2: //移除
                    msgUnitBelongTo.GetComponent<BuffManagerComponentC>().RemoveBuff(message.BuffID);

                    break;
                case 3: //重置
                    List<BuffC> buffList = msgUnitBelongTo.GetComponent<BuffManagerComponentC>().GetBuffByConfigId(message.BuffID);
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