using Cysharp.Text;
using UnityEngine;

namespace ET.Client
{
    [Event(SceneType.Demo)]
    public class Skill_OnSkillSound : AEvent<Scene, SkillSound>
    {
        protected override async ETTask Run(Scene scene, SkillSound args)
        {
            scene.GetComponent<SoundComponent>().PlayClip(args.Asset, "mp3").Coroutine();
            await ETTask.CompletedTask;
        }
    }

    [FriendOf(typeof(SoundComponent))]
    [EntitySystemOf(typeof(SoundComponent))]
    public static partial class SoundComponentSystem
    {
        [EntitySystem]
        private static void Awake(this SoundComponent self)
        {
            self.Root = self.Root().GetComponent<GlobalComponent>().PoolRoot;
            self.SoundClips.Clear();
            self.MusicClips.Clear();
            self.LoadingList.Clear();

            self.InitMusicVolume();
        }

        [EntitySystem]
        private static void Destroy(this SoundComponent self)
        {
            self.DisposeAll();
        }

        private static void InitMusicVolume(this SoundComponent self)
        {
            string music = PlayerPrefsHelper.GetString(PlayerPrefsHelper.MusicVolume);
            string sound = PlayerPrefsHelper.GetString(PlayerPrefsHelper.SoundVolume);
            if (string.IsNullOrEmpty(music))
            {
                self.MusicVolume = 1f;
            }
            else
            {
                self.MusicVolume = float.Parse(music);
            }

            if (string.IsNullOrEmpty(sound))
            {
                self.SoundVolume = 1f;
            }
            else
            {
                self.SoundVolume = float.Parse(sound);
            }
        }

        private static string GetAudioOggPath(this SoundComponent self, string fileName)
        {
            return ZString.Format("Assets/Bundles/Audio/{0}.ogg", fileName);
        }

        private static string GetAudioPath(this SoundComponent self, string fileName)
        {
            return ZString.Format("Assets/Bundles/Audio/{0}.mp3", fileName);
        }

        /// <summary>
        /// 短暂的声音和特效
        /// 无法暂停
        /// 异步加载音效
        /// </summary>
        public static async ETTask PlayClip(this SoundComponent self, string clipName, string musicType, float volume = 0.5f)
        {
            if (!SettingData.PlaySound || self.SoundVolume <= 0f)
            {
                return;
            }

            GameObject gameObject = null;
            for (int i = 0; i < self.SoundClips.Count; i++)
            {
                if (self.SoundClips[i].name != clipName)
                {
                    continue;
                }

                bool isplaying = self.SoundClips[i].GetComponent<AudioSource>().isPlaying;
                if (isplaying)
                {
                    return;
                }
                else
                {
                    gameObject = self.SoundClips[i];
                    break;
                }
            }

            if (gameObject != null)
            {
                gameObject.GetComponent<AudioSource>().volume = volume * self.SoundVolume;
                gameObject.GetComponent<AudioSource>().Play();
                return;
            }

            if (!self.LoadingList.Contains(clipName))
            {
                self.LoadingList.Add(clipName);
                gameObject = new GameObject(clipName);
                string assetPath = musicType == "ogg" ? self.GetAudioOggPath(clipName) : self.GetAudioPath(clipName);

                AudioClip audioClip = await self.Root().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<AudioClip>(assetPath);
                if (gameObject == null)
                {
                    return;
                }

                self.AssetList.Add(assetPath);
                self.LoadingList.Remove(clipName);
                AudioSource audio = gameObject.AddComponent<AudioSource>();
                gameObject.transform.SetParent(self.Root);
                self.SoundClips.Add(gameObject);
                audio.clip = audioClip;
                gameObject.GetComponent<AudioSource>().volume = volume * self.SoundVolume;
                gameObject.GetComponent<AudioSource>().Play();
            }
        }

        /// <summary>
        /// 背景音效
        /// </summary>
        public static void ChangeSoundVolume(this SoundComponent self, float volume)
        {
            self.SoundVolume = volume;
            for (int i = 0; i < self.SoundClips.Count; i++)
            {
                self.SoundClips[i].GetComponent<AudioSource>().volume = volume;
            }

            PlayerPrefsHelper.SetString(PlayerPrefsHelper.SoundVolume, volume.ToString());
        }

        /// <summary>
        /// 音乐
        /// </summary>
        public static void ChangeMusicVolume(this SoundComponent self, float volume)
        {
            self.MusicVolume = volume;
            for (int i = 0; i < self.MusicClips.Count; i++)
            {
                self.MusicClips[i].audio.volume = volume;
            }

            PlayerPrefsHelper.SetString(PlayerPrefsHelper.MusicVolume, volume.ToString());
        }

        public static void PlayBgmSound(this SoundComponent self, MapType mapType, int sceneId, int sonsceneid)
        {
            self.DisposeAll();

            // string music = "MainCity";
            // switch (mapType)
            // {
            //     case MapType.Login:
            //         music = "LoginBack";
            //         break;
            //     case MapType.MainCity:
            //         music = "MainCity";
            //         break;
            //     default:
            //         music = "Fight_1";
            //         break;
            // }
            //
            // if (music != "")
            // {
            //     self.PlayMusic(music).Coroutine();
            // }
        }

        public static async ETTask PlayMusic(this SoundComponent self, string clipName, float volume = 0.5f)
        {
            if (!SettingData.PlaySound || self.SoundVolume <= 0f)
            {
                return;
            }

            string assetpath = ABPathHelper.GetSoundPath(clipName);
            self.AssetList.Add(assetpath);
            GameObject bundleGameObject = await self.Root().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<GameObject>(assetpath);
            GameObject prefab = UnityEngine.Object.Instantiate(bundleGameObject, self.Root, true);
            SoundData soundData = prefab.GetComponent<SoundData>();

            self.MusicClips.Add(soundData);
            soundData.audio.volume = volume * self.MusicVolume;
            soundData.audio.loop = true;
            soundData.audio.Play();
        }

        /// <summary>
        /// 销毁所有声音
        /// </summary>
        public static void DisposeAll(this SoundComponent self)
        {
            self.LoadingList.Clear();

            for (int i = 0; i < self.SoundClips.Count; i++)
            {
                UnityEngine.Object.DestroyImmediate(self.SoundClips[i]);
            }

            self.SoundClips.Clear();

            for (int i = 0; i < self.MusicClips.Count; i++)
            {
                self.MusicClips[i].Dispose();
            }

            self.MusicClips.Clear();

            ResourcesLoaderComponent resourcesLoaderComponent = self.Root().GetComponent<ResourcesLoaderComponent>();
            for (int i = 0; i < self.AssetList.Count; i++)
            {
                resourcesLoaderComponent.UnLoadAsset(self.AssetList[i]);
            }

            self.AssetList.Clear();
        }
    }
}