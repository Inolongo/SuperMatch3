using UI.AnimationSystem;
using UI.ScreenSystem.Screens;
using UnityEngine;

namespace UI.DialogSystem
{
    [RequireComponent(typeof(DialogsAnimator))]
    public abstract class DialogBase : UIView
    {
        public override IViewAnimator ViewAnimator { get; protected set; }
        
        public override void OnShown()
        {
            ViewAnimator = GetComponent<DialogsAnimator>();
        }
        
        public override void OnHidden(){}
        public override void OnClosed(){}
    }
}