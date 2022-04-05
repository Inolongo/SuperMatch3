using UI.ScreenSystem;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Screens
{
    public class GayplayScreen : ScreenBase<GayplayScreen>
    {
        [SerializeField] private Button closeButtonTest;
        public override void OnShow()
        {
            closeButtonTest.onClick.AddListener(Close);
        }
        

        public override void OnHide()
        {
           
        }

        public override void OnClose()
        {
            closeButtonTest.onClick.RemoveListener(Close);
        }
        
        
    }
}