using System;
using UnityEngine;

namespace UI
{
    public abstract class UIControllerBase : MonoBehaviour
    {
        protected Transform RootViews { get; private set; }

        public virtual void Initialize(Transform rootView)
        {
            RootViews = rootView;
        }
        public abstract T Show<T>(Action<UIView> action) where T : UIView;
        public abstract void Close<T>() where T : UIView;
        protected abstract T Hide<T>(UIView viewToHide) where T : UIView;
    }
}