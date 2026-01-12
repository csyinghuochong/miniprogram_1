using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ComponentOf]
    public class UILotteryDrawWishComponent : Entity, IAwake<GameObject>, IDestroy
    {
        public GameObject GameObject { get; set; }

        public Button Button_Close;
        
    }
}