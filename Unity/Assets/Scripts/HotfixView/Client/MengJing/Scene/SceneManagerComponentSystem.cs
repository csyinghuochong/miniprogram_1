using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ET.Client
{
    [FriendOf(typeof(SceneManagerComponent))]
    [EntitySystemOf(typeof(SceneManagerComponent))]
    public static partial class SceneManagerComponentSystem
    {
        [EntitySystem]
        private static void Awake(this SceneManagerComponent self)
        {
        }

        public static void UnLoadAsset(this SceneManagerComponent self)
        {
            self.Root().GetComponent<GameObjectLoadComponent>().DisposeUnUse();

            ResourcesLoaderComponent resourcesLoaderComponent = self.Root().GetComponent<ResourcesLoaderComponent>();

            // 释放前一个场景的所有资源
            resourcesLoaderComponent.UnLoadAllAsset();

            Resources.UnloadUnusedAssets();
            GC.Collect();
        }

        public static async ETTask ChangeScene(this SceneManagerComponent self, int sceneTypeEnum, int lastScene, int sceneid)
        {
            string paramss = "";
            switch (sceneTypeEnum)
            {
                case MapTypeEnum.InitScene:
                    paramss = "Init";
                    break;
                case MapTypeEnum.LoginScene:
                    paramss = "Login";
                    break;
                case MapTypeEnum.MainCityScene:
                    PlayerInfoComponent playerInfoComponent = self.Root().GetComponent<PlayerInfoComponent>();
                    string scenepath = sceneid.ToString();
                    paramss = scenepath;
                    break;
                default:
                    // paramss = SceneConfigCategory.Instance.Get(sceneid).MapID.ToString();
                    break;
            }

            self.Root().GetComponent<GameObjectLoadComponent>().DisposeAll();

            ResourcesLoaderComponent resourcesLoaderComponent = self.Root().GetComponent<ResourcesLoaderComponent>();

            // 释放前一个场景的所有资源
            resourcesLoaderComponent.UnLoadAllAsset();

            await Resources.UnloadUnusedAssets();
            GC.Collect();

            string path = ABPathHelper.GetScenePath("Empty");

            await resourcesLoaderComponent.LoadSceneAsync(path, LoadSceneMode.Single);

            self.Root().GetComponent<GameObjectLoadComponent>().DisposeAll();

            // 释放前一个场景的所有资源
            resourcesLoaderComponent.UnLoadAllAsset();

            await Resources.UnloadUnusedAssets();
            GC.Collect();

            path = ABPathHelper.GetScenePath(paramss);

            await resourcesLoaderComponent.LoadSceneAsync(path, LoadSceneMode.Single);

            Debug.Log("切换场景" + path);

            string scenename = SceneManager.GetActiveScene().name;

            Debug.Log("当前场景的名称是: " + scenename);

            if (sceneTypeEnum != MapTypeEnum.LoginScene)
            {
                ConfigData.LoadSceneFinished = true;
            }

            int sousceneid = self.Root().GetComponent<MapComponent>().SonSceneId;
            // self.Root().GetComponent<SoundComponent>().PlayBgmSound(sceneTypeEnum, sceneid, sousceneid);
        }

        public static void BeforeChangeScene(this SceneManagerComponent self)
        {
            ConfigData.LoadSceneFinished = false;
        }
    }
}