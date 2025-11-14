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
            self.MainCamera = self.Root().GetComponent<GlobalComponent>().MainCamera;
            self.MapMask = (1 << LayerMask.NameToLayer(nameof(LayerEnum.Terrain))) | (1 << LayerMask.NameToLayer(nameof(LayerEnum.Map)));
            self.NpcMask = 1 << LayerMask.NameToLayer(nameof(LayerEnum.NPC));
            self.BoxMask = 1 << LayerMask.NameToLayer(nameof(LayerEnum.Box));
            self.PlayerMask = 1 << LayerMask.NameToLayer(nameof(LayerEnum.Player));
            self.MonsterMask = 1 << LayerMask.NameToLayer(nameof(LayerEnum.Monster));
            self.BuildingMask = 1 << LayerMask.NameToLayer(nameof(LayerEnum.Building));

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