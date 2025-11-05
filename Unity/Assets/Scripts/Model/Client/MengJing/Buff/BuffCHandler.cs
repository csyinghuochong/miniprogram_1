namespace ET.Client
{
    public class BuffCHandlerAttribute : BaseAttribute
    {
    }

    [EnableClass]
    [BuffCHandler]
    public abstract class BuffCHandler
    {
        public abstract void OnInit(BuffC buffc);

        public abstract void OnReset(BuffC buffc, float endTime);

        public abstract void OnExecute(BuffC buffc);

        public abstract void OnUpdate(BuffC buffc);

        public abstract void OnFinished(BuffC buffc);
    }
}