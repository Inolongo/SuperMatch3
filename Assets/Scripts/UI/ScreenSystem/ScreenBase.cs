using UI.AnimationSystem;
using UI.ScreenSystem.Screens;
using UnityEngine;

namespace UI.ScreenSystem
{
    [RequireComponent(typeof(ScreensAnimator))]
    public abstract class ScreenBase : UIView
    {
        public override IViewAnimator ViewAnimator { get; protected set; }
        
        public override void OnShown()
        {
            ViewAnimator = GetComponent<ScreensAnimator>();
        }
    }
}
