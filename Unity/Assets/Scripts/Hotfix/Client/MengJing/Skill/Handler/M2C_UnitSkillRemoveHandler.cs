namespace ET.Client
{
    [MessageHandler(SceneType.Demo)]
    public class M2C_UnitSkillRemoveHandler: MessageHandler<Scene, M2C_UnitSkillRemove>
    {
        protected override async ETTask Run(Scene root, M2C_UnitSkillRemove message)
        {
            using var _ = message;
            
            Scene currentScene = root.CurrentScene();
            if (currentScene == null)
            {
                return;
            }

            Unit unit = currentScene.GetComponent<UnitComponent>().Get(message.UnitId);
            if (unit != null)
            {
                unit.GetComponent<SkillManagerComponentC>().RemoveSkill(message.SkillId);
            }
            
            await ETTask.CompletedTask;
        }
    }
}