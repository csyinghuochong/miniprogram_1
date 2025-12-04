using System.Collections.Generic;
using System.Linq;

namespace ET.Server
{
    public static class BroadCastHelper
    {
        public static async ETTask StopServer(Scene root)
        {
            M2All_StopServer request = M2All_StopServer.Create();

            List<StartSceneConfig> otherScenes = BroadCastHelper.GetAllScene(root.Zone());

            for (int i = 0; i < otherScenes.Count; i++)
            {
                await root.GetComponent<MessageSender>().Call(otherScenes[i].ActorId, request);
            }
        }

        public static List<StartSceneConfig> GetAllScene(int zone)
        {
            List<StartSceneConfig> allScene = new List<StartSceneConfig>();
            foreach (StartSceneConfig startSceneConfig in StartSceneConfigCategory.Instance.DataList)
            {
                if (ServerHelper.GetNewServerId(startSceneConfig.Zone) == zone)
                {
                    allScene.Add(startSceneConfig);
                }
            }

            return allScene;
        }

        /// <summary>
        /// 一般是做全服操作
        /// </summary>
        /// <returns></returns>
        public static List<int> GetAllZone()
        {
            List<int> allZone = new List<int>();
            foreach (StartZoneConfig startZoneConfig in StartZoneConfigCategory.Instance.DataList)
            {
                if (startZoneConfig.Id >= CommonHelp.MaxZone || ConfigData.InnerZoneList.Contains(startZoneConfig.Id))
                {
                    continue;
                }

                if (!StartSceneConfigCategory.Instance.Gates.ContainsKey(startZoneConfig.Id))
                {
                    continue;
                }

                allZone.Add(startZoneConfig.Id);
            }

            return allZone;
        }
    }
}