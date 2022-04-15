using UnityEngine;
using UnityEngine.UI;

namespace UI.DialogSystem
{
    public class SettingsDialog: DialogBase
    {
        [SerializeField] private Button exitButton;
        public override void OnShow()
        {
            exitButton.onClick.AddListener(Close);
        }

        public override void OnHide()
        {
            //UISystem.Instance.Close<SettingsDialog>();
        }

        public override void OnClose()
        {
            exitButton.onClick.RemoveListener(Close);
        }

        protected override void Close()
        {
            UISystem.Instance.Close<SettingsDialog>();
        }
    }
}