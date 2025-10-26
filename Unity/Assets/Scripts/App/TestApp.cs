using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ET.Client
{
    // 最顶层的程序集，可以写Mono脚本，可以引用ET。目前没有什么用，未来可能使用其他Momo脚本写的客户端框架，然后只把ET用于网络通讯
    public class TestApp : MonoBehaviour
    {
        void Start()
        {
            Log.Debug("测试Unit.App程序集");
            // GlobalComponent.Instance.Unit.gameObject.SetActive(false);
        }
    }
}