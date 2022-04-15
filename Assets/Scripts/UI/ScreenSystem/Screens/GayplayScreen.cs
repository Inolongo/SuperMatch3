﻿using UnityEngine;
using UnityEngine.UI;

namespace UI.ScreenSystem.Screens
{
    public class GayplayScreen : ScreenBase
    {
        [SerializeField] private Button exitButton;

        public override void OnShow()
        {
            exitButton.onClick.AddListener(Close);
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