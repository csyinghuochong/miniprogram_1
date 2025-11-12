namespace ET.Client
{
    public class BuffCHandlerAttribute : BaseAttribute
    {
    }

    [EnableClass]
    [BuffCHandler]
    public abstract class BuffCHandler
    {
        public abstract void OnInit(BuffC buff);

        public abstract void OnReset(BuffC buff, float endTime);

        public abstract void OnExecute(BuffC buff);

        public abstract void OnUpdate(BuffC buff, float deltaTime);

        public abstract void OnFinished(BuffC buff);
    }
}