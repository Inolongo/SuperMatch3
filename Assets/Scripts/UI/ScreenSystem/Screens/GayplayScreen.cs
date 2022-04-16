using UI.DialogSystem;
using UnityEngine;
using UnityEngine.UI;

namespace UI.ScreenSystem.Screens
{
    public class GayplayScreen : ScreenBase
    {
        [SerializeField] private Button exitButton;

        public override void OnShow()
        {
            exitButton.onClick.AddListener(OnExitButtonClick);
        }

        private void OnExitButtonClick()
        {
           UISystem.Instance.Show<ConfirmationDialog>();
        }


        public override void OnHide()
        {
        }

        public override void OnClose()
        {
            exitButton.onClick.RemoveListener(Close);
        }

        protected override void Close()
        {
            UISystem.Instance.Close<GayplayScreen>();
        }
    }
}