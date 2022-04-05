using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class FooterNooter : MonoBehaviour
    {
        [SerializeField] private Button homeButton;
        [SerializeField] private Button rankingButton;
        [SerializeField] private Button shopButton;

        public event Action HomeButtonPressed;
        public event Action RankingButtonPressed;
        public event Action ShopButtonPressed;

        public void Initialize()
        {
            homeButton.onClick.AddListener(OnHomeButtonClick);
            rankingButton.onClick.AddListener(OnRankingButtonClick);
            shopButton.onClick.AddListener(OnShopButtonClick);
        }

        private void OnShopButtonClick()
        {
            ShopButtonPressed?.Invoke();
        }

        private void OnRankingButtonClick()
        {
            RankingButtonPressed?.Invoke();
        }

        private void OnHomeButtonClick()
        {
            HomeButtonPressed?.Invoke();
        }
    }
}