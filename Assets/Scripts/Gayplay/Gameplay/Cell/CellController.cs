using System;
using DG.Tweening;
using MVC;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Gayplay.Gameplay.Cell
{
    public class CellController : MonoBehaviour, IController, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField] private CellView cellView;
        [SerializeField] private Ease func;

        public IModel CellModel => _cellModel;
        public event Action<SwipeDirection, CellModel> CellSwiped;

        private CellModel _cellModel;
        private Vector2 _swipeStartPosition;
        private Vector2 _swipeEndPosition;
        
        #if UNITY_EDITOR
        private void OnValidate()
        {
            cellView ??= FindObjectOfType<CellView>();
        }
        #endif

        public void Initialize(IModel model)
        {
            if (model is not CellModel cellModel)
            {
                throw new Exception("CellController.cs only work with CellModel.cs");
            }
            
            _cellModel = cellModel;
            cellView.Initialize(CellModel);
        }
        
        public void DeleteCell()
        {
            transform.DOScale(Vector3.zero, 0.2f).SetEase(func);
        }

        #region Drag

        public void OnBeginDrag(PointerEventData eventData)
        {
            _swipeStartPosition = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _swipeEndPosition = eventData.position;

            var horizontalSwipeDistance = _swipeEndPosition.x - _swipeStartPosition.x;
            var verticalSwipeDistance = _swipeEndPosition.y - _swipeStartPosition.y;

            var swipeDuration = GetSwipeDuration(verticalSwipeDistance, horizontalSwipeDistance);

            if (swipeDuration != SwipeDirection.None)
            {
                CellSwiped?.Invoke(swipeDuration, _cellModel);
            }
        }

        private SwipeDirection GetSwipeDuration(float verticalSwipeDistance, float horizontalSwipeDistance)
        {
            if (Math.Abs(verticalSwipeDistance) - Math.Abs(horizontalSwipeDistance) > 20)
            {
                switch (verticalSwipeDistance)
                {
                    case > 0:
                        return SwipeDirection.Up;
                    case < 0:
                        return SwipeDirection.Down;
                }
            }

            if (Math.Abs(verticalSwipeDistance) - Math.Abs(horizontalSwipeDistance) < -20)
            {
                switch (horizontalSwipeDistance)
                {
                    case < 0:
                        return SwipeDirection.Left;
                    case > 0:
                        return SwipeDirection.Right;
                }
            }

            return SwipeDirection.None;
        }
        
        public void OnDrag(PointerEventData eventData)
        {
        }

        #endregion
    }
}