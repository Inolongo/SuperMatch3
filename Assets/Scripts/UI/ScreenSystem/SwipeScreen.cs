namespace UI.ScreenSystem
{
    public class SwipeScreen: ScreenBase
    {
        public override void OnShow()
        {
        }

        public override void OnHide()
        {
        }

        public override void OnClose()
        {
        }

        protected override void Close()
        {
            UISystem.Instance.Close<SwipeScreen>();
        }
    }
}