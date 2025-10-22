using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UISkillTipComponent))]
    [FriendOf(typeof(UISkillTipComponent))]
    public static partial class UISkillTipComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UISkillTipComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.Button_Close = rc.Get<GameObject>("Button_Close").GetComponent<Button>();
            self.Image_SkillIcon = rc.Get<GameObject>("Image_SkillIcon").GetComponent<Image>();
            self.Text_SkillName = rc.Get<GameObject>("Text_SkillName").GetComponent<TMP_Text>();
            self.Text_SkillDes = rc.Get<GameObject>("Text_SkillDes").GetComponent<TMP_Text>();

            self.Button_Close.AddListener(() => { self.Root().GetComponent<UIComponent>().Remove(UIType.UISkillTip); });
        }

        public static async ETTask UpdateInfo(this UISkillTipComponent self, int skillConfigId)
        {
            SkillConfig skillConfig = SkillConfigCategory.Instance.Get(skillConfigId);

            string path = ABPathHelper.GetAtlasPath_2(ABAtlasTypes.SkillIcon, skillConfig.SkillIcon);
            self.Image_SkillIcon.overrideSprite = await self.Root().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<Sprite>(path);

            self.Text_SkillName.SetText(skillConfig.SkillName);
            self.Text_SkillDes.SetText(skillConfig.SkillDescribe);
        }
    }
}