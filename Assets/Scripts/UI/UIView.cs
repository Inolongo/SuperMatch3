using UnityEngine;

namespace UI
{
    public abstract class UIView : MonoBehaviour
    {
        public abstract void OnShow();
        public abstract void OnHide();
        public abstract void OnClose();
        protected abstract void Close();
    }
}