using UnityEngine;

namespace ET.Client
{
    [ComponentOf]
    public class UILotteryDrawProbabilityComponent : Entity, IAwake<GameObject>, IDestroy
    {
        public GameObject GameObject { get; set; }
    }
}