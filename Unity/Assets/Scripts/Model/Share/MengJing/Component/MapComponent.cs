namespace ET
{
    [ComponentOf(typeof(Scene))]
    public class MapComponent : Entity, IAwake
    {
        private float timeScale = 1f;

        public float TimeScale
        {
            get => this.timeScale;
            set
            {
                this.timeScale = value;

                EventSystem.Instance.Publish(this.Scene(), new UpdateTimeScale() { TimeScale = this.timeScale });
            }
        }

        public float DeltaTime => TimeInfo.Instance.DeltaTime * TimeScale;

        public int SceneId { set; get; }

        public int MapType { set; get; }

        public long LastQuitTime { set; get; }

        public int SonSceneId { set; get; }

        public int NavMeshId { set; get; }

        public string ParamInfo { set; get; }
    }
}