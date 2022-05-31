using Gayplay.Data;
using Gayplay.Gameplay;
using Gayplay.GayplayGrid;
using UI.DialogSystem;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.ScreenSystem.Screens
{
    public class GayplayScreen : ScreenBase
    {
        [SerializeField] private Button exitButton;
        [FormerlySerializedAs("gayplayController")] [SerializeField] private GameplayController gameplayController;
        [SerializeField] private MatchThreeGrid gridController;
        [SerializeField] private PiecesData levelData;
        public override void OnShown()
        {
            gameplayController.Initialize(gridController);
            gridController.Initialize(levelData);
            
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