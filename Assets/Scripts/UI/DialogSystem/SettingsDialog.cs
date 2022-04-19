using UI.ScreenSystem.Screens;
using UnityEngine;
using UnityEngine.UI;

namespace UI.DialogSystem
{
    public class SettingsDialog: DialogBase
    {
        [SerializeField] private Button exitButton;
        
        public override void OnShown()
        {
            base.OnShown();
            exitButton.onClick.AddListener(Close);
        }

        public override void OnHidden()
        {
            base.OnHidden();
            //UISystem.Instance.Close<SettingsDialog>();
        }

        public override void OnClosed()
        {
            base.OnClosed();
            exitButton.onClick.RemoveListener(Close);
        }

        protected override void Close()
        {
            UISystem.Instance.Close<SettingsDialog>();
        }
    }
}