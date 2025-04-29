namespace QQ 
{
    public class UIMainScene : UI<UIMainScene>
    {
        public override UIType uiType => UIType.Main;

        public override UIDepth uiDepth => UIDepth.Fixed1;

        protected override void OnInit()
        {
            
        }

        protected override void OnStart()
        {
        }
        
        protected override void OnFocus()
        {
        }

        protected override void OnLostFocus()
        {
        }

        public void LoadGameScene()
        {
            LoadingSceneManager.LoadScene(LoadingSceneManager.gameSceneName);
        }
    }
}
