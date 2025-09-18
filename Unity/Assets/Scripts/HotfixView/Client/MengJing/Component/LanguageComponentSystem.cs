using System.Collections.Generic;
using System.Text.RegularExpressions;
using I2.Loc;
using UnityEngine;

namespace ET.Client
{
    public static class LanguageType
    {
        public const string Chinese = "Chinese";
        public const string English = "English";
        public const string Japanese = "Japanese";
    }

    [FriendOf(typeof(LanguageComponent))]
    [EntitySystemOf(typeof(LanguageComponent))]
    public static partial class GameSettingLanguageSystem
    {
        [EntitySystem]
        private static void Awake(this LanguageComponent self)
        {
            self.OnInitL2Localization();
        }

        [EntitySystem]
        private static void Update(this LanguageComponent self)
        {
            if (InputHelper.GetKey(KeyCode.LeftAlt) && InputHelper.GetKeyDown(KeyCode.L) ||
                InputHelper.GetKeyDown(KeyCode.LeftAlt) && InputHelper.GetKey(KeyCode.L))
            {
                var languages = new List<string>
                {
                    LanguageType.Chinese,
                    LanguageType.English,
                    LanguageType.Japanese
                };

                int currentIndex = languages.IndexOf(self.CurrentLanguage);

                int nextIndex = (currentIndex + 1) % languages.Count;

                self.SetLanguage(languages[nextIndex], true);
            }
        }

        private static void OnInitL2Localization(this LanguageComponent self)
        {
            self.DefaultLanguage = PlayerPrefsHelper.GetString(PlayerPrefsHelper.Localization, LanguageType.Chinese);

            GameObject go = UnityEngine.Object.Instantiate(new GameObject());
            UnityEngine.Object.DontDestroyOnLoad(go);
            go.name = "[I2LocalizeMgr]";
            go.AddComponent<LanguageSource>();
            self.LanguageSource = go.GetComponent<LanguageSource>();

#if UNITY_EDITOR
            if (!self.UseRuntimeModule)
            {
                LocalizationManager.RegisterSourceInEditor();
                self.UpdateAllLanguages();
                self.SetLanguage(self.DefaultLanguage);
            }
            else
            {
                self.LanguageSourceData.Awake();
                self.LoadLanguage(self.DefaultLanguage, true).Coroutine();
            }
#else
            self.LanguageSourceData.Awake();
            self.LoadLanguage(self.DefaultLanguage, true);
#endif
        }

        private static void UpdateAllLanguages(this LanguageComponent self)
        {
            self.AllLanguage.Clear();
            foreach (var language in LocalizationManager.GetAllLanguages())
            {
                var newLanguage = Regex.Replace(language, @"[\r\n]", "");
                self.AllLanguage.Add(newLanguage);
            }
        }

        public static bool CheckLanguage(this LanguageComponent self, string language)
        {
            return self.AllLanguage.Contains(language);
        }

        //运行时注意 需要提前加载你需要的所有语言
        public static bool SetLanguage(this LanguageComponent self, string language, bool load = false)
        {
            if (!self.CheckLanguage(language))
            {
                if (load)
                {
                    self.LoadLanguage(language, true).Coroutine();
                    return true;
                }

                Log.Error($"当前没有这个语言无法切换到此语言 {language}");
                return false;
            }

            if (self.CurrentLanguage == language)
            {
                return true;
            }

            Log.Debug($"设置当前语言 = {language}");
            LocalizationManager.CurrentLanguage = language;
            self.CurrentLanguage = language;
            return true;
        }

        //根据需求可提前加载语言
        public static async ETTask LoadLanguage(this LanguageComponent self, string language, bool setCurrent = false)
        {
#if UNITY_EDITOR
            if (!self.UseRuntimeModule)
            {
                Log.Error($"禁止在此模式下 动态加载语言 {language}");
                return;
            }
#endif

            if (self.CheckLanguage(language))
            {
                Log.Error($"当前语言已存在 请勿重复加载 {language}");
                return;
            }

            var assetName = self.GetLanguageAssetName(language);

            var assetTextAsset = await self.Root().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<TextAsset>(assetName);
            if (assetTextAsset == null)
            {
                Log.Error($"没有加载到目标语言资源 {language}");
                return;
            }

            Log.Debug($"加载语言成功 {language}");

            self.UseLocalizationCSV(assetTextAsset.text, !setCurrent);
            if (setCurrent)
            {
                self.SetLanguage(language);
            }

            //语言加载完毕后就可以释放资源了
            // YIUILoadHelper.Release(assetTextAsset);
        }

        private static string GetLanguageAssetName(this LanguageComponent self, string language)
        {
            return $"Assets/Bundles/Text/{I2LocalizeHelper.I2ResAssetNamePrefix}{language}.csv";
        }

        private static void UseLocalizationCSV(this LanguageComponent self, string text, bool isLocalizeAll = false)
        {
            self.LanguageSourceData.Import_CSV(string.Empty, text, eSpreadsheetUpdateMode.Replace, ',');
            if (isLocalizeAll)
            {
                LocalizationManager.LocalizeAll(); // 强制使用新数据本地化所有启用的标签/精灵
            }

            self.UpdateAllLanguages();
        }
    }
}