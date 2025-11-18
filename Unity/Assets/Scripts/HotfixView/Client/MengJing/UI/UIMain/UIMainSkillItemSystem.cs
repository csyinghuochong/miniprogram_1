using Cysharp.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIMainSkillItem))]
    [FriendOf(typeof(UIMainSkillItem))]
    public static partial class UIMainSkillItemSystem
    {
        [EntitySystem]
        private static void Awake(this UIMainSkillItem self, GameObject gameObject)
        {
            self.GameObject = gameObject;

            ReferenceCollector rc = gameObject.GetComponent<ReferenceCollector>();
            self.Image_SkillIcon = rc.Get<GameObject>("Image_SkillIcon").GetComponent<Image>();
            self.Image_SkillCd = rc.Get<GameObject>("Image_SkillCd").GetComponent<Image>();
            self.Text_SkillCd = rc.Get<GameObject>("Text_SkillCd").GetComponent<TMP_Text>();
            self.EventTrigger_Click = rc.Get<GameObject>("EventTrigger_Click").GetComponent<EventTrigger>();
        }

        [EntitySystem]
        private static void Destroy(this UIMainSkillItem self)
        {
        }

        public static void Update(this UIMainSkillItem self)
        {
            if (!self.GameObject.activeSelf)
            {
                return;
            }

            Unit unit = self.Root().CurrentScene().GetComponent<UnitComponent>().Get(self.UnitId);
            if (unit == null)
            {
                return;
            }

            SkillConfig skillConfig = SkillConfigCategory.Instance.Get(self.SkillId);

            SkillManagerComponentC skillManagerComponent = unit.GetComponent<SkillManagerComponentC>();
            float cd = skillManagerComponent.GetSkillCD(self.SkillId);

            if (cd <= 0)
            {
                self.Image_SkillCd.gameObject.SetActive(false);
                self.Text_SkillCd.SetText("");
            }
            else
            {
                self.Image_SkillCd.gameObject.SetActive(true);
                self.Image_SkillCd.fillAmount = cd / skillConfig.SkillCD;
                self.Text_SkillCd.SetTextFormat("{0:0.#}", cd);
            }
        }

        public static async ETTask UpdateInfo(this UIMainSkillItem self, long unitId, int skillId)
        {
            self.UnitId = unitId;
            self.SkillId = skillId;

            SkillConfig skillConfig = SkillConfigCategory.Instance.Get(skillId);

            string path = ABPathHelper.GetAtlasPath_2(ABAtlasTypes.SkillIcon, skillConfig.SkillIcon);
            self.Image_SkillIcon.overrideSprite = await self.Root().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<Sprite>(path);
        }
    }
}