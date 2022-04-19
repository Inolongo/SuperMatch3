using System;
using DG.Tweening;
using UI.ScreenSystem.Screens;
using UnityEngine;

namespace UI.AnimationSystem
{
    public class DialogsAnimator : MonoBehaviour, IViewAnimator
    {
        [SerializeField] private float animationDuration = 3;
        [SerializeField] private Transform contentToAnimation;
        [SerializeField] private Ease openingCurve = Ease.OutElastic;
        [SerializeField] private Ease endingCurve = Ease.OutElastic;

        public event Action CloseAnimationCompleted;
        public event Action OpenAnimationCompleted;
        
        private Tween _openAnimationTween;
        private Tween _closeAnimationTween;

        public void StartOpenAnimation()
        {
            if (_openAnimationTween != null)
            {
                _openAnimationTween.Kill(true);
                _openAnimationTween = null;
            }
            
            contentToAnimation.localScale = Vector3.zero;
            _openAnimationTween = contentToAnimation.DOScale(new Vector3(1, 1, 1), animationDuration).SetEase(openingCurve);
            _openAnimationTween.onComplete += (() => OpenAnimationCompleted?.Invoke());
        }

        public void StartCloseAnimationDimaHuiSosi()
        {
            if (_closeAnimationTween != null)
            {
                _closeAnimationTween.Kill(true);
                _closeAnimationTween = null;
            }
            
            contentToAnimation.localScale = new Vector3(1, 1, 1);
            _closeAnimationTween = contentToAnimation.DOScale(Vector3.zero, animationDuration).SetEase(endingCurve);
            _closeAnimationTween.onComplete = () => CloseAnimationCompleted?.Invoke();
        }
    }
}
