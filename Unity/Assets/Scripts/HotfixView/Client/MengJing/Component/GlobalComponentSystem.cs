using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(GlobalComponent))]
    public static partial class GlobalComponentSystem
    {
        [EntitySystem]
        private static void Awake(this GlobalComponent self)
        {
            // GlobalComponent.Instance = self;

            self.Global = GameObject.Find("/Global").transform;
            self.Unit = GameObject.Find("/Global/Unit").transform;
            self.UI = GameObject.Find("/Global/UI").transform;
            self.BloodRoot = GameObject.Find("/Global/UI/BloodRoot").transform;
            self.NormalRoot = GameObject.Find("/Global/UI/NormalRoot").transform;
            self.MidRoot = GameObject.Find("/Global/UI/MidRoot").transform;
            self.PopUpRoot = GameObject.Find("/Global/UI/PopUpRoot").transform;
            self.FixedRoot = GameObject.Find("/Global/UI/FixedRoot").transform;
            self.OtherRoot = GameObject.Find("/Global/UI/OtherRoot").transform;
            self.PoolRoot = GameObject.Find("/Global/PoolRoot").transform;
            self.MainCamera = GameObject.Find("/Global/MainCamera").GetComponent<Camera>();
            self.UICamera = GameObject.Find("/Global/UICamera").GetComponent<Camera>();
            self.GlobalConfig = Resources.Load<GlobalConfig>("GlobalConfig");

            self.BloodPlayer = new GameObject("BloodPlayer");
            self.BloodPlayer.AddComponent<RectTransform>();
            SetParent(self.BloodPlayer, self.BloodRoot.gameObject);
            self.BloodMonster = new GameObject("BloodMonster");
            self.BloodMonster.AddComponent<RectTransform>();
            SetParent(self.BloodMonster, self.BloodRoot.gameObject);

            self.BloodText = new GameObject("BloodText");
            self.BloodText.AddComponent<RectTransform>();
            SetParent(self.BloodText, self.BloodRoot.gameObject);
            self.BloodText_Layer0 = new GameObject("BloodText_Layer0");
            self.BloodText_Layer0.AddComponent<RectTransform>();
            SetParent(self.BloodText_Layer0, self.BloodText);
            self.BloodText_Layer1 = new GameObject("BloodText_Layer1");
            self.BloodText_Layer1.AddComponent<RectTransform>();
            SetParent(self.BloodText_Layer1, self.BloodText);
            self.BloodText_Layer2 = new GameObject("BloodText_Layer1");
            self.BloodText_Layer2.AddComponent<RectTransform>();
            SetParent(self.BloodText_Layer2, self.BloodText);

            CommonViewHelper.TargetFrameRate(60);
            self.SetQualitySettings();
            self.SetCamera();
            self.SetCanvas();
        }

        [EntitySystem]
        private static void Destroy(this GlobalComponent self)
        {
            UnityEngine.Object.DestroyImmediate(self.BloodPlayer);
            UnityEngine.Object.DestroyImmediate(self.BloodMonster);
            UnityEngine.Object.DestroyImmediate(self.BloodText);
            UnityEngine.Object.DestroyImmediate(self.BloodText_Layer0);
            UnityEngine.Object.DestroyImmediate(self.BloodText_Layer1);
            UnityEngine.Object.DestroyImmediate(self.BloodText_Layer2);
            self.BloodPlayer = null;
            self.BloodMonster = null;
            self.BloodText = null;
            self.BloodText_Layer0 = null;
            self.BloodText_Layer1 = null;
            self.BloodText_Layer2 = null;
        }

        private static void SetParent(GameObject son, GameObject parent)
        {
            if (son == null || parent == null)
                return;
            son.transform.SetParent(parent.transform);
            son.transform.localPosition = Vector3.zero;
            son.transform.localScale = Vector3.one;
        }

        private static void SetQualitySettings(this GlobalComponent self)
        {
            QualitySettings.vSyncCount = 0; // 关闭垂直同步，由TargetFrameRate控制帧率
            QualitySettings.antiAliasing = 0; // 关闭MSAA抗锯齿
            QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable; // 关闭各向异性过滤
            QualitySettings.shadows = UnityEngine.ShadowQuality.Disable; // 完全禁用阴影
            QualitySettings.shadowResolution = UnityEngine.ShadowResolution.Low; // 阴影分辨率最低
            QualitySettings.shadowDistance = 0f; // 阴影距离为0
            QualitySettings.shadowCascades = 0; // 关闭级联阴影
            QualitySettings.skinWeights = SkinWeights.TwoBones; // 骨骼蒙皮权重降低到2
            QualitySettings.pixelLightCount = 1; // 像素光源数量降低到1
            QualitySettings.realtimeReflectionProbes = false; // 关闭实时反射探针
            QualitySettings.billboardsFaceCameraPosition = false; // 广告牌不面向相机
            QualitySettings.softParticles = false; // 关闭软粒子
            QualitySettings.softVegetation = false; // 关闭软植被

            // LOD设置
            QualitySettings.lodBias = 0.5f; // 降低LOD细节级别
            QualitySettings.maximumLODLevel = 2; // 限制最高LOD等级

            // 粒子光线投射预算
            QualitySettings.particleRaycastBudget = 16; // 降低粒子光线投射预算
        }

        private static void SetCamera(this GlobalComponent self)
        {
            // 主摄像机设置
            self.MainCamera.useOcclusionCulling = true; // 保留遮挡剔除
            self.MainCamera.allowMSAA = false; // 禁用MSAA
            self.MainCamera.allowHDR = false; // 禁用HDR
            self.MainCamera.allowDynamicResolution = false; // 禁用动态分辨率（微信小游戏不支持）
            self.MainCamera.farClipPlane = 100f; // 降低远裁剪面（根据实际场景调整）
            self.MainCamera.nearClipPlane = 0.3f; // 近裁剪面（避免太小）

            // UniversalAdditionalCameraData mainCameraData = self.MainCamera.GetUniversalAdditionalCameraData();
            // mainCameraData.renderPostProcessing = false;
            // mainCameraData.renderShadows = false;
            // mainCameraData.requiresColorOption = CameraOverrideOption.Off;
            // mainCameraData.requiresDepthOption = CameraOverrideOption.Off;
            // mainCameraData.antialiasing = AntialiasingMode.None; // URP抗锯齿关闭
            // mainCameraData.renderType = CameraRenderType.Base; // 基础渲染类型

            // UI摄像机设置
            self.UICamera.useOcclusionCulling = false;
            self.UICamera.allowMSAA = false;
            self.UICamera.allowHDR = false;

            // UniversalAdditionalCameraData uiCameraData = self.UICamera.GetUniversalAdditionalCameraData();
            // uiCameraData.renderPostProcessing = false;
            // uiCameraData.renderShadows = false;
            // uiCameraData.requiresColorOption = CameraOverrideOption.Off;
            // uiCameraData.requiresDepthOption = CameraOverrideOption.Off;
            // uiCameraData.antialiasing = AntialiasingMode.None;
        }

        private static void SetCanvas(this GlobalComponent self)
        {
            Vector2 screenSize = new Vector2(1152, 2048);
            self.BloodRoot.GetComponent<CanvasScaler>().referenceResolution = screenSize;
            self.BloodRoot.GetComponent<CanvasScaler>().matchWidthOrHeight = 0.5f;
            self.NormalRoot.GetComponent<CanvasScaler>().referenceResolution = screenSize;
            self.NormalRoot.GetComponent<CanvasScaler>().matchWidthOrHeight = 0.5f;
            self.MidRoot.GetComponent<CanvasScaler>().referenceResolution = screenSize;
            self.MidRoot.GetComponent<CanvasScaler>().matchWidthOrHeight = 0.5f;
            self.FixedRoot.GetComponent<CanvasScaler>().referenceResolution = screenSize;
            self.FixedRoot.GetComponent<CanvasScaler>().matchWidthOrHeight = 0.5f;
            self.PopUpRoot.GetComponent<CanvasScaler>().referenceResolution = screenSize;
            self.PopUpRoot.GetComponent<CanvasScaler>().matchWidthOrHeight = 0.5f;
        }

        public static void SetMainCameraOcclusionCulling(this GlobalComponent self, bool enable)
        {
            if (self.MainCamera != null)
            {
                self.MainCamera.useOcclusionCulling = enable;
            }
        }
    }
}