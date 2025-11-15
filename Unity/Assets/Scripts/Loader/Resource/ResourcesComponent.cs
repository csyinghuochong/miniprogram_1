using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using YooAsset;

namespace ET
{
    /// <summary>
    /// 远端资源地址查询服务类
    /// </summary>
    public class RemoteServices : IRemoteServices
    {
        private readonly string _defaultHostServer;
        private readonly string _fallbackHostServer;

        public RemoteServices(string defaultHostServer, string fallbackHostServer)
        {
            _defaultHostServer = defaultHostServer;
            _fallbackHostServer = fallbackHostServer;
        }
        string IRemoteServices.GetRemoteMainURL(string fileName)
        {
            return $"{_defaultHostServer}/{fileName}";
        }
        string IRemoteServices.GetRemoteFallbackURL(string fileName)
        {
            return $"{_fallbackHostServer}/{fileName}";
        }
    }
    
    public class ResourcesComponent: Singleton<ResourcesComponent>, ISingletonAwake
    {
        public void Awake()
        {
            YooAssets.Initialize();
            //YooAssets.SetOperationSystemMaxTimeSlice(30);
            BetterStreamingAssets.Initialize();
        }

        protected override void Destroy()
        {
            YooAssets.Destroy();
        }

        public async ETTask CreatePackageAsync(string packageName, EPlayMode ePlayMode, bool isDefault = false)
        {
            ResourcePackage package = YooAssets.CreatePackage(packageName);
            if (isDefault)
            {
                YooAssets.SetDefaultPackage(package);
            }

            // 编辑器下的模拟模式
            switch (ePlayMode)
            {
                case EPlayMode.EditorSimulateMode:
                {
                    var buildResult = EditorSimulateModeHelper.SimulateBuild(packageName);    
                    var packageRoot = buildResult.PackageRootDirectory;
                    var editorFileSystemParams = FileSystemParameters.CreateDefaultEditorFileSystemParameters(packageRoot);
                    EditorSimulateModeParameters createParameters = new();
                    createParameters.EditorFileSystemParameters = editorFileSystemParams;
                    await package.InitializeAsync(createParameters).Task;
                    break;
                }
                case EPlayMode.OfflinePlayMode:
                {
                    var buildinFileSystemParams = FileSystemParameters.CreateDefaultBuildinFileSystemParameters();
                    var createParameters = new OfflinePlayModeParameters();
                    createParameters.BuildinFileSystemParameters = buildinFileSystemParams;
                    await package.InitializeAsync(createParameters).Task;
                    break;
                }
                case EPlayMode.HostPlayMode:
                {
                    string defaultHostServer = GetHostServerURL();
                    string fallbackHostServer = GetHostServerURL();
                    IRemoteServices remoteServices = new RemoteServices(defaultHostServer, fallbackHostServer);
                    var cacheFileSystemParams = FileSystemParameters.CreateDefaultCacheFileSystemParameters(remoteServices);
                    var buildinFileSystemParams = FileSystemParameters.CreateDefaultBuildinFileSystemParameters();   
                    HostPlayModeParameters createParameters = new();
                    createParameters.BuildinFileSystemParameters = buildinFileSystemParams; 
                    createParameters.CacheFileSystemParameters = cacheFileSystemParams;
                    await package.InitializeAsync(createParameters).Task;
                    break;
                }
                case EPlayMode.WebPlayMode:
                {
                    string defaultHostServer = GetHostServerURL();
                    string fallbackHostServer = GetHostServerURL();
                    IRemoteServices remoteServices = new RemoteServices(defaultHostServer, fallbackHostServer);
                    var webServerFileSystemParams = FileSystemParameters.CreateDefaultWebServerFileSystemParameters();
                    var webRemoteFileSystemParams = FileSystemParameters.CreateDefaultWebRemoteFileSystemParameters(remoteServices); //支持跨域下载
                    var createParameters = new WebPlayModeParameters();
                    createParameters.WebServerFileSystemParameters = null;//本地直接Build and Run，设置为null
                    // createParameters.WebServerFileSystemParameters = webServerFileSystemParams;
                    createParameters.WebRemoteFileSystemParameters = webRemoteFileSystemParams;
                    await package.InitializeAsync(createParameters).Task;
                    
                    var rpvo = package.RequestPackageVersionAsync();
                    await rpvo.Task; 
                    if (rpvo.Status == EOperationStatus.Succeed)
                    {
                        //更新成功
                        string packageVersion = rpvo.PackageVersion;
                        Log.Warning($"Request package Version : {packageVersion}");
                    }
                    else
                    {
                        //更新失败
                        Log.Warning(rpvo.Error);
                    }
                    
                    var upmo = package.UpdatePackageManifestAsync(rpvo.PackageVersion);
                    await upmo.Task;
                    if (upmo.Status == EOperationStatus.Succeed)
                    {
                        //更新成功
                        Log.Warning("Update package manifest succeed!");
                    }
                    else
                    {
                        //更新失败
                        Log.Warning(upmo.Error);
                    }
                    
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return;

            string GetHostServerURL()
            {
                //string hostServerIP = "http://10.0.2.2"; //安卓模拟器地址
                // string hostServerIP = "http://weijinghot.weijinggame.com";
                string hostServerIP = "http://115.190.237.40:8080";
                string appVersion = "v1.0";
                
#if UNITY_EDITOR
                if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android)
                    return $"{hostServerIP}/weijing1/DLCBeta/MJ/Android";
                else if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.iOS)
                    return $"{hostServerIP}/weijing1/DLCBeta/MJ/iOS";
                else if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.WebGL)
                    return $"{hostServerIP}/weijing1/DLCBeta/MJ/WebGL";
                else
                    return $"{hostServerIP}/weijing1/DLCBeta/MJ/PC";
#else
		        if (Application.platform == RuntimePlatform.Android)
		        	return $"{hostServerIP}/weijing1/DLCBeta/MJ/Android";
		        else if (Application.platform == RuntimePlatform.IPhonePlayer)
		        	return $"{hostServerIP}/weijing1/DLCBeta/MJ/iOS";
		        else if (Application.platform == RuntimePlatform.WebGLPlayer)
		        	// return $"{hostServerIP}/weijing1/DLCBeta/MJ/WebGL";
                    return $"{Application.streamingAssetsPath}/yoo/DefaultPackage";
		        else
		        	return $"{hostServerIP}/weijing1/DLCBeta/MJ/PC";
#endif
            }
        }
        
