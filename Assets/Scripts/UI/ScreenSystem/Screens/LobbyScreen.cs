using Leaderboard;
using ServerEmulator;
using UnityEngine;
using UnityEngine.UI;

namespace UI.ScreenSystem.Screens
{
    public class LobbyScreen : SwipeScreen
    {
        [SerializeField] private Button openViewTest;
        [SerializeField] private FooterNooter footer;
        [SerializeField] private LobbySwipe swipe;
        [SerializeField] private LeaderboardController leaderboardController;

        private APIController _apiController;

        private void InitializeLeaderboard()
        {
            leaderboardController.Init(_apiController);
        }

        private void OnHomeButtonPressed()
        {
            swipe.MoveToPage(LobbyPageType.Home);
        }

        public override void OnShow()
        {
            base.OnShow();

            InitializeFooter();
            InitializeLobbySwipe();
            InitializeLeaderboard();

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

        public void Init(APIController apiController)
        {
            _apiController = apiController;
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
    }
}