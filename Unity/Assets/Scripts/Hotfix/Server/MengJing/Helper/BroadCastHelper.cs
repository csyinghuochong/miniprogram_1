using System.Collections.Generic;
using System.Linq;

namespace ET.Server
{
    
    public static class BroadCastHelper
    {
        public static List<StartSceneConfig> GetAllScene(int zone)
        {
            List<StartSceneConfig> zonescenes = new List<StartSceneConfig>();
            List<StartSceneConfig> listallscene = StartSceneConfigCategory.Instance.DataList;
            for (int i = 0; i < listallscene.Count; i++)
            {
                if ( ServerHelper.GetNewServerId(listallscene[i].Zone) == zone)
                {
                    zonescenes.Add( listallscene[i] );
                }
            }

            return zonescenes;
        }

        /// <summary>
        /// 一般是做全服操作
        /// </summary>
        /// <returns></returns>
        public static List<int> GetAllZone()
        {
            List<int> zoneList = new List<int> { };
            List<StartZoneConfig> listprogress = StartZoneConfigCategory.Instance.DataList;
            for (int i = 0; i < listprogress.Count; i++)
            {
                if (listprogress[i].Id >= CommonHelp.MaxZone || ConfigData.InnerZoneList.Contains(listprogress[i].Id))
                {
                    continue;
                }
                if (!StartSceneConfigCategory.Instance.Gates.ContainsKey(listprogress[i].Id))
                {
                    continue;
                }
                zoneList.Add(listprogress[i].Id);
            }
            return zoneList;
        }
    }
}

