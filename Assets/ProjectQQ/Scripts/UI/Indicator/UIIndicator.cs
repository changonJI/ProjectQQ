namespace QQ {
    public class UIIndicator : UI<UIIndicator>
    {
        public override UIType uiType => UIType.Back;

        public override UIDepth uiDepth => UIDepth.Indicator;

        protected override void OnFocus() {}

        protected override void OnInit() {}

        protected override void OnLostFocus() {}

        protected override void OnStart() {}

        protected override void OnExit() {}
    }
}
