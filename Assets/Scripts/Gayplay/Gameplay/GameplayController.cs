using System;
using System.Threading.Tasks;
using Gayplay.Gameplay.Cell;
using Gayplay.GayplayGrid;
using UnityEngine;

namespace Gayplay.Gameplay
{
    public class GameplayController : MonoBehaviour
    {
        private MatchThreeGrid _gridController;
        
     
        //TODO:Remove after tests
        private void Awake()
        {
            var matchThreeGrid = FindObjectOfType<MatchThreeGrid>();
            _gridController = matchThreeGrid;
            _gridController.CellCreated += OnCellCreated;
        }
        
        public void Initialize(MatchThreeGrid gridController)
        {
            _gridController = gridController;
            _gridController.CellCreated += OnCellCreated;
        }
        
        private void OnCellCreated(CellController cell)
        {
            cell.CellSwiped += OnCellSwiped;
        }

        private void OnCellSwiped(SwipeDirection swipeDirection, CellModel cellModel)
        {
            var firstCellPosition = cellModel.CellPositionInGrid;
            CellPositionInGrid secondCellPosition;
            switch (swipeDirection)
            {
                case SwipeDirection.None:
                    throw new Exception("Duration can't be None");
                case SwipeDirection.Up:
                    secondCellPosition =
                        new CellPositionInGrid(firstCellPosition.rowNum - 1, firstCellPosition.columnNum);
                    break;
                case SwipeDirection.Down:
                    secondCellPosition =
                        new CellPositionInGrid(firstCellPosition.rowNum + 1, firstCellPosition.columnNum);
                    break;
                case SwipeDirection.Left:
                    secondCellPosition =
                        new CellPositionInGrid(firstCellPosition.rowNum, firstCellPosition.columnNum - 1);
                    break;
                case SwipeDirection.Right:
                    secondCellPosition =
                        new CellPositionInGrid(firstCellPosition.rowNum, firstCellPosition.columnNum + 1);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(swipeDirection), swipeDirection, null);
            }

            var firstCell = _gridController.GetCell(cellModel.CellPositionInGrid);
            var secondCell = _gridController.GetCell(secondCellPosition);


            _gridController.SwipeCells(firstCell, secondCell, swipeDirection,
                () => OnCellSwipeAnimationCompleted(firstCell, secondCell, swipeDirection));
        }

        private async void OnCellSwipeAnimationCompleted(CellController firstCell, CellController secondCell, SwipeDirection swipeDirection)
        {
            if (firstCell.Model is not CellModel firstCellCellModel) return;
            if (secondCell.Model is not CellModel secondCellCellModel) return;

            var firstCellCanMatch = _gridController.IsCanMatchHorizontal(firstCellCellModel.CellPositionInGrid) ||
                                    _gridController.IsCanMatchVertical(firstCellCellModel.CellPositionInGrid);
           
            var secondCellCanMatch = _gridController.IsCanMatchHorizontal(secondCellCellModel.CellPositionInGrid) ||
                                    _gridController.IsCanMatchVertical(secondCellCellModel.CellPositionInGrid);

            
            if (firstCellCanMatch || secondCellCanMatch)
            {
                await Task.Delay(300);
                await _gridController.DeleteMatchedCellsIfNeedAsync();
            }
            else
            {
                _gridController.SwipeCells(secondCell, firstCell, swipeDirection);
            }
        }
    }
}