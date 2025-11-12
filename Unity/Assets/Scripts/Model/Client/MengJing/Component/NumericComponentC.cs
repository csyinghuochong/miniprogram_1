using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;

namespace ET.Client
{
    [FriendOf(typeof(NumericComponentC))]
    public static class NumericComponentCSystem
    {
        public static float GetAsFloat(this NumericComponentC self, int numericType)
        {
            return (float)self.GetByKey(numericType) / 10000;
        }

        public static int GetAsInt(this NumericComponentC self, int numericType)
        {
            return (int)self.GetByKey(numericType);
        }

        public static long GetAsLong(this NumericComponentC self, int numericType)
        {
            return self.GetByKey(numericType);
        }

        public static long GetByKey(this NumericComponentC self, int key)
        {
            long value = 0;
            self.NumericDic.TryGetValue(key, out value);
            return value;
        }

        public static void ApplyValue(this NumericComponentC self, int numericType, long value, bool notice = true, bool check = true,
        long attackId = 0, int skillId = 0, DamageType damageType = 0)
        {
            long old = self.GetByKey(numericType);

            self.NumericDic[numericType] = value;
            
            if (notice)
            {
                //发送改变属性的相关消息
                NumbericChange args = new NumbericChange();
                args.Defend = self.Parent as Unit;
                args.AttackId = attackId;
                args.NumericType = numericType;
                args.OldValue = old;
                args.NewValue = self.NumericDic[numericType];
                args.SkillId = skillId;
                args.DamageType = damageType;
                EventSystem.Instance.Publish(self.Scene(), args);
            }
        }
        
        public static void ApplyChange(this NumericComponentC self, int numericType, long value, bool notice = true, bool check = true,
        long attackId = 0, int skillId = 0, DamageType damageType = 0)
        {
            long old = self.GetByKey(numericType);

            self.NumericDic[numericType] = self.NumericDic[numericType] + value;
            
            if (notice)
            {
                //发送改变属性的相关消息
                NumbericChange args = new NumbericChange();
                args.Defend = self.Parent as Unit;
                args.AttackId = attackId;
                args.NumericType = numericType;
                args.OldValue = old;
                args.NewValue = self.NumericDic[numericType];
                args.SkillId = skillId;
                args.DamageType = damageType;
                EventSystem.Instance.Publish(self.Scene(), args);
            }
        }

        /// <summary>
        /// 传入改变值,设置当前的属性值, 不走公式
        /// </summary>
        /// <param name="self"></param>
        /// <param name="attackId"></param>
        /// <param name="numericType"></param>
        /// <param name="value"></param>
        /// <param name="skillID"></param>
        /// <param name="notice"></param>
        /// <param name="damageType"></param>
        public static void ApplyValue(this NumericComponentC self, long attackId, int numericType, long value, int skillID, bool notice = true,
        DamageType damageType = 0)
        {
            //是否超过指定上限值
            long old = self.GetByKey(numericType);
            self.NumericDic[numericType] = value;
            
            if (notice)
            {
                //发送改变属性的相关消息
                NumbericChange args = new NumbericChange();
                args.Defend = self.Parent as Unit;
                args.AttackId = attackId;
                args.NumericType = numericType;
                args.OldValue = old;
                args.NewValue = self.NumericDic[numericType];
                args.SkillId = skillID;
                args.DamageType = damageType;
                EventSystem.Instance.Publish(self.Scene(), args);
            }
        }
        
        //重置所有属性
        public static void ResetProperty(this NumericComponentC self)
        {
            long max = NumericType.Max;

            foreach (int key in self.NumericDic.Keys)
            {
                //这个范围内的属性为特殊属性不进行重置
                if (key >= NumericType.Now_Hp && key < max)
                {
                    continue;
                }

                //buff属性不进行重置
                int yushu = key % 100;
                if (yushu == 11 || yushu == 12)
                {
                    continue;
                }

                self.NumericDic[key] = 0;
            }
        }
    }

    [ComponentOf(typeof(Unit))]
    public class NumericComponentC : Entity, IAwake, ITransfer
    {
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, long> NumericDic = new();
    }
}