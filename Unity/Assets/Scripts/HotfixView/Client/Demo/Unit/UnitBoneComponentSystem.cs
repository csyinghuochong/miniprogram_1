using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(UnitBoneComponent))]
    [FriendOf(typeof(UnitBoneComponent))]
    public static partial class UnitBoneComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UnitBoneComponent self)
        {
            Unit MyHero = self.GetParent<Unit>();
            Transform transform = MyHero.GetComponent<GameObjectComponent>().GameObject.transform;
            Transform boneSet = transform.Find("BoneSet");
            if (boneSet != null)
            {
                self.Hp = boneSet.Find("Hp");
            }
        }

        [EntitySystem]
        private static void Destroy(this UnitBoneComponent self)
        {
            self.Hp = null;
        }
    }
}