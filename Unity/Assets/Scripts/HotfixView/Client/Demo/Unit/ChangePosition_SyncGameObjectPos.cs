namespace ET.Client
{
    [FriendOf(typeof(GameObjectComponent))]
    [Event(SceneType.Current)]
    public class ChangePosition_SyncGameObjectPos : AEvent<Scene, ChangePosition>
    {
        protected override async ETTask Run(Scene scene, ChangePosition args)
        {
            Unit unit = args.Unit;

            if (unit.Position.Equals(args.OldPos))
            {
                return;
            }

            if (unit.MainHero)
            {
                scene.Root().GetComponent<UIComponent>().Get(UIType.UIMain)?.GetComponent<UIMainComponent>()?.UIMiniMapComponent?.OnUpdateMiniMapAllUnit();
            }
            else
            {
                scene.Root().GetComponent<UIComponent>().Get(UIType.UIMain)?.GetComponent<UIMainComponent>()?.UIMiniMapComponent?.OnUpdateMiniMapOneUnit(unit);
            }

            TransformSyncComponent transformSyncComponent = unit.GetComponent<TransformSyncComponent>();
            if (transformSyncComponent != null)
            {
                transformSyncComponent.ReceiveServerPosition(unit.Position);
                return;
            }

            GameObjectComponent gameObjectComponent = unit.GetComponent<GameObjectComponent>();
            if (gameObjectComponent != null)
            {
                gameObjectComponent.UpdatePositon(unit.Position);
                return;
            }

            await ETTask.CompletedTask;
        }
    }
}