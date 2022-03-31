using System;
using System.Collections;
using System.Collections.Generic;
using UI.Screens;
using UI.ScreenSystem;
using UnityEngine;
using UnityEngine.UI;

public class FooterNooter : MonoBehaviour
{
    // private TabButton _selectedTab;
    // private const float PageWidth = 245.7128f;
    // private int _lastPageIndex;
    // private int _newPageIndex;
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


    // public void OnTabSelected(TabButton button)
    // {
    //     _selectedTab = button;
    //     _newPageIndex = tabButtons.IndexOf(_selectedTab);
    //     ResetTabs();
    //     button.background.sprite = tabSelected;
    //
    //     Debug.Log("start position" + lobbyScreen.transform.position);
    //     lobbyScreen.transform.position = lobbyScreen.transform.position +
    //                                      (_newPageIndex - _lastPageIndex) * new Vector3(PageWidth,0, 0);
    //     Debug.Log("end position" + lobbyScreen.transform.position);
    //
    // }



}
