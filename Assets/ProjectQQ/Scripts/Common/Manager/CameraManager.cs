using Unity.Cinemachine;
using UnityEngine;

namespace QQ {
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private CinemachineBrain brainCam;
        [SerializeField] private CinemachineCamera[] childCam;
        private OutputChannels outputChannels = 0;

#if UNITY_EDITOR
        private void OnValidate()
        {
            SetBrainCamera();
        }
#endif
        
        public void Init()
        {
            // 씬이 로드될 때 카메라를 설정합니다.
            SetBrainCamera();
        }

        private void SetBrainCamera()
        {
            if (brainCam == null) return;

            // 초기화면은 전체 맵
            outputChannels = OutputChannels.Default;

            brainCam.ShowDebugText = false;
            brainCam.ShowCameraFrustum = true;
            brainCam.IgnoreTimeScale = true;    // Time.timeScale이 0일 때도 카메라 업데이트
            brainCam.WorldUpOverride = null;
            brainCam.ChannelMask = outputChannels;
            brainCam.UpdateMethod = CinemachineBrain.UpdateMethods.FixedUpdate;
            brainCam.BlendUpdateMethod = CinemachineBrain.BrainUpdateMethods.LateUpdate;    // NOTE:물리 기반 오브젝트 싱크 안맞으면 Fixed로 변경
            brainCam.LensModeOverride.Enabled = true;
            brainCam.LensModeOverride.DefaultMode = LensSettings.OverrideModes.Orthographic;
            brainCam.DefaultBlend = new CinemachineBlendDefinition
            {
                Style = CinemachineBlendDefinition.Styles.EaseInOut,
                Time = 2f
            };
            brainCam.CustomBlends = null;
        }

        public void SetCameraTarget(CameraType type, Transform target)
        {
            if (brainCam == null) return;

            SetCameraChannel(type);

            int childCamIndex = (int)type;
            
            childCam[childCamIndex].Follow = target;
            childCam[childCamIndex].LookAt = target;
        }

        private void SetCameraChannel(CameraType type)
        {
            if (brainCam == null) return;

            outputChannels = GetCameraChannel(type);
            brainCam.ChannelMask = outputChannels;
        }

        private OutputChannels GetCameraChannel(CameraType type)
        {
            switch (type)
            {
                case CameraType.Default:
                    return OutputChannels.Default;
                case CameraType.Player:
                    return OutputChannels.Channel01;
                case CameraType.Boss:
                    return OutputChannels.Channel02;
                default:
                    // 모든 채널 활성화
                    return (OutputChannels)(-1);
            }
        }
    }
}