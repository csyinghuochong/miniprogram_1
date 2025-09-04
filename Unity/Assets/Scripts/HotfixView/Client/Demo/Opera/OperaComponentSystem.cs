using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ET.Client
{
    public enum LayerEnum
    {
        Drop,
        NPC,
        Terrain,
        Monster,
        Player,
        Map,
        RenderTexture,
        Box,
        Obstruct,
        Building,
    }

    [EntitySystemOf(typeof(OperaComponent))]
    [FriendOf(typeof(OperaComponent))]
    public static partial class OperaComponentSystem
    {
        [EntitySystem]
        private static void Awake(this OperaComponent self)
        {
            self.MainCamera = self.Root().GetComponent<GlobalComponent>().MainCamera.GetComponent<Camera>();
            self.MapMask = (1 << LayerMask.NameToLayer(LayerEnum.Terrain.ToString())) | (1 << LayerMask.NameToLayer(LayerEnum.Map.ToString()));
            self.NpcMask = 1 << LayerMask.NameToLayer(LayerEnum.NPC.ToString());
            self.BoxMask = 1 << LayerMask.NameToLayer(LayerEnum.Box.ToString());
            self.PlayerMask = 1 << LayerMask.NameToLayer(LayerEnum.Player.ToString());
            self.MonsterMask = 1 << LayerMask.NameToLayer(LayerEnum.Monster.ToString());
            self.BuildingMask = 1 << LayerMask.NameToLayer(LayerEnum.Building.ToString());

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