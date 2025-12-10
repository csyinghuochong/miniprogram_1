namespace ET.Server
{
    [Event(SceneType.Map)]
    public class UnitKillEvent_NotifyOther : AEvent<Scene, UnitKillEvent>
    {
        protected override async ETTask Run(Scene scene, UnitKillEvent args)
        {
            Unit defendUnit = args.UnitDefend;
            Unit attackUnit = args.UnitAttack;

            MapComponent mapComponent = scene.GetComponent<MapComponent>();
            int sceneId = mapComponent.SceneId;
            MapType mapType = mapComponent.MapType;

            // 角色要过几秒才销毁，先停止一些组件
            // defendUnit.GetComponent<MoveComponent>()?.Stop(false);
            // defendUnit.GetComponent<Move2DComponent>()?.Stop();
            defendUnit.GetComponent<UnitMoveComponent>()?.Stop();
            defendUnit.GetComponent<AIComponent>()?.Stop();
            defendUnit.GetComponent<SkillManagerComponentS>()?.OnFinish(false);
            defendUnit.GetComponent<SkillPassiveComponent>()?.Stop();
            defendUnit.GetComponent<BuffManagerComponentS>()?.OnFinish();

            switch (mapType)
            {
                case MapType.LocalLevel:
                {
                    if (defendUnit.Type == UnitType.Monster)
                    {
                        scene.GetComponent<LocalLevelComponent>().OnKillEvent(defendUnit);
                        DropHelper.MonsterDropItem(defendUnit);
                    }

                    break;
                }
            }

            if (attackUnit != null && !attackUnit.IsDisposed)
            {
                if (attackUnit.Type == UnitType.Hero)
                {
                    Unit master = attackUnit.GetParent<UnitComponent>().Get(attackUnit.GetComponent<NumericComponentS>().GetAsLong(NumericType.MasterId));
                    master.GetComponent<TaskComponentS>().OnKillUnit(defendUnit, mapType);
                }
            }

            NumericComponentS numericComponentDefend = defendUnit.GetComponent<NumericComponentS>();
            numericComponentDefend.ApplyValue(NumericType.Now_Dead, 1);

            long waitTime = 500;
            OnRemoveUnit(defendUnit.Root(), args, waitTime).Coroutine();

            await ETTask.CompletedTask;
        }

        private async ETTask OnRemoveUnit(Scene root, UnitKillEvent args, long waitTime)
        {
            Unit unitDefend = args.UnitDefend;

            await root.GetComponent<TimerComponent>().WaitAsync(waitTime);

            if (unitDefend.IsDisposed)
            {
                return;
            }

            if (unitDefend.Type != UnitType.Player && args.WaitRevive == 0)
            {
                unitDefend.GetParent<UnitComponent>().Remove(unitDefend.Id);
            }
        }
    }
}