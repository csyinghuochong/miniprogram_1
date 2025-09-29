using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using System.ComponentModel;

namespace ET
{
    [Config]
    public partial class SkillBuffConfigCategory : Singleton<SkillBuffConfigCategory>, IMerge
    {
        [BsonElement]
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        private Dictionary<int, SkillBuffConfig> dict = new();
		
        public void Merge(object o)
        {
            SkillBuffConfigCategory s = o as SkillBuffConfigCategory;
            foreach (var kv in s.dict)
            {
                this.dict.Add(kv.Key, kv.Value);
            }
        }
		
        public SkillBuffConfig Get(int id)
        {
            this.dict.TryGetValue(id, out SkillBuffConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (SkillBuffConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, SkillBuffConfig> GetAll()
        {
            return this.dict;
        }

        public SkillBuffConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            
            var enumerator = this.dict.Values.GetEnumerator();
            enumerator.MoveNext();
            return enumerator.Current; 
        }
    }

	public partial class SkillBuffConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		public int Id { get; set; }
		/// <summary>状态名称</summary>
		public string BuffName { get; set; }
		/// <summary>Buff存在时间</summary>
		public int BuffTime { get; set; }
		/// <summary>Buff延迟生效时间</summary>
		public int BuffDelayTime { get; set; }
		/// <summary>Buff目标类型</summary>
		public int TargetType { get; set; }
		/// <summary>Buff增益减益</summary>
		public int BuffBenefitType { get; set; }
		/// <summary>Buff类型</summary>
		public int BuffType { get; set; }
		/// <summary>Buff参数操作类型</summary>
		public int BuffParameterType { get; set; }
		/// <summary>Buff参数操作值</summary>
		public double BuffParameterValue { get; set; }
		/// <summary>Buff特效ID</summary>
		public int BuffEffectID { get; set; }
		/// <summary>Buff描述</summary>
		public string BuffDescribe { get; set; }

	}
}
