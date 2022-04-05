using System;
using UI.ScreenSystem.Screens;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Screens
{
    public class LobbyScreen : SwipeScreen
    {
        [SerializeField] private Button openViewTest;
        [SerializeField] private FooterNooter footer;
        [SerializeField] private LobbySwipe swipe;

        private void Awake()
        {
            InitializeFooter();
            InitializeLobbySwipe();
        }
        private void InitializeFooter()
        {
            footer.Initialize();
            footer.HomeButtonPressed += OnHomeButtonPressed;
            footer.ShopButtonPressed += OnShopButtonPressed;
            footer.RankingButtonPressed += OnRankingButtonPressed;
        }

        private void InitializeLobbySwipe()
        {
            swipe.Initialize();
        }

        private void OnShopButtonPressed()
        {
            swipe.MoveToPage(LobbyPageType.Shop);
        }

        private void OnRankingButtonPressed()
        {
            swipe.MoveToPage(LobbyPageType.Ranking);
        }

        private void OnHomeButtonPressed()
        {
            swipe.MoveToPage(LobbyPageType.Home);
        }

        public override void OnShow()
        {
            base.OnShow();


            openViewTest.onClick.AddListener(OnTestButtonClick);
        }

        private void OnTestButtonClick()
        {
            UISystem.Instance.Show<GayplayScreen>();
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