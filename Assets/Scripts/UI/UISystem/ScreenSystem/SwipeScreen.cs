namespace UI.ScreenSystem
{
    public class SwipeScreen: ScreenBase
    {
        public override void OnShown()
        {
        }

        public override void OnHidden()
        {
            
        }

        public override void OnClosed()
        {
        }

        protected override void Close()
        {
            UISystem.Instance.Close<SwipeScreen>();
        }
    }
}