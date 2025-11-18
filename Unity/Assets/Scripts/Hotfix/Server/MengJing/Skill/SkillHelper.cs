namespace ET.Server
{
    public static class SkillHelper
    {
        public static bool havePassiveSkillType(SkillPassiveType[] typeList, SkillPassiveType passType)
        {
            if (typeList == null)
            {
                return false;
            }

            for (int i = 0; i < typeList.Length; i++)
            {
                if (typeList[i] == passType)
                {
                    return true;
                }
            }

            return false;
        }
    }
}