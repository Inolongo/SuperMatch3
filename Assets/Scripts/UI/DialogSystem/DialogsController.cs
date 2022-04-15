using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UI.DialogSystem
{
    public class DialogsController : UIControllerBase
    {
        private const string DialogPrefabsPath = "Views/Dialogs";
        
        private List<DialogBase> _dialogPrefabs;
        private readonly List<UIView> _showingView = new List<UIView>();

        public override void Initialize(Transform rootViews)
        {
            base.Initialize(rootViews);
            
            _dialogPrefabs = Resources.LoadAll<DialogBase>(DialogPrefabsPath).ToList();
        }

        public override T Show<T>(Action<UIView> beforeShown)
        {
            UIView viewToShow = null;
            foreach (var view in _dialogPrefabs)
            {
                if (view is T)
                {
                    viewToShow = view;
                }
            }

            if (viewToShow is null)
            {
                throw new Exception("Need add prefab type " + typeof(T) + "add in folder " + DialogPrefabsPath);
            }

            TryHideLastShownView();

            var uiView = (T) Instantiate(viewToShow, RootViews);
            beforeShown?.Invoke(uiView);
            uiView.OnShow();
            _showingView.Add(uiView);
            
            return uiView;
        }

        public override void Close<T>()
        {
            if (_showingView.Count == 0)
            {
                throw new Exception("No opened view");
            }
            
            var uiView = _showingView.Last();
            _showingView.Remove(uiView);
            uiView.OnClose();
            Destroy(uiView.gameObject);

            TryShowLastHiddenView();
        }

        private bool TryHideLastShownView()
        {
            if (_showingView.Count > 0)
            {
                var uiView = _showingView.Last();
                uiView.OnHide();
                uiView.gameObject.SetActive(false);

                return true;
            }

            return false;
        }

        private bool TryShowLastHiddenView()
        {
            if (_showingView.Count > 0)
            {
                var uiView = _showingView.Last();
                uiView.OnShow();
                uiView.gameObject.SetActive(true);
                
                return true;
            }

            return false;
        }

        protected override T Hide<T>(UIView viewToHide)
        {
            viewToHide.gameObject.SetActive(false);
            viewToHide.OnHide();
            
            return viewToHide as T;
        }
    }
}