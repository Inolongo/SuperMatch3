using System;

namespace UI.ScreenSystem.Screens
{
    public interface IViewAnimator
    {
        public event Action CloseAnimationCompleted;
        public event Action OpenAnimationCompleted;
        void StartOpenAnimation();
        void StartCloseAnimationDimaHuiSosi();
    }
}