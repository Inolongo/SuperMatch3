using UnityEngine;

namespace UI.ScreenSystem.Screens
{
    [RequireComponent(typeof(RectTransform))]
    public class LobbySwipeSizeFitter : MonoBehaviour
    {
        public void SetSwiperSize(RectTransform screen,  RectTransform lobbySwipeRectTransform, int allPagesCount)
        {
            var screenRect = screen.rect;
            lobbySwipeRectTransform.offsetMax = new Vector2(screenRect.width * allPagesCount / 2, 0);
            lobbySwipeRectTransform.offsetMin = new Vector2(-screenRect.width * allPagesCount / 2, 0);
        }
    }
}