using UnityEngine;

namespace ET.Client
{
    [ComponentOf]
    public class UILotteryDrawWishComponent : Entity, IAwake<GameObject>, IDestroy
    {
        public GameObject GameObject { get; set; }
    }
}