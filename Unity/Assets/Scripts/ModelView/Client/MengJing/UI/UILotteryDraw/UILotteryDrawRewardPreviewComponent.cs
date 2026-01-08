using UnityEngine;

namespace ET.Client
{
    [ComponentOf]
    public class UILotteryDrawRewardPreviewComponent : Entity, IAwake<GameObject>, IDestroy
    {
        public GameObject GameObject { get; set; }
    }
}