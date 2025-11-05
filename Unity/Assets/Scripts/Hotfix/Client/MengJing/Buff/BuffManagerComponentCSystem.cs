using System.Collections.Generic;

namespace ET.Client
{
    [EntitySystemOf(typeof(BuffManagerComponentC))]
    [FriendOf(typeof(BuffManagerComponentC))]
    public static partial class BuffManagerComponentCSystem
    {
        [EntitySystem]
        private static void Awake(this BuffManagerComponentC self)
        {
        }

        [EntitySystem]
        private static void Update(this BuffManagerComponentC self)
        {
            for (int i = self.Buffs.Count - 1; i >= 0; i--)
            {
                BuffC buffC = self.Buffs[i];

                if (buffC.BuffState == BuffState.WaitRemove)
                {
                    // self.OnRemoveBuffItem(buff);
                    buffC.BuffState = BuffState.Finished;
                }

                if (buffC.BuffState == BuffState.Finished)
                {
                    buffC.Dispose();
                    self.Buffs.RemoveAt(i);
                    continue;
                }

                buffC.OnUpdate();
            }
        }

        [EntitySystem]
        private static void Destroy(this BuffManagerComponentC self)
        {
            self.Buffs.Clear();
            self.Buffs = null;
        }

        public static void BuffFactory(this BuffManagerComponentC self, BuffData buffData, Unit from, SkillC skillC)
        {
            Unit unit = self.GetParent<Unit>();
            BuffConfig newBuffConfig = BuffConfigCategory.Instance.Get(buffData.BuffConfigId);

            // 判断一些状态。。。

            int addBufStatus = 1; //1新增buff  2 移除 3 重置 4同状态返回

            // 移除互斥
            for (int i = self.Buffs.Count - 1; i >= 0; i--)
            {
                BuffC buffC = self.Buffs[i];
                bool remove = false;

                BuffConfig oldBuffConfig = buffC.BuffConfig;
                if (oldBuffConfig.Id == newBuffConfig.Id && newBuffConfig.IsBuffStackable == 0)
                {
                    remove = true;
                }

                if (remove)
                {
                    buffC.BuffState = BuffState.WaitRemove;
                }
            }

            if (addBufStatus == 4)
            {
                return;
            }

            // 添加Buff
            if (addBufStatus == 1)
            {
                BuffC buffC = self.AddChild<BuffC>();
                self.Buffs.Add(buffC);
                buffC.OnInit(buffData, from, unit, skillC);
            }
        }
    }
}