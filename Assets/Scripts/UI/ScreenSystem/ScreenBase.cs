namespace UI.ScreenSystem
{
    public abstract class ScreenBase<T> : UIView where T : UIView
    {
        protected override void Close()
        {
            UISystem.Instance.Close<T>();

        }
    }
}
