using Cysharp.Text;

namespace ET
{
    public static class ABAtlasTypes
    {
        public const string ItemIcon = "ItemIcon";
        public const string ItemQualityIcon = "ItemQualityIcon";
        public const string HeroIcon = "HeroIcon";
        public const string RoleSkillIcon = "RoleSkillIcon";
        public const string MonsterIcon = "MonsterIcon";
        public const string TaskIcon = "TaskIcon";
        public const string OtherIcon = "OtherIcon";
    }

    public static class ABUnitType
    {
        public const string Hero = "Hero";
        public const string Monster = "Monster";
    }

    /// <summary>
    /// AB实用函数集，主要是路径拼接
    /// </summary>
    public static class ABPathHelper
    {
        public static string GetAnimFbxPath(string fileName)
        {
            string prefabPath = ZString.Format("Assets/Bundles/Animation/{0}.fbx", fileName);

            return prefabPath;
        }

        public static string GetMaterialPath(string fileName)
        {
            string prefabPath = ZString.Format("Assets/Bundles/Material/{0}.mat", fileName);

            return prefabPath;
        }

        public static string GetTexturePath(string fileName)
        {
            string prefabPath = ZString.Format("Assets/Bundles/Altas/{0}.prefab", fileName);

            return prefabPath;
        }

        public static string GetUGUIPath(string name)
        {
            string prefabPath = ZString.Format("Assets/Bundles/UI/{0}.prefab", name);

            return prefabPath;
        }

        public static string GetConfigPath(string fileName)
        {
            string prefabPath = ZString.Format("Assets/Bundles/Config/{0}.bytes", fileName);

            return prefabPath;
        }

        public static string GetMapConfigPath(string fileName)
        {
            string prefabPath = ZString.Format("Assets/Bundles/MapConfig/{0}.bytes", fileName);

            return prefabPath;
        }

        public static string GetNormalConfigPath(string fileName)
        {
            string prefabPath = ZString.Format("Assets/Bundles/Independent/{0}.prefab", fileName);

            return prefabPath;
        }

        public static string GetAudioPath(string fileName)
        {
            string prefabPath = ZString.Format("Assets/Bundles/Audio/{0}.mp3", fileName);

            return prefabPath;
        }

        public static string GetAudioOggPath(string fileName)
        {
            string prefabPath = ZString.Format("Assets/Bundles/Audio/{0}.ogg", fileName);

            return prefabPath;
        }

        public static string GetSoundPath(string fileName)
        {
            string prefabPath = ZString.Format("Assets/Bundles/Sound/{0}.prefab", fileName);

            return prefabPath;
        }

        public static string GetUnitPath(string path, string fileName)
        {
            string prefabPath = ZString.Format("Assets/Bundles/Unit/{0}/{1}.prefab", path, fileName);

            return prefabPath;
        }

        // 在UI上显示的Spine动画
        public static string GetUIUnitPath(string path, string fileName)
        {
            string prefabPath = ZString.Format("Assets/Bundles/UI/Spine/{0}/{1}.prefab", path, fileName);

            return prefabPath;
        }

        public static string GetSceneUnitPath(string fileName)
        {
            string prefabPath = ZString.Format("Assets/Bundles/Unit/Scene/{0}.prefab", fileName);

            return prefabPath;
        }

        public static string GetItemPath(string fileName)
        {
            string prefabPath = ZString.Format("Assets/Bundles/Unit/ItemModel/{0}.prefab", fileName);

            return prefabPath;
        }

        public static string GetScenePath(string fileName)
        {
            string prefabPath = ZString.Format("Assets/Bundles/Scenes/{0}.unity", fileName);

            return prefabPath;
        }

        public static string GetEffetPath(string fileName)
        {
            string prefabPath = ZString.Format("Assets/Bundles/Effect/{0}.prefab", fileName);

            return prefabPath;
        }

        //技能特效
        public static string GetSkillEffetPath(string fileName)
        {
            string prefabPath = ZString.Format("Assets/Bundles/Effect/SkillEffect/{0}.prefab", fileName);

            return prefabPath;
        }

        //技能受击特效
        public static string GetSkillHitEffetPath(string fileName)
        {
            string prefabPath = ZString.Format("Assets/Bundles/Effect/SkillHitEffect/{0}.prefab", fileName);

            return prefabPath;
        }

        //图集2
        public static string GetAtlasPath_2(string path, string name)
        {
            string prefabPath = ZString.Format("Assets/Bundles/Icon/{0}/{1}.png", path, name);

            return prefabPath;
        }

        //图集
        public static string GetAtlasPath(string path)
        {
            string prefabPath = ZString.Format("Assets/Bundles/Atlas/{0}.prefab", path);

            return prefabPath;
        }

        //Png
        public static string GetJpgPath(string path)
        {
            string prefabPath = ZString.Format("Assets/Bundles/Jpg/{0}.jpg", path);

            return prefabPath;
        }

        //文本
        public static string GetTextPath(string text)
        {
            string prefabPath = ZString.Format("Assets/Bundles/Text/{0}.txt", text);

            return prefabPath;
        }

        public static string GetRecastPath(int text)
        {
            string prefabPath = ZString.Format("Assets/Bundles/Recast/{0}.bytes", text);

            return prefabPath;
        }
    }
}