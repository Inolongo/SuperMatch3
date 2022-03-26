using UI.ScreenSystem;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Screens
{
    public class TestView : ScreenBase
    {
        [SerializeField] private Button closeButtonTest;
        public override void OnShow()
        {
            closeButtonTest.onClick.AddListener(CloseView);
        }

        private void CloseView()
        {
            UISystem.Instance.Close<TestView>();
        }

        public override void OnHide()
        {
           
        }

        public override void OnClose()
        {
            closeButtonTest.onClick.RemoveListener(CloseView);
        }
        
        
    }
}