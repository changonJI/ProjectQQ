using System.Collections.Generic;
using UnityEngine;

namespace QQ
{
    public class SoundManager : DontDestroySingleton<SoundManager>
    {
        [SerializeField] private AudioSource bgmSource;
        [SerializeField] private AudioSource uiSource;
        [SerializeField] private AudioSource sfxSource;
        private Dictionary<string, AudioClip> soundDictionary;

        private const int bgmPriority = 0;
        private const int uiPriority = 128;
        private const int sfxPriority = 256;

        protected override void Awake()
        {
            base.Awake();
        }

        public void Init()
        {
            bgmSource = gameObject.AddComponent<AudioSource>();
            uiSource = gameObject.AddComponent<AudioSource>();
            sfxSource = gameObject.AddComponent<AudioSource>();

            soundDictionary = new Dictionary<string, AudioClip>();

            // 초기화
            InitAudioSource(SoundType.BGM, bgmSource);
            InitAudioSource(SoundType.UI, uiSource);
            InitAudioSource(SoundType.SFX, sfxSource);

            //TODO: sounddata테이블 에서 string값 가져와서 읽어야됨
            //LoadSound().Forget();

            var sound = ResManager.LoadResource<AudioClip>(ResType.Sound, "Jump");
            var sound2 = ResManager.LoadResource<AudioClip>(ResType.Sound, "ButtonClick");

            soundDictionary.Add("Jump", sound);
            soundDictionary.Add("ButtonClick", sound2);
        }

        private void InitAudioSource(SoundType type, AudioSource source)
        {
            // 음소거
            source.mute = false;

            // 오디오 이펙트를 무시하고 원본 오디오 출력
            source.bypassEffects = false;
            // AudioListener의 이펙트(ex: 3D 공간 음향) 무시
            source.bypassListenerEffects = false;
            // 리버브 존 효과를 무시
            source.bypassReverbZones = false;
            source.playOnAwake = false;

            // 반복
            source.loop = false;

            // 실행 순서
            source.priority = SetPriority(type);
            
            source.volume = 1.0f;
            source.pitch = 1.0f;

            // 오디오의 좌우중 어느쪽으로 소리를 강하게 내보낼지 결정하는 값
            source.panStereo = 0.0f;
            // 오디오가 2D(스테레오)와 3D(공간 음향)사이에서 어떻게 재생되는지의 값
            // 0.0f = 2D, 1.0f = 3D, 0.5f = 2D와 3D의 하이브리드
            source.spatialBlend = 0.0f;
            // 오디오가 공간의 잔향효과를 얼마나 적용 받을지
            // 0.0f = 적용 안됨(원본 소리), 1.0f = 적용됨(잔향 강하게), 1.1f = 극대화
            source.reverbZoneMix = 1.0f;
        }


        public void PlayBGM(string soundName)
        {
            if (soundDictionary.ContainsKey(soundName))
            {
                AudioClip clip = soundDictionary[soundName];

                bgmSource.clip = clip;
                bgmSource.loop = true;
                bgmSource.Play();
            }
            else
            {
                LogHelper.LogWarning($"Sound '{soundName}' not found!");
            }
        }
        public void PlaySFX(string soundName)
        {
            if (soundDictionary.ContainsKey(soundName))
            {
                AudioClip clip = soundDictionary[soundName];
                sfxSource.PlayOneShot(clip);
            }
            else
            {
                LogHelper.LogWarning($"Sound '{soundName}' not found!");
            }
        }

        public void PlayUI(string soundName)
        {
            if (soundDictionary.ContainsKey(soundName))
            {
                AudioClip clip = soundDictionary[soundName];
                uiSource.PlayOneShot(clip);
            }
            else
            {
                LogHelper.LogWarning($"Sound '{soundName}' not found!");
            }
        }


        private int SetPriority(SoundType type)
        {
            if (type == SoundType.BGM)
            {
                return bgmPriority;
            }
            else if (type == SoundType.UI)
            {
                return uiPriority;
            }
            else if (type == SoundType.SFX)
            {
                return sfxPriority;
            }
            else
                return 0;
        }
    }
}