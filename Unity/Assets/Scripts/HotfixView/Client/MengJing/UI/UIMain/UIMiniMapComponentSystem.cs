using TMPro;
using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIMiniMapComponent))]
    [FriendOf(typeof(UIMiniMapComponent))]
    public static partial class UIMiniMapComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UIMiniMapComponent self, GameObject gameObject)
        {
            self.GameObject = gameObject;
            ReferenceCollector rc = gameObject.GetComponent<ReferenceCollector>();

            self.Text_MiniMapName = rc.Get<GameObject>("Text_MiniMapName").GetComponent<TMP_Text>();
        }

        [EntitySystem]
        private static void Destroy(this UIMiniMapComponent self)
        {
        }

        public static void AfterEnterScene(this UIMiniMapComponent self, MapType mapType)
        {
            if (mapType == MapType.MainCity)
            {
                self.Text_MiniMapName.SetText("主城");
            }

            if (mapType == MapType.LocalLevel)
            {
                Unit unit = UnitHelper.GetMyUnitFromClientScene(self.Root());
                NumericComponentC numericComponent = unit.GetComponent<NumericComponentC>();
                int currentLevelId = numericComponent.GetAsInt(NumericType.CurrentLevelId);
                if (!LevelConfigCategory.Instance.DataMap.ContainsKey(currentLevelId))
                {
                    return;
                }

                LevelConfig levelConfig = LevelConfigCategory.Instance.Get(currentLevelId);
                
                self.Text_MiniMapName.SetText(levelConfig.LevelName);
            }
        }
    }
}