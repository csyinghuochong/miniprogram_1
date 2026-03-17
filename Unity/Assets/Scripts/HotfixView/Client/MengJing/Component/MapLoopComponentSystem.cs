using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(MapLoopComponent))]
    [FriendOf(typeof(MapLoopComponent))]
    public static partial class MapLoopComponentSystem
    {
        [EntitySystem]
        private static void Awake(this MapLoopComponent self)
        {
            self.LookAtUnit = UnitHelper.GetMyUnitFromClientScene(self.Root());

            self.MapList.Add(GameObject.Find("Tile_0"));
            self.MapList.Add(GameObject.Find("Tile_1"));
            self.MapList.Add(GameObject.Find("Tile_2"));

            foreach (GameObject map in self.MapList)
            {
                self.MapOldPositions.Add(map.transform.position);
                self.TotalHeight += 72;
            }
        }

        [EntitySystem]
        private static void Update(this MapLoopComponent self)
        {
            if (self.LookAtUnit.Position.y < 72)
            {
                if (self.MapOldPositions[0] != self.MapList[0].transform.position)
                {
                    for (int i = 0; i < self.MapList.Count; i++)
                    {
                        self.MapList[i].transform.position = self.MapOldPositions[i];
                    }
                }

                return;
            }

            if (self.LookAtUnit.Position.y > self.MapList[0].transform.position.y + self.TotalHeight / 2)
            {
                GameObject bottomMap = self.MapList[0];
                bottomMap.transform.position = new Vector3(bottomMap.transform.position.x, bottomMap.transform.position.y + self.TotalHeight, bottomMap.transform.position.z);
                self.MapList.RemoveAt(0);
                self.MapList.Add(bottomMap);
            }

            if (self.LookAtUnit.Position.y < self.MapList[^1].transform.position.y - self.TotalHeight / 2)
            {
                GameObject topMap = self.MapList[^1];
                topMap.transform.position = new Vector3(topMap.transform.position.x, topMap.transform.position.y - self.TotalHeight, topMap.transform.position.z);
                self.MapList.RemoveAt(self.MapList.Count - 1);
                self.MapList.Insert(0, topMap);
            }
        }

        [EntitySystem]
        private static void Destroy(this MapLoopComponent self)
        {
            self.MapList.Clear();
        }
    }
}