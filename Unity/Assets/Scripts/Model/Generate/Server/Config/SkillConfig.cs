using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using System.ComponentModel;

namespace ET
{
    [Config]
    public partial class SkillConfigCategory : Singleton<SkillConfigCategory>, IMerge
    {
        [BsonElement]
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        private Dictionary<int, SkillConfig> dict = new();
		
        public void Merge(object o)
        {
            SkillConfigCategory s = o as SkillConfigCategory;
            foreach (var kv in s.dict)
            {
                this.dict.Add(kv.Key, kv.Value);
            }
        }
		
        public SkillConfig Get(int id)
        {
            this.dict.TryGetValue(id, out SkillConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (SkillConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, SkillConfig> GetAll()
        {
            return this.dict;
        }

        public SkillConfig GetOne()
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

	public partial class SkillConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		public int Id { get; set; }
		/// <summary>技能名称</summary>
		public string SkillName { get; set; }
		/// <summary>技能Icon</summary>
		public string SkillIcon { get; set; }
		/// <summary>技能攻击类型</summary>
		public int SkillActType { get; set; }
		/// <summary>脚本名称</summary>
		public string SkillHandler { get; set; }
		/// <summary>技能CD</summary>
		public double SkillCD { get; set; }
		/// <summary>技能存在时间[毫秒]</summary>
		public int SkillLiveTime { get; set; }
		/// <summary>固定伤害值</summary>
		public int DamgeValue { get; set; }
		/// <summary>施法动作名称</summary>
		public string SkillAnimation { get; set; }
		/// <summary>技能特效Id</summary>
		public int SkillHitEffectID { get; set; }
		/// <summary>释放BUFFID</summary>
		public int[] BuffID { get; set; }
		/// <summary>技能音效</summary>
		public string SkillMusic { get; set; }
		/// <summary>技能描述</summary>
		public string SkillDescribe { get; set; }

	}
}
