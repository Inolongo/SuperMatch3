using UI.ScreenSystem.Screens;
using UnityEngine;
using UnityEngine.UI;

namespace UI.DialogSystem
{
    public class ConfirmationDialog: DialogBase
    {
        [SerializeField] private Button closeButton;
        [SerializeField] private Button noButton;
        [SerializeField] private Button yesButton;
        public override void OnShow()
        {
            closeButton.onClick.AddListener(OnNoButtonClick);
            noButton.onClick.AddListener(OnNoButtonClick);
            yesButton.onClick.AddListener(OnExitButtonClick);
        }

        private void OnNoButtonClick()
        {
            UISystem.Instance.Close<ConfirmationDialog>();
        }

        private void OnExitButtonClick()
        {
            UISystem.Instance.Close<ConfirmationDialog>();
            UISystem.Instance.Close<GayplayScreen>();
        }

        public override void OnHide()
        {
            
        }

        public override void OnClose()
        {
            closeButton.onClick.RemoveListener(OnExitButtonClick);
            yesButton.onClick.RemoveListener(OnExitButtonClick);
            noButton.onClick.RemoveListener(OnNoButtonClick);
        }

        protected override void Close()
        {
            UISystem.Instance.Close<ConfirmationDialog>();
        }
    }
}