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
            
            bgmSource = gameObject.AddComponent<AudioSource>();
            uiSource = gameObject.AddComponent<AudioSource>();
            sfxSource = gameObject.AddComponent<AudioSource>();

            soundDictionary = new Dictionary<string, AudioClip>();

            // �ʱ�ȭ
            InitAudioSource(SoundType.BGM, bgmSource);
            InitAudioSource(SoundType.UI, uiSource);
            InitAudioSource(SoundType.SFX, sfxSource);

            //TODO: sounddata���̺� ���� string�� �����ͼ� �о�ߵ�
            //LoadSound().Forget();

            var sound = ResManager.soundLoad("Jump");
            var sound2 = ResManager.soundLoad("ButtonClick");
            soundDictionary.Add("Jump", sound);
            soundDictionary.Add("ButtonClick", sound2);
        }

        private void InitAudioSource(SoundType type, AudioSource source)
        {
            // ���Ұ�
            source.mute = false;

            // ����� ����Ʈ�� �����ϰ� ���� ����� ���
            source.bypassEffects = false;
            // AudioListener�� ����Ʈ(ex: 3D ���� ����) ����
            source.bypassListenerEffects = false;
            // ������ �� ȿ���� ����
            source.bypassReverbZones = false;
            source.playOnAwake = false;

            // �ݺ�
            source.loop = false;

            // ���� ����
            source.priority = SetPriority(type);
            
            source.volume = 1.0f;
            source.pitch = 1.0f;

            // ������� �¿��� ��������� �Ҹ��� ���ϰ� �������� �����ϴ� ��
            source.panStereo = 0.0f;
            // ������� 2D(���׷���)�� 3D(���� ����)���̿��� ��� ����Ǵ����� ��
            // 0.0f = 2D, 1.0f = 3D, 0.5f = 2D�� 3D�� ���̺긮��
            source.spatialBlend = 0.0f;
            // ������� ������ ����ȿ���� �󸶳� ���� ������
            // 0.0f = ���� �ȵ�(���� �Ҹ�), 1.0f = �����(���� ���ϰ�), 1.1f = �ش�ȭ
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
                Debug.LogWarning($"Sound '{soundName}' not found!");
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
                Debug.LogWarning($"Sound '{soundName}' not found!");
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
                Debug.LogWarning($"Sound '{soundName}' not found!");
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