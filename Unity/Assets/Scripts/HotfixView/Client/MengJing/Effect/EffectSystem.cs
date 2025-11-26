using System;
using Unity.Mathematics;
using UnityEngine;
using Spine.Unity;

namespace ET.Client
{
    [EntitySystemOf(typeof(Effect))]
    [FriendOf(typeof(Effect))]
    public static partial class EffectSystem
    {
        [EntitySystem]
        private static void Awake(this Effect self)
        {
        }

        [EntitySystem]
        private static void Destroy(this Effect self)
        {
            self.OnFinished();
        }

        public static void OnInit(this Effect self, InitEffectData initEffectData, Unit theUnitBelongTo)
        {
            self.EffectPath = string.Empty;
            self.EffectObj = null;
            self.InitEffectData = initEffectData;
            self.EffectState = EffectState.Running;
            self.TheUnitBelongTo = theUnitBelongTo;
            self.EffectConfig = EffectConfigCategory.Instance.Get(initEffectData.EffectId);
            self.ElapsedTime = 0;
            self.EffectAngle = -10000;
        }

        public static void OnUpdate(this Effect self, float deltaTime)
        {
            self.ElapsedTime += deltaTime;

            if (self.ElapsedTime < self.EffectConfig.SkillEffectDelayTime)
            {
                return;
            }

            if (string.IsNullOrEmpty(self.EffectPath))
            {
                self.PlayEffect();
            }

            if (self.ElapsedTime > self.EffectConfig.SkillEffectLiveTime)
            {
                self.EffectState = EffectState.Finished;
                return;
            }

            if (self.TheUnitBelongTo == null || self.TheUnitBelongTo.IsDisposed || self.InitEffectData.InstanceId == 0)
            {
                self.EffectState = EffectState.Finished;
                return;
            }
        }

        public static void OnFinished(this Effect self)
        {
            self.Root().GetComponent<GameObjectLoadComponent>().RecoverGameObject(self.EffectPath, self.EffectObj);
            self.EffectState = EffectState.Finished;
            self.TheUnitBelongTo = null;
            self.EffectObj = null;
            self.EffectPath = string.Empty;
            self.ElapsedTime = 0;
        }

        private static void OnLoadGameObject(this Effect self, GameObject gameObject, long instanceId)
        {
            if (self.EffectState != EffectState.Running || instanceId != self.InstanceId)
            {
                if (gameObject != null)
                {
                    UnityEngine.Object.DestroyImmediate(gameObject);
                }

                return;
            }

            if (self.InitEffectData.InstanceId == 0 || gameObject == null)
            {
                self.EffectState = EffectState.Finished;
            }

            if (self.TheUnitBelongTo == null || self.TheUnitBelongTo.IsDisposed)
            {
                self.EffectState = EffectState.Finished;
            }

            if (self.EffectState == EffectState.Finished)
            {
                return;
            }

            self.EffectObj = gameObject;
            self.EffectObj.name = self.EffectConfig.EffectName;
            int skillParentID = self.EffectConfig.SkillParent;
            GlobalComponent globalComponent = self.Root().GetComponent<GlobalComponent>();
            switch (skillParentID)
            {
                //固定位置
                case 0:
                {
                    self.EffectObj.transform.SetParent(globalComponent.Unit);

                    float angle = self.InitEffectData.EffectAngle != 0 ? self.InitEffectData.EffectAngle : self.InitEffectData.TargetAngle;
                    if (self.Root().GetComponent<GlobalComponent>().ViewMode == 0)
                    {
                        self.EffectObj.transform.position = new Vector3(self.InitEffectData.EffectPosition.x, self.InitEffectData.EffectPosition.y, self.InitEffectData.EffectPosition.y);
                    }
                    else
                    {
                        self.EffectObj.transform.position = new Vector3(self.InitEffectData.EffectPosition.x, self.InitEffectData.EffectPosition.y, 0);
                    }
                    // self.EffectObj.transform.localRotation = Quaternion.Euler(0, 0, angle);

                    self.EffectObj.transform.localScale = Vector3.one;
                    break;
                }
                //跟随玩家
                case 1:
                {
                    UnitBoneComponent unitBoneComponent = self.TheUnitBelongTo.GetComponent<UnitBoneComponent>();
                    if (unitBoneComponent == null)
                    {
                        self.EffectState = EffectState.Finished;
                        return;
                    }

                    Transform tParent = unitBoneComponent.GetTransform(self.EffectConfig.SkillParentPosition);
                    if (tParent == null)
                    {
                        self.EffectState = EffectState.Finished;
                        return;
                    }

                    self.EffectObj.transform.SetParent(tParent);
                    self.EffectObj.transform.localPosition = Vector3.zero;
                    self.EffectObj.transform.localScale = Vector3.one;
                    self.EffectObj.transform.localRotation = Quaternion.Euler(0, 0, 0);
                    break;
                }
                //实时跟随玩家位置,但是不跟随旋转
                case 2:
                {
                    self.EffectObj.transform.SetParent(globalComponent.Unit);
                    if (self.Root().GetComponent<GlobalComponent>().ViewMode == 0)
                    {
                        self.EffectObj.transform.position = new Vector3(self.TheUnitBelongTo.Position.x, self.TheUnitBelongTo.Position.y, self.TheUnitBelongTo.Position.y);
                    }
                    else
                    {
                        self.EffectObj.transform.position = new Vector3(self.TheUnitBelongTo.Position.x, self.TheUnitBelongTo.Position.y, 0);
                    }
                    
                    self.EffectObj.transform.localScale = Vector3.one;
                    self.EffectObj.transform.localRotation = Quaternion.Euler(0, 0, self.InitEffectData.TargetAngle);
                    break;
                }
            }

            self.EffectObj.GetComponentInChildren<SkeletonAnimation>()?.AnimationState.SetAnimation(0, "animation", true);
            self.EffectObj.SetActive(true);

#if UNITY_EDITOR
            self.AddCollider();
#endif
        }

        private static void AddCollider(this Effect self)
        {
            if (self.InitEffectData.Radius > 0)
            {
                if (self.EffectObj.GetComponent<CircleCollider2D>() == null)
                {
                    self.EffectObj.AddComponent<CircleCollider2D>();
                }
                
                self.EffectObj.GetComponent<CircleCollider2D>().radius = self.InitEffectData.Radius;
                self.EffectObj.GetComponent<CircleCollider2D>().isTrigger = true;
            }
        }

        public static void PlayEffect(this Effect self)
        {
            if (self.InitEffectData.InstanceId == 0)
            {
                return;
            }

            self.EffectPath = ABPathHelper.GetSkillEffectPath(self.EffectConfig.EffectName);
            self.Root().GetComponent<GameObjectLoadComponent>().AddLoadQueue(self.EffectPath, self.InstanceId, true, self.OnLoadGameObject);
        }

        public static void UpdateEffectPosition(this Effect self, float3 vec3, float angle)
        {
            if (self.EffectObj == null)
            {
                if (self.Root().GetComponent<GlobalComponent>().ViewMode == 0)
                {
                    self.EffectPosition = new Vector3(vec3.x, vec3.y, vec3.y);
                }
                else
                {
                    self.EffectPosition = new Vector3(vec3.x, vec3.y, 0);
                }

                self.EffectAngle = angle;
                return;
            }

            if (!Mathf.Approximately(angle, -1))
            {
                self.EffectObj.transform.rotation = Quaternion.Euler(0, 0, angle);
            }

            self.EffectObj.transform.position = vec3;
        }
    }
}