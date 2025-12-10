using System;
using Spine.Unity;
using UnityEngine;

namespace ET
{
    public enum AnimName
    {
        None,
        Idle,
        Run,
        Attack,
        Skill,
        Death
    }

    public class SpineAnimator : MonoBehaviour
    {
        public SkeletonAnimation SkeletonAnimation;

        [Header("开始动画")]
        public AnimName StartAnim = AnimName.None;

        [Header("动画")]
        public AnimationReferenceAsset Idle;

        public AnimationReferenceAsset Run;
        public AnimationReferenceAsset Attack;
        public AnimationReferenceAsset Skill;
        public AnimationReferenceAsset Death;

        private AnimName CurrentAnim;

        private void Start()
        {
            Play(StartAnim, true);
        }

        public void Play(AnimName animName, bool loop, bool fromStart = false)
        {
            if (this.SkeletonAnimation == null)
            {
                Debug.LogError("SkeletonAnimation is null");
                return;
            }

            if (!fromStart && this.CurrentAnim != AnimName.None && this.CurrentAnim == animName)
            {
                return;
            }

            AnimationReferenceAsset playAnim = null;
            switch (animName)
            {
                case AnimName.Idle:
                    playAnim = this.Idle;
                    break;
                case AnimName.Run:
                    playAnim = this.Run;
                    break;
                case AnimName.Attack:
                    playAnim = this.Attack;
                    break;
                case AnimName.Death:
                    playAnim = this.Death;
                    break;
            }

            if (playAnim == null)
            {
                Debug.LogWarning("playAnim is null");
                return;
            }

            this.SkeletonAnimation.AnimationState.SetAnimation(0, playAnim, loop);
            this.CurrentAnim = animName;
        }
    }
}