using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIMainCityMapNPCButton))]
    [FriendOf(typeof(UIMainCityMapNPCButton))]
    public static partial class UIMainCityMapNPCButtonSystem
    {
        [EntitySystem]
        private static void Awake(this UIMainCityMapNPCButton self, GameObject gameObject)
        {
            ReferenceCollector rc = gameObject.GetComponent<ReferenceCollector>();

            self.GameObject = gameObject;

            self.Button_GoToNPC = rc.Get<GameObject>("Button_GoToNPC").GetComponent<Button>();
            self.Text_NPCName = rc.Get<GameObject>("Text_NPCName").GetComponent<TMP_Text>();

            self.Button_GoToNPC.AddListener(self.OnButton_GoToNPC);
        }

        public static void UpdateInfo(this UIMainCityMapNPCButton self, NPCConfig npcConfig)
        {
            self.NpcId = npcConfig.Id;
            self.Text_NPCName.SetText(npcConfig.Name);
        }

        private static void OnButton_GoToNPC(this UIMainCityMapNPCButton self)
        {
            NPCConfig npcConfig = NPCConfigCategory.Instance.Get(self.NpcId);

            Unit unit = UnitHelper.GetMyUnitFromClientScene(self.Root());
            if (unit == null)
            {
                return;
            }

            MoveHelper.MoveTo(unit, new float2(npcConfig.Position.X, npcConfig.Position.Y));
        }
    }
}