using Unity.Mathematics;

namespace ET.Server
{
    [EntitySystemOf(typeof(SkillS))]
    [FriendOf(typeof(SkillS))]
    public static partial class SkillSSystem
    {
        [EntitySystem]
        private static void Awake(this SkillS self)
        {
        }

        [EntitySystem]
        private static void Destroy(this SkillS self)
        {
        }

        public static void OnInit(this SkillS self, InitSkillData initSkillData, Unit theUnitFrom)
        {
            self.InitSkillData = initSkillData;
            self.SkillConfig = SkillConfigCategory.Instance.Get(initSkillData.SkillConfigId);
            self.SkillHandler = SkillDispatcherComponentS.Instance.Get(self.SkillConfig.SkillHandler);
            self.SkillState = SkillState.Running;
            self.TheUnitFrom = theUnitFrom;
            if (initSkillData.TargetId != 0)
            {
                self.TheUnitTarget = self.Scene().GetComponent<UnitComponent>().Get(initSkillData.TargetId);
            }

            self.TargetPosition = initSkillData.TargetPosition;

            self.SkillHandler?.OnInit(self);
        }

        public static void OnExecute(this SkillS self)
        {
            self.SkillHandler?.OnExecute(self);
        }

        public static void BaseUpdate(this SkillS self, float deltaTime)
        {
            self.RunTime += deltaTime;
            if (self.RunTime >= self.SkillConfig.SkillLiveTime)
            {
                self.SkillState = SkillState.Finished;
            }
        }

        public static void OnUpdate(this SkillS self, float deltaTime)
        {
            self.SkillHandler?.OnUpdate(self, deltaTime);
        }

        public static void OnFinished(this SkillS self)
        {
            self.SkillHandler?.OnFinished(self);
        }

        public static void InitSelfBuff(this SkillS self)
        {
            if (self.SkillConfig.InitBuffID != null && self.SkillConfig.InitBuffID[0] != 0)
            {
                for (int i = 0; i < self.SkillConfig.InitBuffID.Length; i++)
                {
                    self.SkillBuff(self.SkillConfig.InitBuffID[i], self.TheUnitFrom);
                }
            }
        }

        public static void SkillBuff(this SkillS self, int buffId, Unit uu)
        {
            if (uu == null)
            {
                return;
            }

            if (!BuffConfigCategory.Instance.DataMap.ContainsKey(buffId))
            {
                Log.Warning($"config==null： buffId{buffId}");
                return;
            }

            BuffConfig buffConfig = BuffConfigCategory.Instance.Get(buffId);

            //1：自身
            //2：队友
            //3: 敌方
            bool canBuff = false;
            switch (buffConfig.TargetType)
            {
                case 1:
                {
                    canBuff = uu.Id == self.TheUnitFrom.Id;

                    break;
                }
                case 2:
                {
                    canBuff = self.TheUnitFrom.IsTeam(uu);

                    break;
                }
                case 3:
                {
                    canBuff = self.TheUnitFrom.IsCanAttackUnit(uu);

                    break;
                }
            }

            if (!canBuff)
            {
                return;
            }

            InitBuffData initBuffData = new InitBuffData();
            initBuffData.SkillConfigId = self.SkillConfig.Id;
            initBuffData.BuffConfigId = buffConfig.Id;
            uu.GetComponent<BuffManagerComponentS>().BuffFactory(initBuffData, self.TheUnitFrom, self);
        }

        public static Shape CreateCheckShape(this SkillS self, int targetAngle)
        {
            Shape ishape = null;

            switch (self.SkillConfig.DamageRangeType)
            {
                case 0:
                    break;
                case 1:
                    ishape = new Circle();
                    (ishape as Circle).s_position = self.TargetPosition;
                    (ishape as Circle).range = self.SkillConfig.DamageRange[0];
                    break;
                case 2:
                    ishape = new Rectangle();
                    (ishape as Rectangle).s_position = self.TargetPosition;
                    quaternion forward = quaternion.Euler(0, math.radians(targetAngle), 0);
                    (ishape as Rectangle).s_forward = math.mul(forward, math.forward());
                    (ishape as Rectangle).x_range = self.SkillConfig.DamageRange[0] * 0.5f;
                    (ishape as Rectangle).y_range = self.SkillConfig.DamageRange[1];
                    break;
                case 3:
                    ishape = new Fan();
                    (ishape as Fan).s_position = self.TargetPosition;
                    (ishape as Fan).s_rotation = quaternion.Euler(0, math.radians(targetAngle), 0);
                    (ishape as Fan).skill_distance = self.SkillConfig.DamageRange[0];
                    (ishape as Fan).skill_angle = self.SkillConfig.DamageRange[1];
                    break;
            }

            return ishape;
        }
    }
}