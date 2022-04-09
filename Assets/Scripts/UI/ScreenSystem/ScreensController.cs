using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UI.ScreenSystem
{
    public class ScreensController : UIControllerBase
    {
        private const string DialogPrefabsPath = "Views";

        private List<ScreenBase> _screenPrefabs;
        private readonly List<UIView> _showingView = new List<UIView>();

        public override void Initialize(Transform rootViews)
        {
            base.Initialize(rootViews);

            _screenPrefabs = Resources.LoadAll<ScreenBase>(DialogPrefabsPath).ToList();
        }

        public override T Show<T>()
        {
            UIView viewToShow = null;
            foreach (var view in _screenPrefabs)
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

            if (_showingView.Count > 0)
            {
                var lastView = _showingView.Last();
                Hide<T>(lastView);
            }

            var uiView = (T) Instantiate(viewToShow, RootViews);
            uiView.OnShow();
            _showingView.Add(uiView);

            return uiView;
        }

        public override void Close<T>()
        {
            switch (_showingView.Count)
            {
                case 0:
                    throw new Exception("No opened view");
                case 1:
                    throw new Exception("Can't close single view");
            }

            if (_showingView.Count > 1)
            {
                var previousView = _showingView.Last();
                previousView.OnClose();
                Destroy(previousView.gameObject);

                _showingView.Remove(previousView);
            }

            var viewToShow = _showingView.Last();
            viewToShow.gameObject.SetActive(true);
            viewToShow.OnShow();
        }

        protected override T Hide<T>(UIView viewToHide)
        {
            viewToHide.gameObject.SetActive(false);
            viewToHide.OnHide();

            return viewToHide as T;
        }
    }
}