using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using System.ComponentModel;

namespace ET
{
    [Config]
    public partial class MonsterConfigCategory : Singleton<MonsterConfigCategory>, IMerge
    {
        [BsonElement]
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        private Dictionary<int, MonsterConfig> dict = new();
		
        public void Merge(object o)
        {
            MonsterConfigCategory s = o as MonsterConfigCategory;
            foreach (var kv in s.dict)
            {
                this.dict.Add(kv.Key, kv.Value);
            }
        }
		
        public MonsterConfig Get(int id)
        {
            this.dict.TryGetValue(id, out MonsterConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (MonsterConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, MonsterConfig> GetAll()
        {
            return this.dict;
        }

        public MonsterConfig GetOne()
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

	public partial class MonsterConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		public int Id { get; set; }
		/// <summary>怪物名称</summary>
		public string MonsterName { get; set; }
		/// <summary>怪物头像</summary>
		public string MonsterHeadIcon { get; set; }
		/// <summary>怪物类型</summary>
		public int MonsterType { get; set; }
		/// <summary>怪物模型ID</summary>
		public int MonsterModelID { get; set; }
		/// <summary>怪物挑战时间[秒]</summary>
		public int ChallengeTime { get; set; }
		/// <summary>攻击距离</summary>
		public int ActDistance { get; set; }
		/// <summary>普通攻击ID</summary>
		public int ActSkillID { get; set; }
		/// <summary>怪物技能ID</summary>
		public int[] SkillID { get; set; }
		/// <summary>攻击</summary>
		public int Act { get; set; }
		/// <summary>防御</summary>
		public int Def { get; set; }
		/// <summary>生命</summary>
		public int Hp { get; set; }
		/// <summary>攻速</summary>
		public double AtkSpeed { get; set; }
		/// <summary>移速</summary>
		public double MoveSpeed { get; set; }
		/// <summary>暴击概率</summary>
		public double Cri { get; set; }
		/// <summary>怪物简介</summary>
		public string MonsterDescription { get; set; }

	}
}
