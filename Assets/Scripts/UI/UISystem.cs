using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UI
{
    public class UISystem : Singleton<UISystem>
    {
        private const string ViewPrefabsPath = "Views";

        [SerializeField] private Transform rootViews;

        private UIView[] _prefabs;

        private readonly List<UIView> _views = new List<UIView>();

        public void Initialize()
        {
            _prefabs = Resources.LoadAll<UIView>(ViewPrefabsPath);
        }

        public T Show<T>() where T : UIView
        {
            UIView viewToShow = null;
            foreach (var view in _prefabs)
            {
                if (view is T)
                {
                    viewToShow = view;
                }
            }

            if (viewToShow is null)
            {
                throw new Exception("Need add prefab type " + typeof(T) + "add in folder " + ViewPrefabsPath);
            }

            if (_views.Count > 0)
            {
                var lastView = _views.Last();
                Hide(lastView);
            }

            var uiView = (T)Instantiate(viewToShow, rootViews);
            uiView.OnShow();
            _views.Add(uiView);

            return uiView;
        }

        public void Close<T>() where T : UIView
        {
            switch (_views.Count)
            {
                case 0:
                    throw new Exception("No opened view");
                case 1:
                    throw new Exception("Can't close single view");
            }

            if (_views.Count > 1)
            {
                var previousView = _views.Last();
                previousView.OnClose();
                Destroy(previousView.gameObject);

                _views.Remove(previousView);
            }

            var viewToShow = _views.Last();
            viewToShow.gameObject.SetActive(true);
            viewToShow.OnShow();
        }

        private UIView Hide(UIView lastView)
        {
            lastView.gameObject.SetActive(false);
            lastView.OnHide();

            return lastView;
        }
    }
}