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

        public CellType CellType => CellDataModel.CellType;

        public CellDataModel CellDataModel { get; private set; }

        private Vector2 _swipeStartPosition;
        private Vector2 _swipeEndPosition;

        public Action<SwipeDirection, int, int> CellSwiped;

        public void Init(CellDataModel cellDataModel, RectTransform decoyRectTransform)
        {
            CellDataModel = cellDataModel;
            cellView.Init(cellDataModel, decoyRectTransform);
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
            Debug.Log("verticalSwipe" + verticalSwipe);
            Debug.Log("horizontalSwipe" + horizontalSwipe);
            if (Math.Abs(verticalSwipe) - Math.Abs(horizontalSwipe) > 20)
            {
                switch (verticalSwipe)
                {
                    case > 0:
                        CellSwiped?.Invoke(SwipeDirection.Up, CellDataModel.RowColumnPair.RowNum,
                            CellDataModel.RowColumnPair.ColumnNum);
                        break;
                    case < 0:
                        CellSwiped?.Invoke(SwipeDirection.Down, CellDataModel.RowColumnPair.RowNum,
                            CellDataModel.RowColumnPair.ColumnNum);
                        break;
                }
            }

            if (Math.Abs(verticalSwipe) - Math.Abs(horizontalSwipe) < -20)
            {
                switch (horizontalSwipe)
                {
                    case < 0:
                        CellSwiped?.Invoke(SwipeDirection.Left, CellDataModel.RowColumnPair.RowNum,
                            CellDataModel.RowColumnPair.ColumnNum);
                        break;
                    case > 0:
                        CellSwiped?.Invoke(SwipeDirection.Right, CellDataModel.RowColumnPair.RowNum,
                            CellDataModel.RowColumnPair.ColumnNum);
                        break;
                }
            }
        }

        public void SwipeCell(Enum swipeDirection)
        {
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