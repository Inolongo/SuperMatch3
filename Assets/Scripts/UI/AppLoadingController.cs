using System;
using UI.Screens;
using UnityEngine;

namespace UI
{
    public class AppLoadingController: MonoBehaviour
    {
        [SerializeField] private UISystem uiSystem;

        private void Awake()
        {
            uiSystem.Initialize();
            LoadApplication();
        }

        private void LoadApplication()
        {
            var lobbyScreen = uiSystem.Show<LobbyScreen>();
        }
    }
}