using Cysharp.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UISkillItem))]
    [FriendOf(typeof(UISkillItem))]
    public static partial class UISkillItemSystem
    {
        [EntitySystem]
        private static void Awake(this UISkillItem self, GameObject gameObject)
        {
            self.GameObject = gameObject;

            ReferenceCollector rc = gameObject.GetComponent<ReferenceCollector>();

            self.Image_SkillIcon = rc.Get<GameObject>("Image_SkillIcon").GetComponent<Image>();
            self.Button_Click = rc.Get<GameObject>("Button_Click").GetComponent<Button>();
            self.Unlock = rc.Get<GameObject>("Unlock");
            self.Text_Unlock = rc.Get<GameObject>("Text_Unlock").GetComponent<TMP_Text>();

            self.Button_Click.AddListener(() => { self.OnButton_Click().Coroutine(); });
        }

        [EntitySystem]
        private static void Destroy(this UISkillItem self)
        {
        }

        public static async ETTask UpdateInfo(this UISkillItem self, int skillConfigId, int heroStar)
        {
            self.SkillConfigId = skillConfigId;

            SkillConfig skillConfig = SkillConfigCategory.Instance.Get(skillConfigId);

            string path = ABPathHelper.GetAtlasPath_2(ABAtlasTypes.SkillIcon, skillConfig.SkillIcon);
            self.Image_SkillIcon.overrideSprite = await self.Root().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<Sprite>(path);

            if (heroStar < skillConfig.UnlockStar)
            {
                self.Unlock.SetActive(true);
                self.Text_Unlock.SetTextFormat("{0}星激活", skillConfig.UnlockStar);
            }
            else
            {
                self.Unlock.SetActive(false);
            }
        }

        private static async ETTask OnButton_Click(this UISkillItem self)
        {
            UI ui = await self.Root().GetComponent<UIComponent>().Create(UIType.UISkillTip);
            UISkillTipComponent uiSkillTipComponent = ui.GetComponent<UISkillTipComponent>();
            uiSkillTipComponent.UpdateInfo(self.SkillConfigId).Coroutine();
        }
    }
}