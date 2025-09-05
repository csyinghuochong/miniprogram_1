namespace ET.Client
{
    public abstract class AUIEvent: HandlerObject
    {
        public abstract ETTask<UI> OnCreate(Scene scene, UIComponent uiComponent);
        public abstract void OnRemove(Scene scene, UIComponent uiComponent);
    }
}