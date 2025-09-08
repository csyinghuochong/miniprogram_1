using System.Collections.Generic;
using UnityEngine;

namespace ET.Client
{
    public static class PlayerPrefsHelper
    {
        public const string MyServerID = "Mini_1_MyServerID";
        public const string LastUserID = "Mini_1_LastUserID";
        public const string LastGuide = "Mini_1_LastGuide_0";
        public const string LastFrame = "Mini_1_LastFrame_0";
        public const string MusicVolume = "Mini_1_MusicVolume";
        public const string SoundVolume = "Mini_1_SoundVolume";
        public const string MyOldServerID = "Mini_1_MyOldServerID";
        public const string LastLoginType = "Mini_1_LastLoginType";
        public const string LoginErrorTime = "Mini_1_LoginErrorTime";
        public const string Localization = "Mini_1_Localization";

        public static void SetInt(string key, int value)
        {
            PlayerPrefs.SetInt(key, value);
        }

        public static int GetInt(string key, int defaultValue = 0)
        {
            return PlayerPrefs.GetInt(key, defaultValue);
        }

        public static void SetFloat(string key, float value)
        {
            PlayerPrefs.SetFloat(key, value);
        }

        public static float GetFloat(string key, float defaultValue = 0)
        {
            return PlayerPrefs.GetFloat(key, defaultValue);
        }

        public static void SetString(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
        }

        public static string GetString(string key, string defaultValue = "")
        {
            return PlayerPrefs.GetString(key, defaultValue);
        }
        
        public static List<int> GetOldServerIds()
        {
            List<int> serverids = new List<int>();
            string oldservers = GetString(MyOldServerID);
            if (string.IsNullOrEmpty(oldservers))
            {
                return serverids;
            }

            string[] serverstr = oldservers.Split('@');
            for (int i = 0; i < serverstr.Length; i++)
            {
                serverids.Add(int.Parse(serverstr[i]));
            }

            return serverids;
        }

        public static void SetOldServerIds(int serverid)
        {
            string oldservers = GetString(MyOldServerID);
            if (string.IsNullOrEmpty(oldservers))
            {
                oldservers = serverid.ToString();
            }
            else
            {
                List<int> serveridlist = new List<int>();
                string[] serverstr = oldservers.Split('@');
                for (int i = 0; i < serverstr.Length; i++)
                {
                    serveridlist.Add(int.Parse(serverstr[i]));
                }

                if (serveridlist.Contains(serverid))
                {
                    serveridlist.Remove(serverid);
                }

                serveridlist.Add((int)serverid);
                if (serveridlist.Count > 6)
                {
                    serveridlist.RemoveAt(0);
                }

                oldservers = string.Empty;
                for (int i = 0; i < serveridlist.Count; i++)
                {
                    oldservers = $"{oldservers}{serveridlist[i]}@";
                }

                oldservers = oldservers.Substring(0, oldservers.Length - 1);
            }

            SetString(MyOldServerID, oldservers);
        }
    }
}