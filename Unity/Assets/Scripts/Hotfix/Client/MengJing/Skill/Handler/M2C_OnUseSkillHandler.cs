namespace ET.Client
{
    [MessageHandler(SceneType.Demo)]
    public class M2C_OnUseSkillHandler : MessageHandler<Scene, M2C_OnUseSkill>
    {
        protected override async ETTask Run(Scene root, M2C_OnUseSkill message)
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
                unit.GetComponent<SkillManagerComponentC>().OnUseSkill(message);
            }

            await ETTask.CompletedTask;
        }
    }
}