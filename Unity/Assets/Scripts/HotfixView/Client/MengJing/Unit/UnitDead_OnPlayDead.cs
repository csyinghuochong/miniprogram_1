using System;
using System.Collections.Generic;
using Cysharp.Text;
using UnityEngine;

namespace ET.Client
{
    [Event(SceneType.Demo)]
    public class UnitDead_OnPlayDead : AEvent<Scene, UnitDead>
    {
        protected override async ETTask Run(Scene root, UnitDead args)
        {
            try
            {
                Unit unit = args.Unit;
                if (unit == null || unit.IsDisposed)
                {
                    Log.Error("unitplaydead  unit.InstanceId == 0 || unit.IsDisposed");
                    return;
                }

                if (root.CurrentScene() == null)
                {
                    Log.Error("unitplaydead  unit.ZoneScene().CurrentScene() == null");
                    return;
                }

                MapComponent mapComponent = root.GetComponent<MapComponent>();

                unit.GetComponent<EffectViewComponent>()?.OnDispose();
                unit.GetComponent<FsmComponent>()?.ChangeState(FsmStateEnum.FsmDeathState);

                if (unit.Type == UnitType.Monster)
                {
                    Log.Warning("怪物死亡，播放死亡音效");
                    root.GetComponent<SoundComponent>().PlayClip("SkillAudio/dao", "mp3").Coroutine();
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
            }

            await ETTask.CompletedTask;
        }
    }
}