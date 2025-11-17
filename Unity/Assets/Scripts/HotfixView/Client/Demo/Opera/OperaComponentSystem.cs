using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ET.Client
{
    public enum LayerEnum
    {
        Player,
        Monster,
        Hero,
        Map,
        MovableArea,
    }

    [EntitySystemOf(typeof(OperaComponent))]
    [FriendOf(typeof(OperaComponent))]
    public static partial class OperaComponentSystem
    {
        [EntitySystem]
        private static void Awake(this OperaComponent self)
        {
            self.MainCamera = self.Root().GetComponent<GlobalComponent>().MainCamera;

            Init init = GameObject.Find("Global").GetComponent<Init>();
            self.EditorMode = init.EditorMode;
            self.MainUnit = UnitHelper.GetMyUnitFromClientScene(self.Root());
        }

        [EntitySystem]
        private static void Update(this OperaComponent self)
        {
        }
    }
}