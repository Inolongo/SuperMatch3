using System;
using System.Collections.Generic;
using Gayplay.Data;
using UnityEngine;

namespace Gayplay.GayplayGrid
{
    public class GayplayController : MonoBehaviour
    {
        private GridController _gridController;

        public void Initialize(GridController gridController)
        {
            _gridController = gridController;
            _gridController.CellCreated += OnCellCreated;
        }
        
        private void OnCellCreated(CellController cell)
        {
            cell.CellSwiped += OnCellSwiped;
        }

        private void OnCellSwiped(SwipeDirection swipeDirection, int rowNum, int columnNum)
        {
            var semeCell = _gridController.GetCell(rowNum, columnNum);
            Debug.Log("OnCellSwiped dir " + swipeDirection);
            switch (swipeDirection)
            {
                case SwipeDirection.Up:
                {
                    var ukeCell = _gridController.GetCell(rowNum - 1, columnNum);
                    _gridController.SwipeCells(semeCell, ukeCell, OnCellSwipeAnimationCompleted);
                    break;
                }
                case SwipeDirection.Down:
                {
                    var ukeCell = _gridController.GetCell(rowNum + 1, columnNum);
                    _gridController.SwipeCells(semeCell, ukeCell, OnCellSwipeAnimationCompleted);
                    break;
                }
                case SwipeDirection.Left:
                {
                    var ukeCell = _gridController.GetCell(rowNum, columnNum - 1);
                    _gridController.SwipeCells(semeCell, ukeCell, OnCellSwipeAnimationCompleted);
                    break;
                }
                case SwipeDirection.Right:
                {
                    var ukeCell = _gridController.GetCell(rowNum, columnNum + 1);
                    _gridController.SwipeCells(semeCell, ukeCell, OnCellSwipeAnimationCompleted);
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(swipeDirection), swipeDirection, null);
            }
        }

        private void OnCellSwipeAnimationCompleted(CellController semeCell, CellController ukeCell)
        {
            if (TryMatch(semeCell, ukeCell, out var matchedCells))
            {
                foreach (var rowColumnPair in matchedCells)
                {
                    var rowNum = rowColumnPair.RowNum;
                    var gridRowCells = _gridController.GetRowByNum(rowNum);
                    foreach (var cellController in gridRowCells)
                    {
                        if (cellController.CellDataModel.RowColumnPair.ColumnNum == rowColumnPair.ColumnNum)
                        {
                            cellController.DeleteCell();
                            //ToDo: remove cell in array
                        }
                    }
                }
            }
            else
            {
                _gridController.SwipeCells(ukeCell, semeCell);
            }
        }


        private bool TryMatch(CellController semeCell, CellController ukeCell, out List<RowColumnPair> matchedCells)
        {
            matchedCells = new List<RowColumnPair>();
            matchedCells.AddRange(GetMatchedList(semeCell));
            matchedCells.AddRange(GetMatchedList(ukeCell));

            return matchedCells.Count > 1;
        }

        private List<RowColumnPair> GetMatchedList(CellController cell)
        {
            var matchedCells = new List<RowColumnPair>();
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
                matchedCells.Add(cell.CellDataModel.RowColumnPair);
            }

            return matchedCells;
        }


        private List<RowColumnPair> GetSameNearHorizontalCells(CellController semeCell)
        {
            List<RowColumnPair> potancevalnyList = new();

            var rowNum = semeCell.CellDataModel.RowColumnPair.RowNum;
            var row = _gridController.GetRowByNum(rowNum);

            var columnCount = row[row.Count - 1].CellDataModel.RowColumnPair.ColumnNum;
            var semeIndex = row.IndexOf(semeCell);


            for (int i = semeIndex + 1; i <= columnCount; i++)
            {
                if (i >= row.Count) break;

                if (row[i].CellDataModel.CellType == semeCell.CellType)
                {
                    potancevalnyList.Add(row[i].CellDataModel.RowColumnPair);
                }
                else
                {
                    break;
                }
            }

            for (int i = semeIndex - 1; i >= 0; i--)
            {
                if (row[i].CellDataModel.CellType == semeCell.CellType)
                {
                    potancevalnyList.Add(row[i].CellDataModel.RowColumnPair);
                }
                else
                {
                    break;
                }
            }

            return potancevalnyList;
        }


        private List<RowColumnPair> GetSameNearVerticalCells(CellController semeCell)
        {
            List<RowColumnPair> potancevalnyList = new();

            var columnNum = semeCell.CellDataModel.RowColumnPair.ColumnNum;
            var columnList = _gridController.GetColumnList(columnNum);
            var semeIndex = columnList.IndexOf(semeCell);
            var rowCount = columnList[columnList.Count - 1].CellDataModel.RowColumnPair.RowNum;

            for (int i = semeIndex + 1; i <= rowCount; i++)
            {
                if (i >= columnList.Count)
                {
                    break;
                }

                if (columnList[i].CellDataModel.CellType == semeCell.CellType)
                {
                    potancevalnyList.Add(columnList[i].CellDataModel.RowColumnPair);
                }
                else
                {
                    break;
                }
            }


            for (int i = semeIndex - 1; i >= 0; i--)
            {
                if (columnList[i].CellDataModel.CellType == semeCell.CellType)
                {
                    potancevalnyList.Add(columnList[i].CellDataModel.RowColumnPair);
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