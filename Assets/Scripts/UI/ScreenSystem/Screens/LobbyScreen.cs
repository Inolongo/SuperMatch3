using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Screens
{
    public class LobbyScreen: SwipeScreen
    {

        [SerializeField] private Button openViewTest;

        public override void OnShow()
        {
            base.OnShow();
            
            openViewTest.onClick.AddListener(OnTestButtonClick);
        }

        private void OnTestButtonClick()
        {
            UISystem.Instance.Show<TestView>();
        }

        public override void OnClose()
        {
            base.OnClose();
            openViewTest.onClick.RemoveListener(OnTestButtonClick);
        }

        public override void OnHide()
        {
            base.OnHide();
            openViewTest.onClick.RemoveListener(OnTestButtonClick);
        }
    }
}