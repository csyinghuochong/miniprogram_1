using System.Collections.Generic;
using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class SoundComponent : Entity, IAwake, IDestroy
    {
        //根物体
        public Transform Root;

        //所有音效
        public List<GameObject> SoundClips = new();

        //所有音乐
        public List<SoundData> MusicClips = new();
        public List<string> LoadingList = new();
        public List<string> AssetList = new();

        public float MusicVolume = 1f;
        public float SoundVolume = 1f;
    }
}