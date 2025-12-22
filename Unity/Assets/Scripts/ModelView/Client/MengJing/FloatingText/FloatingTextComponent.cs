using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class FloatingTextComponent : Entity, IAwake, IUpdate, IDestroy
    {
        public const string PhysicalDamageText = "Assets/Bundles/UI/Blood/Text_PhysicalDamage.prefab";
        public const string MagicDamageText = "Assets/Bundles/UI/Blood/Text_MagicDamage.prefab";
        public const string CriDamageText = "Assets/Bundles/UI/Blood/Text_CriDamage.prefab";
        public const string RecoverText = "Assets/Bundles/UI/Blood/Text_Recover.prefab";
        public const string NormalText = "Assets/Bundles/UI/Blood/Text_Normal.prefab";
        public const string TipText = "Assets/Bundles/UI/Blood/Text_Tip.prefab";
        
        public List<EntityRef<FloatingText>> FloatingTexts = new();
    }
}