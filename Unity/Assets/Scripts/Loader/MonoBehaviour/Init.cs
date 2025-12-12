using System;
using System.Collections;
using CommandLine;
using UniFramework.Event;
using UnityEngine;
using YooAsset;

namespace ET
{
    [EnableClass]
    public class Init : MonoBehaviour
    {
        public bool EditorMode;
        public int VersionMode = ET.VersionMode.Alpha;
        private EPlayMode ePlayMode;

        private int BigVersion = 0;
        private int BigVersionIOS = 0;

        public Action<bool> OnApplicationFocusHandler;

        private void Start()
        {
            this.StartAsync().Coroutine();
        }

        private async ETTask StartAsync()
        {
            DontDestroyOnLoad(gameObject);

            AppDomain.CurrentDomain.UnhandledException += (sender, e) => { Log.Error(e.ExceptionObject.ToString()); };

            // 命令行参数
            string[] args = "".Split(" ");
            Parser.Default.ParseArguments<Options>(args)
                    .WithNotParsed(error => throw new Exception($"命令行格式错误! {error}"))
                    .WithParsed((o) => World.Instance.AddSingleton(o));
            Options.Instance.StartConfig = $"StartConfig/Localhost";

            Options.Instance.Develop = VersionMode >= ET.VersionMode.Beta ? 0 : 1;
            Options.Instance.LogLevel = VersionMode >= ET.VersionMode.Beta ? 3 : 1; // 打印Debug Message消耗较大，可根据需要改为 3

            World.Instance.AddSingleton<Logger>().Log = new UnityLogger();
            ETTask.ExceptionHandler += Log.Error;

            World.Instance.AddSingleton<TimeInfo>();
            World.Instance.AddSingleton<FiberManager>();

            GlobalConfig globalConfig = Resources.Load<GlobalConfig>("GlobalConfig");
            ePlayMode = globalConfig.EPlayMode;

            // 游戏管理器
            GameManager.Instance.Behaviour = this;

            // 初始化事件系统
            UniEvent.Initalize();

            // 初始化资源系统
            World.Instance.AddSingleton<ResourcesComponent>();

            OnStartGame();
            await ETTask.CompletedTask;
        }

        public void OnStartGame()
        {
            // 显示更新页面
            TogglePatchWindow(true);

            // 开始补丁更新流程
            StartCoroutine(StartUpdate());
        }

        public void TogglePatchWindow(bool show)
        {
            GameObject.Find("Global/UI/PopUpRoot/PatchWindow").gameObject.SetActive(show);
        }

        private IEnumerator StartUpdate()
        {
            PatchOperation operation = new PatchOperation("DefaultPackage", ePlayMode);
            operation.UpdateDownHandler = () => { OnUpdaterDone().Coroutine(); };
            YooAssets.StartOperation(operation);
            yield return operation;
        }

        // 更新完成
        private async ETTask OnUpdaterDone()
        {
            // 设置默认的资源包
            var gamePackage = YooAssets.GetPackage("DefaultPackage");
            YooAssets.SetDefaultPackage(gamePackage);
            
            // 加载热更代码
            CodeLoader codeLoader = World.Instance.AddSingleton<CodeLoader>();
            await codeLoader.DownloadAsync();

            codeLoader.Start();
        }

        private void Update()
        {
            TimeInfo.Instance.Update();
            FiberManager.Instance.Update();
        }

        private void LateUpdate()
        {
            FiberManager.Instance.LateUpdate();
        }

        private void FixedUpdate()
        {
            FiberManager.Instance.FixedUpdate();
        }

        private void OnApplicationQuit()
        {
            World.Instance.Dispose();
        }

        // 当程序获得或者是去焦点时
        private void OnApplicationFocus(bool hasFocus)
        {
            try
            {
                OnApplicationFocusHandler?.Invoke(hasFocus);
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}