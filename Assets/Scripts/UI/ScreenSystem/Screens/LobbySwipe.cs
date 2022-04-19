using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.ScreenSystem.Screens
{
    [RequireComponent(typeof(LobbySwipeSizeFitter))]
    public class LobbySwipe : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
    {
        [SerializeField] private int pageCount;
        [SerializeField] private float swipeDuration;
        [SerializeField] private float swipeBlock;
        [SerializeField] private RectTransform screenRectTransform;
        public int PageCount => pageCount;

        private RectTransform _lobbySwipeRectTransform;
        private float _pageWidth;
        private LobbyPageType _currentPage;
        private float _swipeStartPositionX;
        private Vector3 _lastPagePosition;
        private Tween _moveToPageTween;
        private float _distanceBetweenRectAndDragX;
        private LobbySwipeSizeFitter _lobbySwipeSizeFitter;

        public void Initialize()
        {
            _lobbySwipeSizeFitter = GetComponent<LobbySwipeSizeFitter>();

            _lobbySwipeRectTransform = GetComponent<RectTransform>();
            _lobbySwipeSizeFitter.SetSwiperSize(screenRectTransform, _lobbySwipeRectTransform, PageCount);
            _pageWidth = _lobbySwipeRectTransform.rect.width / pageCount;
            _currentPage = LobbyPageType.Home;
            _lastPagePosition = _lobbySwipeRectTransform.position;
        }

        public void MoveToPage(LobbyPageType page)
        {
            var pagePlaceDifference = page - _currentPage;
            var isMoveRight = page > _currentPage;
            var isMoveLeft = page < _currentPage;

            if (isMoveRight)
            {
                MoveRight(pagePlaceDifference, page);
            }
            else if (isMoveLeft)
            {
                MoveLeft(pagePlaceDifference, page);
            }
            else
            {
                _moveToPageTween = _lobbySwipeRectTransform.DOLocalMove(_lastPagePosition, swipeDuration);
            }
        }

        private void MoveLeft(int pagePlaceDifference, LobbyPageType page)
        {
            if ((int)_currentPage > 1)
            {
                Move(pagePlaceDifference);
                _currentPage = page;
            }
            else
            {
                Move(0);
            }
        }

        private void MoveRight(int pagePlaceDifference, LobbyPageType page)
        {
            if ((int)_currentPage < pageCount)
            {
                Move(pagePlaceDifference);
                _currentPage = page;
            }
            else
            {
                Move(0);
            }
        }

        private void Move(int pagePlaceDifference)
        {
            var endPosition = new Vector3(_lastPagePosition.x - pagePlaceDifference * _pageWidth,
                _lastPagePosition.y, _lastPagePosition.z);

            if (_moveToPageTween != null)
            {
                _moveToPageTween.Kill();
                _moveToPageTween = null;
            }

            _moveToPageTween = _lobbySwipeRectTransform.DOLocalMove(endPosition, swipeDuration);
            _moveToPageTween.onComplete += OnMoveToPageComplete;
        }

        private void OnMoveToPageComplete()
        {
            _lastPagePosition = _lobbySwipeRectTransform.position;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _swipeStartPositionX = eventData.position.x;
            _distanceBetweenRectAndDragX = _swipeStartPositionX - _lobbySwipeRectTransform.localPosition.x;
        }

        public void OnDrag(PointerEventData eventData)
        {
            _lobbySwipeRectTransform.localPosition = Vector3.Lerp(_lobbySwipeRectTransform.localPosition,
                new Vector3(eventData.position.x - _distanceBetweenRectAndDragX,
                    _lobbySwipeRectTransform.localPosition.y, _lobbySwipeRectTransform.localPosition.z), swipeDuration);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            var positionDelta = eventData.position.x - _swipeStartPositionX;
            var isSwipeToLeft = positionDelta > 0;
            if (Mathf.Abs(positionDelta) > swipeBlock)
            {
                if (isSwipeToLeft)
                {
                    MoveToPage(_currentPage - 1);
                }
                else
                {
                    MoveToPage(_currentPage + 1);
                }
            }
            else
            {
                MoveToPage(_currentPage);
            }
        }
    }
}