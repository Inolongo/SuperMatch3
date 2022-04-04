using System.Collections;
using DG.Tweening;
using UI.Screens;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.ScreenSystem.Screens
{
    public class LobbySwipe : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
    {
        [SerializeField] private int pageCount;
        [SerializeField] private float swipeDuration;

        private RectTransform _rectTransform;
        private float _pageWidth;
        private LobbyPageType _currentPage;
        private float _swipeStartPositionX;
        private Vector3 _lastPagePosition;
        private Tween _moveToPageTween;
        private float _distanceBetweenRectAndDragX;
        

        public void Initialize()
        {
            _rectTransform = GetComponent<RectTransform>();
            _pageWidth = _rectTransform.rect.width / pageCount;
            _currentPage = LobbyPageType.Home;
            _lastPagePosition = _rectTransform.position;
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
        }

        private void MoveLeft(int pagePlaceDifference, LobbyPageType page)
        {
            if ((int) _currentPage > 1)
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
            if ((int) _currentPage < pageCount)
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
            
            _moveToPageTween = _rectTransform.DOMove(endPosition, swipeDuration);
            _moveToPageTween.onComplete += OnMoveToPageComplete;
        }

        private void OnMoveToPageComplete()
        {
            _lastPagePosition = _rectTransform.position;
        }
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            _swipeStartPositionX = eventData.position.x;
            _distanceBetweenRectAndDragX =  _swipeStartPositionX-_rectTransform.position.x;
        }

        public void OnDrag(PointerEventData eventData)
        {
            _rectTransform.position = Vector3.Lerp(_rectTransform.position,
                new Vector3(eventData.position.x - _distanceBetweenRectAndDragX,
                    _rectTransform.position.y, _rectTransform.position.z), swipeDuration);

        }

        public void OnEndDrag(PointerEventData eventData)
        {
            var isSwipeToLeft = eventData.position.x - _swipeStartPositionX > 0;
            Debug.Log(eventData.delta);
            if (isSwipeToLeft)
            {
                MoveToPage(_currentPage - 1);
            }
            else
            {
               MoveToPage(_currentPage + 1);
            }
        }
    }
}