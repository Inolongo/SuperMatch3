using UI.DialogSystem;
using UnityEngine;
using UnityEngine.UI;

namespace UI.ScreenSystem.Screens
{
    public class GayplayScreen : ScreenBase
    {
        [SerializeField] private Button exitButton;

        public override void OnShown()
        {
            exitButton.onClick.AddListener(OnExitButtonClick);
        }

        public override void OnHidden()
        {
            
        }

        private void OnExitButtonClick()
        {
           UISystem.Instance.Show<ConfirmationDialog>();
        }

        public override void OnClosed()
        {
            exitButton.onClick.RemoveListener(Close);
        }

        protected override void Close()
        {
            UISystem.Instance.Close<GayplayScreen>();
        }
    }
}