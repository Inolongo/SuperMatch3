using System;
using DG.Tweening;
using Gayplay.Data;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Gayplay.GayplayGrid
{
    public class CellController : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField] private CellView cellView;
        [SerializeField] private Ease func;

        public CellModel CellModel { get; private set; }

        private Vector2 _swipeStartPosition;
        private Vector2 _swipeEndPosition;
        public event Action<SwipeDirection, CellModel> CellSwiped;

        public void Init(CellModel cellDataModel)
        {
            CellModel = cellDataModel;
            cellView.Init(CellModel);
        }

        public void SmoothMove(Vector3 endPosition)
        {
            
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _swipeStartPosition = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _swipeEndPosition = eventData.position;

            var horizontalSwipe = _swipeEndPosition.x - _swipeStartPosition.x;
            var verticalSwipe = _swipeEndPosition.y - _swipeStartPosition.y;

            var swipeDuration = SwipeDirection.None;

            if (Math.Abs(verticalSwipe) - Math.Abs(horizontalSwipe) > 20)
            {
                switch (verticalSwipe)
                {
                    case > 0:
                        swipeDuration = SwipeDirection.Up;
                        break;
                    case < 0:
                        swipeDuration = SwipeDirection.Down;
                        break;
                }
            }

            if (Math.Abs(verticalSwipe) - Math.Abs(horizontalSwipe) < -20)
            {
                switch (horizontalSwipe)
                {
                    case < 0:
                        swipeDuration = SwipeDirection.Left;
                        break;
                    case > 0:
                        swipeDuration = SwipeDirection.Right;
                        break;
                }
            }
            
            CellSwiped?.Invoke(swipeDuration, CellModel);
        }
        
        public void OnDrag(PointerEventData eventData)
        {
        }

        public void DeleteCell()
        {
            transform.DOScale(Vector3.zero, 0.2f).SetEase(func);
        }
    }
}