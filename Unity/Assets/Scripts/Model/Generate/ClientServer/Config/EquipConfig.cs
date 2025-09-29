using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using System.ComponentModel;

namespace ET
{
    [Config]
    public partial class EquipConfigCategory : Singleton<EquipConfigCategory>, IMerge
    {
        [BsonElement]
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        private Dictionary<int, EquipConfig> dict = new();
		
        public void Merge(object o)
        {
            EquipConfigCategory s = o as EquipConfigCategory;
            foreach (var kv in s.dict)
            {
                this.dict.Add(kv.Key, kv.Value);
            }
        }
		
        public EquipConfig Get(int id)
        {
            this.dict.TryGetValue(id, out EquipConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (EquipConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, EquipConfig> GetAll()
        {
            return this.dict;
        }

        public EquipConfig GetOne()
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

	public partial class EquipConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		public int Id { get; set; }
		/// <summary>装备名称</summary>
		public string ItemName { get; set; }
		/// <summary>隐藏属性类型</summary>
		public int[] HideType { get; set; }
		/// <summary>单条隐藏属性出现概率</summary>
		public double HideShowPro { get; set; }
		/// <summary>分解资源类型</summary>
		public int[] SellResourcesType { get; set; }
		/// <summary>分解资源值</summary>
		public int[] SellResourcesValue { get; set; }
		/// <summary>最低攻击</summary>
		public int Equip_MinAct { get; set; }
		/// <summary>最高攻击</summary>
		public int Equip_MaxAct { get; set; }
		/// <summary>最低防御</summary>
		public int Equip_MinDef { get; set; }
		/// <summary>最高防御</summary>
		public int Equip_MaxAdf { get; set; }
		/// <summary>最低生命</summary>
		public int Equip_MinHp { get; set; }
		/// <summary>最高生命</summary>
		public int Equip_MaxHp { get; set; }
		/// <summary>最低攻速</summary>
		public double Equip_MinAtkSpeed { get; set; }
		/// <summary>最高攻速</summary>
		public double Equip_MaxAtkSpeed { get; set; }
		/// <summary>最低移速</summary>
		public double Equip_MinMoveSpeed { get; set; }
		/// <summary>最高攻速</summary>
		public double Equip_MaxMoveSpeed { get; set; }
		/// <summary>暴击</summary>
		public double Equip_Cri { get; set; }
		/// <summary>连击</summary>
		public double Equip_Combo { get; set; }
		/// <summary>反击</summary>
		public double Equip_Counterattack { get; set; }
		/// <summary>吸血</summary>
		public double Equip_LifeSteal { get; set; }
		/// <summary>闪避</summary>
		public double Equip_Eva { get; set; }
		/// <summary>抗暴击</summary>
		public double Equip_ReCri { get; set; }
		/// <summary>抗连击</summary>
		public double Equip_ReCombo { get; set; }
		/// <summary>抗反击</summary>
		public double Equip_ReCounterattack { get; set; }
		/// <summary>抗吸血</summary>
		public double Equip_ReLifeSteal { get; set; }
		/// <summary>抗闪避</summary>
		public double Equip_ReEva { get; set; }

	}
}
