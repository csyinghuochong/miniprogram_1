using UnityEngine;

namespace ET.Client
{
    public static class GlobalHelp
    {
        public static int GetVersionMode()
        {
            return GameObject.Find("Global").GetComponent<Init>().VersionMode;
        }
    }
}