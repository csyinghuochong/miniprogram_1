using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using System.ComponentModel;

namespace ET
{
    [Config]
    public partial class HeroConfigCategory : Singleton<HeroConfigCategory>, IMerge
    {
        [BsonElement]
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        private Dictionary<int, HeroConfig> dict = new();
		
        public void Merge(object o)
        {
            HeroConfigCategory s = o as HeroConfigCategory;
            foreach (var kv in s.dict)
            {
                this.dict.Add(kv.Key, kv.Value);
            }
        }
		
        public HeroConfig Get(int id)
        {
            this.dict.TryGetValue(id, out HeroConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (HeroConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, HeroConfig> GetAll()
        {
            return this.dict;
        }

        public HeroConfig GetOne()
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

	public partial class HeroConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		public int Id { get; set; }
		/// <summary>英雄名称</summary>
		public string HeroName { get; set; }
		/// <summary>英雄类型</summary>
		public int HeroType { get; set; }
		/// <summary>英雄头像</summary>
		public string HeroHeadIcon { get; set; }
		/// <summary>英雄模型ID</summary>
		public int HeroModelID { get; set; }
		/// <summary>英雄品质</summary>
		public int HeroQuality { get; set; }
		/// <summary>攻击距离</summary>
		public int AtkDistance { get; set; }
		/// <summary>普通攻击ID</summary>
		public int AtkID { get; set; }
		/// <summary>英雄技能ID</summary>
		public int[] SkillID { get; set; }
		/// <summary>初始攻击</summary>
		public int Act { get; set; }
		/// <summary>初始防御</summary>
		public int Def { get; set; }
		/// <summary>初始生命</summary>
		public int Hp { get; set; }
		/// <summary>初始攻速</summary>
		public double AtkSpeed { get; set; }
		/// <summary>初始移速</summary>
		public double MoveSpeed { get; set; }
		/// <summary>初始暴击</summary>
		public double Cri { get; set; }
		/// <summary>初始连击</summary>
		public double Combo { get; set; }
		/// <summary>初始反击</summary>
		public double Counterattack { get; set; }
		/// <summary>初始吸血</summary>
		public double LifeSteal { get; set; }
		/// <summary>初始闪避</summary>
		public double Eva { get; set; }
		/// <summary>初始抗暴击</summary>
		public double ReCri { get; set; }
		/// <summary>初始抗连击</summary>
		public double ReCombo { get; set; }
		/// <summary>初始抗反击</summary>
		public double ReCounterattack { get; set; }
		/// <summary>初始抗吸血</summary>
		public double ReLifeSteal { get; set; }
		/// <summary>初始抗闪避</summary>
		public double ReEva { get; set; }
		/// <summary>英雄简介</summary>
		public string HeroDescription { get; set; }

	}
}
