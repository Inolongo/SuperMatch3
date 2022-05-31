using UI.ScreenSystem.Screens;
using UnityEngine;

namespace UI
{
    //TODO:think about how you can reduce abstractions count
    public abstract class UIView : MonoBehaviour
    {
        public abstract IViewAnimator  ViewAnimator { get; protected set; }
        
        public abstract void OnShown();
        public abstract void OnHidden();
        public abstract void OnClosed();
        protected abstract void Close();
    }
}