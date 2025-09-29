using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using System.ComponentModel;

namespace ET
{
    [Config]
    public partial class ItemConfigCategory : Singleton<ItemConfigCategory>, IMerge
    {
        [BsonElement]
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        private Dictionary<int, ItemConfig> dict = new();
		
        public void Merge(object o)
        {
            ItemConfigCategory s = o as ItemConfigCategory;
            foreach (var kv in s.dict)
            {
                this.dict.Add(kv.Key, kv.Value);
            }
        }
		
        public ItemConfig Get(int id)
        {
            this.dict.TryGetValue(id, out ItemConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (ItemConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, ItemConfig> GetAll()
        {
            return this.dict;
        }

        public ItemConfig GetOne()
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

	public partial class ItemConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		public int Id { get; set; }
		/// <summary>道具名称</summary>
		public string ItemName { get; set; }
		/// <summary>道具Icon</summary>
		public string Icon { get; set; }
		/// <summary>道具品质</summary>
		public int ItemQuality { get; set; }
		/// <summary>道具类型</summary>
		public int ItemType { get; set; }
		/// <summary>道具子类</summary>
		public int ItemSubType { get; set; }
		/// <summary>道具堆叠最大数量</summary>
		public int ItemPileSum { get; set; }
		/// <summary>装备ID</summary>
		public int ItemEquipID { get; set; }
		/// <summary>道具描述</summary>
		public string ItemDescription { get; set; }

	}
}
