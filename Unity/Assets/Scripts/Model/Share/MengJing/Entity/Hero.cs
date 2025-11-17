using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;

namespace ET
{
    public enum HeroOpType
    {
        Add,
        Remove,
        Update,
    }

    public enum HeroType
    {
        Melee = 1, //近战
        Ranged = 2, //远程
    }

    [ChildOf]
    public class Hero : Entity, IAwake, IDestroy, ISerializeToEntity
    {
        public int ConfigId { get; set; }
        public int Lv { get; set; }
        public int Exp { get; set; }
        public int Star { get; set; }
        public int HunShi { get; set; }

        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, long> Equipments { get; set; } = new();

        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, long> NumericDic { get; set; } = new();

        public List<int> Skills { get; set; } = new();
    }
}