        public void DestroyPackage(string packageName)
        {
            ResourcePackage package = YooAssets.GetPackage(packageName);
            package.UnloadUnusedAssetsAsync();
        }
        
        /// <summary>
        /// 主要用来加载dll config aotdll，因为这时候纤程还没创建，无法使用ResourcesLoaderComponent。
        /// 游戏中的资源应该使用ResourcesLoaderComponent来加载
        /// </summary>
        public  T LoadAssetSync<T>(string location) where T: UnityEngine.Object
        {
            AssetHandle handle = YooAssets.LoadAssetSync<T>(location);
            T t = (T)handle.AssetObject;
            handle.Release();
            return t;
        }

        /// <summary>
        /// 主要用来加载dll config aotdll，因为这时候纤程还没创建，无法使用ResourcesLoaderComponent。
        /// 游戏中的资源应该使用ResourcesLoaderComponent来加载
        /// </summary>
        public async ETTask<T> LoadAssetAsync<T>(string location) where T: UnityEngine.Object
        {
            AssetHandle handle = YooAssets.LoadAssetAsync<T>(location);
            await handle.Task;
            T t = (T)handle.AssetObject;
            handle.Release();
            return t;
        }
        
        /// <summary>
        /// 主要用来加载dll config aotdll，因为这时候纤程还没创建，无法使用ResourcesLoaderComponent。
        /// 游戏中的资源应该使用ResourcesLoaderComponent来加载
        /// </summary>
        public async ETTask<Dictionary<string, T>> LoadAllAssetsAsync<T>(string location) where T: UnityEngine.Object
        {
            AllAssetsHandle allAssetsOperationHandle = YooAssets.LoadAllAssetsAsync<T>(location);
            await allAssetsOperationHandle.Task;
            Dictionary<string, T> dictionary = new Dictionary<string, T>();
            foreach(UnityEngine.Object assetObj in allAssetsOperationHandle.AllAssetObjects)
            {    
                T t = assetObj as T;
                dictionary.Add(t.name, t);
            }
            allAssetsOperationHandle.Release();
            return dictionary;
        }
    }
}