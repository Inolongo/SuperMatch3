using Leaderboard;
using ServerEmulator;
using UI.DialogSystem;
using UnityEngine;
using UnityEngine.UI;

namespace UI.ScreenSystem.Screens
{
    public class LobbyScreen : SwipeScreen
    {
        [SerializeField] private Button startButton;
        [SerializeField] private Button settingsButton;
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

        public override void OnShown()
        {
            base.OnShown();

            InitializeFooter();
            InitializeLobbySwipe();
            InitializeLeaderboard();

            startButton.onClick.AddListener(OnStartButtonClick);
            settingsButton.onClick.AddListener(OnSettingsButtonClick);
        }

        private void OnSettingsButtonClick()
        {
            UISystem.Instance.Show<SettingsDialog>();
        }

        private void OnStartButtonClick()
        {
            UISystem.Instance.Show<GayplayScreen>();
        }

        public override void OnClosed()
        {
            base.OnClosed();
            startButton.onClick.RemoveListener(OnStartButtonClick);
            settingsButton.onClick.RemoveListener(OnSettingsButtonClick);
        }

        public override void OnHidden()
        {
            base.OnHidden();
            startButton.onClick.RemoveListener(OnStartButtonClick);
            settingsButton.onClick.RemoveListener(OnSettingsButtonClick);

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