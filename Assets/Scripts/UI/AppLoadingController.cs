using ServerEmulator;
using UI.ScreenSystem.Screens;
using UnityEngine;

namespace UI
{
    public class AppLoadingController : MonoBehaviour
    {
        [SerializeField] private UISystem uiSystem;

        private APIController _apiController;

        private void Awake()
        {
            _apiController = new APIController();
            uiSystem.Initialize();
            LoadApplication();
        }

        private void LoadApplication()
        {
            var lobbyScreen = uiSystem.Show<LobbyScreen>();
            lobbyScreen.Init(_apiController);
        }
    }
}