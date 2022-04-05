using UnityEngine;
using UnityEngine.UI;

namespace UI.ScreenSystem.Screens
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