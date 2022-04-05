namespace UI.DialogSystem
{
    public abstract class DialogBase<T> : UIView where T : UIView
    {
        protected override void Close()
        {
            UISystem.Instance.Close<T>();
        }
    }
}