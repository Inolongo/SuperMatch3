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
            var rowNum = cellModel.CellPositionInGrid.RowNum;
            var columnNum= cellModel.CellPositionInGrid.ColumnNum;
            if (!_gridController.TryGetCell(rowNum, columnNum, out var semeCell))
            {
                throw new Exception("Can't find cell by row num = " + rowNum + "column num = " + columnNum);
            }

            int ukeRowNum;
            int ukeColumnNum;
            var backwordSwipeDirection = SwipeDirection.None;

            switch (swipeDirection)
            {
                case SwipeDirection.Up:
                {
                    ukeRowNum = rowNum - 1;
                    ukeColumnNum = columnNum;
                    backwordSwipeDirection = SwipeDirection.Down;
                    break;
                }
                case SwipeDirection.Down:
                {
                    ukeRowNum = rowNum + 1;
                    ukeColumnNum = columnNum;
                    backwordSwipeDirection = SwipeDirection.Up;
                    break;
                }
                case SwipeDirection.Left:
                {
                    ukeRowNum = rowNum;
                    ukeColumnNum = columnNum - 1;
                    backwordSwipeDirection = SwipeDirection.Right;
                    break;
                }
                case SwipeDirection.Right:
                {
                    ukeRowNum = rowNum;
                    ukeColumnNum = columnNum + 1;
                    backwordSwipeDirection = SwipeDirection.Left;
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(swipeDirection), swipeDirection, null);
            }
            
            
            if (_gridController.TryGetCell(ukeRowNum, ukeColumnNum, out var ukeCell))
            {
                _gridController.SwipeCells(semeCell, ukeCell, swipeDirection,
                    () => OnCellSwipeAnimationCompleted(semeCell, ukeCell, backwordSwipeDirection));
            }
            else
            {
                throw new Exception("Can't find cell by row num = " + rowNum + "column num = " + columnNum);
            }
        }

        private async void OnCellSwipeAnimationCompleted(CellController semeCell, CellController ukeCell, SwipeDirection swipeDirection)
        {
            if (TryMatch(semeCell, ukeCell, out var matchedCells))
            {
                foreach (var rowColumnPair in matchedCells)
                {
                    var rowNum = rowColumnPair.RowNum;
                    var gridRowCells = _gridController.GetRowByNum(rowNum);
                    foreach (var cellController in gridRowCells)
                    {
                        if (cellController.CellModel.CellPositionInGrid.ColumnNum == rowColumnPair.ColumnNum)
                        {
                            cellController.DeleteCell();
                        }
                    }
                }

                await Task.Delay(300);
                _gridController.DeleteMatchedCellsIfNeedAsync();
            }
            else
            {
                _gridController.SwipeCells(ukeCell, semeCell, swipeDirection);
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

            var rowNum = semeCell.CellModel.CellPositionInGrid.RowNum;
            var row = _gridController.GetRowByNum(rowNum);

            var columnCount = row[^1].CellModel.CellPositionInGrid.ColumnNum;
            var semeIndex = row.IndexOf(semeCell);


            for (int i = semeIndex + 1; i <= columnCount; i++)
            {
                if (i >= row.Count) break;

                if (row[i].CellModel.CellType == semeCell.CellType)
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
                if (row[i].CellModel.CellType == semeCell.CellType)
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

            var columnNum = semeCell.CellModel.CellPositionInGrid.ColumnNum;
            var columnList = _gridController.GetColumn(columnNum);
            var semeIndex = columnList.IndexOf(semeCell);
            var rowCount = columnList[^1].CellModel.CellPositionInGrid.RowNum;

            for (int i = semeIndex + 1; i <= rowCount; i++)
            {
                if (i >= columnList.Count)
                {
                    break;
                }

                if (columnList[i].CellModel.CellType == semeCell.CellType)
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
                if (columnList[i].CellModel.CellType == semeCell.CellType)
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