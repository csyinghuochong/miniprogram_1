using System.Collections.Generic;

namespace ET.Client
{
    [EntitySystemOf(typeof(BuffManagerComponent))]
    [FriendOf(typeof(BuffManagerComponent))]
    public static partial class BuffManagerComponentSystem
    {
        [EntitySystem]
        private static void Awake(this BuffManagerComponent self)
        {
        }

        [EntitySystem]
        private static void Update(this BuffManagerComponent self)
        {
            for (int i = self.Buffs.Count - 1; i >= 0; i--)
            {
                Buff buff = self.Buffs[i];

                if (buff.BuffState == BuffState.WaitRemove)
                {
                    // self.OnRemoveBuffItem(buff);
                    buff.BuffState = BuffState.Finished;
                }

                if (buff.BuffState == BuffState.Finished)
                {
                    buff.Dispose();
                    self.Buffs.RemoveAt(i);
                    continue;
                }

                buff.OnUpdate();
            }
        }

        [EntitySystem]
        private static void Destroy(this BuffManagerComponent self)
        {
            self.Buffs.Clear();
            self.Buffs = null;
        }

        public static void BuffFactory(this BuffManagerComponent self, BuffData buffData, Unit from, SkillC skillC)
        {
            Unit unit = self.GetParent<Unit>();
            BuffConfig newBuffConfig = BuffConfigCategory.Instance.Get(buffData.BuffConfigId);

            // 判断一些状态。。。

            int addBufStatus = 1; //1新增buff  2 移除 3 重置 4同状态返回

            // 移除互斥
            for (int i = self.Buffs.Count - 1; i >= 0; i--)
            {
                Buff buff = self.Buffs[i];
                bool remove = false;

                BuffConfig oldBuffConfig = buff.BuffConfig;
                if (oldBuffConfig.Id == newBuffConfig.Id && newBuffConfig.IsBuffStackable == 0)
                {
                    remove = true;
                }

                if (remove)
                {
                    buff.BuffState = BuffState.WaitRemove;
                }
            }

            if (addBufStatus == 4)
            {
                return;
            }

            // 添加Buff
            if (addBufStatus == 1)
            {
                Buff buff = self.AddChild<Buff>();
                self.Buffs.Add(buff);
                buff.OnInit(buffData, from, unit, skillC);
            }
        }
    }
}