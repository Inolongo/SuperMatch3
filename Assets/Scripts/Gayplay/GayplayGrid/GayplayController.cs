using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gayplay.Data;
using UnityEngine;

namespace Gayplay.GayplayGrid
{
    public class GayplayController : MonoBehaviour
    {
        private MatchThreeGrid _gridController;
        
        //TEST

        private void Awake()
        {
            var matchThreeGrid = FindObjectOfType<MatchThreeGrid>();
            _gridController = matchThreeGrid;
            _gridController.CellCreated += OnCellCreated;
        }

        //

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
            var firstCellCanMatch = _gridController.IsCanMatchHorizontal(firstCell.CellModel.CellPositionInGrid) ||
                                    _gridController.IsCanMatchVertical(firstCell.CellModel.CellPositionInGrid);
           
            var secondCellCanMatch = _gridController.IsCanMatchHorizontal(secondCell.CellModel.CellPositionInGrid) ||
                                    _gridController.IsCanMatchVertical(secondCell.CellModel.CellPositionInGrid);

            
            if (firstCellCanMatch || secondCellCanMatch)
            {
                // foreach (var rowColumnPair in matchedCells)
                // {
                //     var rowNum = rowColumnPair.rowNum;
                //     var gridRowCells = _gridController.GetRowByNum(rowNum);
                //     foreach (var cellController in gridRowCells)
                //     {
                //         if (cellController.CellModel.CellPositionInGrid.columnNum == rowColumnPair.columnNum)
                //         {
                //             cellController.DeleteCell();
                //         }
                //     }
                // }

                await Task.Delay(300);
                _gridController.DeleteMatchedCellsIfNeedAsync();
            }
            else
            {
                _gridController.SwipeCells(secondCell, firstCell, swipeDirection);
            }
        }


        private bool TryMatch(CellController semeCell, CellController ukeCell, out List<CellPositionInGrid> matchedCells)
        {
            matchedCells = new List<CellPositionInGrid>();
            matchedCells.AddRange(GetMatchedList(semeCell));
            matchedCells.AddRange(GetMatchedList(ukeCell));

            return matchedCells.Count > 1;
        }

        private List<CellPositionInGrid> GetMatchedList(CellController cell)
        {
            var matchedCells = new List<CellPositionInGrid>();
            var horizontalMatch = GetSameNearHorizontalCells(cell);
            if (horizontalMatch.Count >= 2)
            {
                matchedCells.AddRange(horizontalMatch);
            }

            var verticalMatch = GetSameNearVerticalCells(cell);
            if (verticalMatch.Count >= 2)
            {
                matchedCells.AddRange(verticalMatch);
            }

            if (matchedCells.Count > 0)
            {
                matchedCells.Add(cell.CellModel.CellPositionInGrid);
            }

            return matchedCells;
        }


        private List<CellPositionInGrid> GetSameNearHorizontalCells(CellController semeCell)
        {
            List<CellPositionInGrid> potancevalnyList = new();

            var rowNum = semeCell.CellModel.CellPositionInGrid.rowNum;
            var row = _gridController.GetRowByNum(rowNum);

            var columnCount = row[^1].CellModel.CellPositionInGrid.columnNum;
            var semeIndex = row.IndexOf(semeCell);


            for (int i = semeIndex + 1; i <= columnCount; i++)
            {
                if (i >= row.Count) break;

                if (row[i].CellModel.CellType == semeCell.CellModel.CellType)
                {
                    potancevalnyList.Add(row[i].CellModel.CellPositionInGrid);
                }
                else
                {
                    break;
                }
            }

            for (int i = semeIndex - 1; i >= 0; i--)
            {
                if (row[i].CellModel.CellType == semeCell.CellModel.CellType)
                {
                    potancevalnyList.Add(row[i].CellModel.CellPositionInGrid);
                }
                else
                {
                    break;
                }
            }

            return potancevalnyList;
        }


        private List<CellPositionInGrid> GetSameNearVerticalCells(CellController semeCell)
        {
            List<CellPositionInGrid> potancevalnyList = new();

            var columnNum = semeCell.CellModel.CellPositionInGrid.columnNum;
            var columnList = _gridController.GetColumn(columnNum);
            var semeIndex = columnList.IndexOf(semeCell);
            var rowCount = columnList[^1].CellModel.CellPositionInGrid.rowNum;

            for (int i = semeIndex + 1; i <= rowCount; i++)
            {
                if (i >= columnList.Count)
                {
                    break;
                }

                if (columnList[i].CellModel.CellType == semeCell.CellModel.CellType)
                {
                    potancevalnyList.Add(columnList[i].CellModel.CellPositionInGrid);
                }
                else
                {
                    break;
                }
            }


            for (int i = semeIndex - 1; i >= 0; i--)
            {
                if (columnList[i].CellModel.CellType == semeCell.CellModel.CellType)
                {
                    potancevalnyList.Add(columnList[i].CellModel.CellPositionInGrid);
                }
                else
                {
                    break;
                }
            }

            return potancevalnyList;
        }
    }
}