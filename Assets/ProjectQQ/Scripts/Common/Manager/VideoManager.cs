using QQ;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Video;

namespace ProjectQQ
{
    public class VideoManager : DontDestroySingleton<VideoManager>
    {
        public VideoPlayer videoPlayer;
        private Camera mainCam;
        public override void Init() 
        {
            mainCam = Camera.main;

            videoPlayer = transform.AddComponent<VideoPlayer>();
            videoPlayer.renderMode = VideoRenderMode.CameraNearPlane;
            videoPlayer.targetCamera = mainCam;
            videoPlayer.targetCameraAlpha = 1.0f;
        }

        public void PlayViedo(string fileName)
        {
            if (videoPlayer == null) return;

            if(mainCam == null)
                mainCam = Camera.main;
            
            if (UIRoot.IsValid())
                UIRoot.Instance.SetActive(false);

            // Video Clip을 Resources 폴더에 넣었을 경우
            videoPlayer.clip = ResManager.LoadResource<VideoClip>(ResType.Animation, fileName);

            videoPlayer.enabled = true;

            // 재생 준비 및 시작
            videoPlayer.Prepare();
            videoPlayer.prepareCompleted += OnPrepared;

            // 영상 종료 시 호출될 메서드 등록
            videoPlayer.loopPointReached += OnVideoFinished;
        }

        void OnPrepared(VideoPlayer source)
        {
            source.Play();
        }

        // 영상이 끝났을 때 실행되는 콜백
        void OnVideoFinished(VideoPlayer source)
        {
            if (UIRoot.IsValid())
                UIRoot.Instance.SetActive(true);

            source.Stop();
            source.enabled = false;
        }


    }
}
