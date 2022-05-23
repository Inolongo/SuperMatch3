using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DG.Tweening;
using Gayplay.Data;
using UnityEngine;
using UnityEngine.UIElements;
using Random = System.Random;

namespace Gayplay.GayplayGrid
{
    public class MatchThreeGrid : MonoBehaviour
    {
        [SerializeField] private Vector2 spacing;
        [SerializeField] private RectTransform content;

        #region Test
        
        [SerializeField] private PiecesData levelDataTest;

        #endregion

        public event Action<CellController> CellCreated;
        
        private Vector2 _cellSize;
        private List<List<CellController>> _cellsRowsToColumns;

        private RectTransform _rectTransform;

        #region Editor

        public void DestroyCells()
        {
            if (content.childCount > 0)
            {
                var componentsInChildren = GetComponentsInChildren<CellController>();
                foreach (var componentsInChild in componentsInChildren)
                {
                    DestroyImmediate(componentsInChild.gameObject);
                }
            }   
        }

        public void CreatCellsTest()
        {
            Initialize(levelDataTest);
        }

        #endregion

        public void Initialize(PiecesData levelData)
        {
            var rowCount = levelData.IsActiveCellRows.Count;
            _cellSize = GetCellSize(levelData.IsActiveCellRows[0].Row.Count, rowCount);
            
            CreatCells(levelData);
            InitCellsPosition();
        }

        public void SwipeCells(CellController firstCell, CellController secondCell, SwipeDirection swipeDirection,
            Action onCellSwipeAnimationCompleted = null)
        {
            if (firstCell == null && secondCell == null)
            {
                throw new Exception("Can't move null pair cells");
            }

            Vector3 firstPosition;
            Vector3 secondPosition;
            if (firstCell != null)
            {
                firstPosition = firstCell.transform.localPosition;
            }
            else
            {
                var secondCellSize = secondCell.CellModel.Size;
                var secondCellPosition = secondCell.transform.localPosition;
                firstPosition = GetPositionToMove(swipeDirection, secondCellSize, secondCellPosition);
            }

            if (secondCell != null)
            {
                secondPosition = secondCell.transform.localPosition;
            }
            else
            {
                var firstCellSize = firstCell.CellModel.Size;
                var firstCellPosition = firstCell.transform.localPosition;
                secondPosition = GetPositionToMove(swipeDirection, firstCellSize, firstCellPosition);
            }

            var secondCellPositionInGrid = secondCell.CellModel.CellPositionInGrid;
            var firstCellPositionInGrid = firstCell.CellModel.CellPositionInGrid;

            firstCell.transform.DOLocalMove(secondPosition, 0.3f);

            Tween secondCellTween = secondCell.transform.DOLocalMove(firstPosition, 0.3f);
            secondCellTween.onComplete += () =>
            {
                secondCell.CellModel.CellPositionInGrid = firstCellPositionInGrid;
                SwapCells(firstCellPositionInGrid, secondCellPositionInGrid);
                onCellSwipeAnimationCompleted?.Invoke();
            };
        }

        public bool TryGetCell(CellPositionInGrid cellPositionInGrid, out CellController cellController)
        {
            var rowNum = cellPositionInGrid.rowNum;
            var columnNum = cellPositionInGrid.columnNum;
            
            if (_cellsRowsToColumns.Count < rowNum || _cellsRowsToColumns[0].Count < columnNum)
            {
                cellController = null;
                return false;
            }

            cellController = _cellsRowsToColumns[rowNum][columnNum];
            
            return true;
        }

        private void SwapCells(CellPositionInGrid newCellPosition, CellPositionInGrid oldCellPosition)
        {
            var firstCell = GetCell(oldCellPosition);
            var secondCell = GetCell(newCellPosition);
            // ReSharper disable once InlineTemporaryVariable
            var temp = secondCell;
            _cellsRowsToColumns[newCellPosition.rowNum][newCellPosition.columnNum] = firstCell;
            _cellsRowsToColumns[oldCellPosition.rowNum][oldCellPosition.columnNum] = temp;

            firstCell.CellModel.CellPositionInGrid = new CellPositionInGrid(newCellPosition);
            secondCell.CellModel.CellPositionInGrid = new CellPositionInGrid(oldCellPosition);
        }

        private void InitCellsPosition()
        {
            var cellInColumnCount = GetColumn(0).Count;
            
            var leftUpGridPosition = GetLeftUpPosition();
            
            for (var i = 0; i < cellInColumnCount; i++)
            {
                var rowCellsCount = _cellsRowsToColumns[i].Count;
                
                for (var j = 0; j < rowCellsCount; j++)
                {
                    var offsetX = j * (spacing.x + _cellSize.x);
                    var cellXPosition = leftUpGridPosition.x + spacing.x + _cellSize.x / 2 + offsetX;

                    var offsetY = i * (spacing.y + _cellSize.y);
                    var cellYPosition = leftUpGridPosition.y - (spacing.y + _cellSize.y / 2 + offsetY);

                    _cellsRowsToColumns[i][j].transform.localPosition =
                        new Vector3(cellXPosition, cellYPosition, content.transform.localPosition.z);
                }
            }
        }

        private Vector2 GetLeftUpPosition()
        {
            var contentRect = ((RectTransform)content.transform);
            var contentLocalPosition = contentRect.localPosition;
            
            var leftUpGridXPosition = contentLocalPosition.x - contentRect.rect.width / 2;
            var leftUpGridYPosition = contentLocalPosition.y + contentRect.rect.height / 2;
            
            var leftUpGridPosition = new Vector2(leftUpGridXPosition, leftUpGridYPosition);

            return leftUpGridPosition;
        }

        private Vector2 GetCellSize(int rowCellsCount, int columnCellsCount)
        {
            var contentRect = ((RectTransform) content.transform).rect;

            var allSpacingSizeX = (rowCellsCount + 1) * spacing.x;
            var sizeForAllCellsX = contentRect.width - allSpacingSizeX;
            var cellWidth = sizeForAllCellsX / rowCellsCount;
            
            var allSpacingSizeY = (columnCellsCount + 1) * spacing.y;
            var sizeForAllCellsY = contentRect.height - allSpacingSizeY;
            var cellHeight = sizeForAllCellsY / columnCellsCount;

            var cellSideSize = cellHeight > cellWidth ? cellWidth : cellHeight;

            if (cellSideSize <= 0)
            {
                throw new Exception("Cell side can't be less 0");
            }

            return new Vector2(cellSideSize, cellSideSize);
        }

        public List<CellController> GetColumn(int columnIndex)
        {
            if (columnIndex < 0)
            {
                throw new Exception("Column index can't be less than zero");
            }

            if (columnIndex > _cellsRowsToColumns.Count - 1)
            {
                throw new Exception("Column index can't be bigger row lenght");
            }
            
            var column = new List<CellController>(_cellsRowsToColumns.Count);

            foreach (var row in _cellsRowsToColumns)
            {
                column.Add(row[columnIndex]);
            }

            return column;
        }

        private void CreatCells(PiecesData levelData)
        {
            _cellsRowsToColumns = new List<List<CellController>>(levelData.IsActiveCellRows.Count);

            for (var i = 0; i < levelData.IsActiveCellRows.Count; i++)
            {
                var row = levelData.IsActiveCellRows[i];
                var cellsRow = new List<CellController>(row.Row.Count);

                for (var j = 0; j < row.Row.Count; j++)
                {
                    var isCellActive = row.Row[j];
                    
                    var cellModel = GetRandomCellModel(levelData, isCellActive);
                    cellModel.CellPositionInGrid = new CellPositionInGrid(i, j);
                    
                    var cellController = CreatCell(levelData.CellPrefab, cellModel);

                    cellsRow.Add(cellController);
                }

                _cellsRowsToColumns.Add(cellsRow);
            }
        }

        private CellModel GetRandomCellModel(PiecesData levelData, bool isCellActive)
        {
            var levelCellDataModel = GetRandomLevelDataModel(levelData);

            CellModel cellModel;
            if (isCellActive)
            {
                cellModel = new CellModel(false, levelCellDataModel.PieceIcon,
                    levelCellDataModel.PieceFace, levelCellDataModel.CellType, _cellSize);
            }
            else
            {
                cellModel = new CellModel(true, null, null, CellType.None, _cellSize);
            }

            return cellModel;
        }

        private CellController CreatCell(CellController cellPrefab, CellModel cellModel)
        {
            var cellController = Instantiate(cellPrefab, content);
            cellController.Init(cellModel);
            CellCreated?.Invoke(cellController);

            return cellController;
        }
        
        public CellController GetCell(CellPositionInGrid cellPositionInGrid)
        {
            if (_cellsRowsToColumns.Count < cellPositionInGrid.rowNum ||
                _cellsRowsToColumns[0].Count < cellPositionInGrid.columnNum)
            {
                return null;
            }
            
            return _cellsRowsToColumns[cellPositionInGrid.rowNum][cellPositionInGrid.columnNum];
        }

        private CellDataModel GetRandomLevelDataModel(PiecesData levelData)
        {
            Random rand = new();

            var randomIndex = rand.Next(0, levelData.CellModels.Count - 1);
            
            return levelData.CellModels[randomIndex];
        }

        private Vector3 GetPositionToMove(SwipeDirection swipeDirection, Vector2 cellSize, Vector3 cellPosition)
        {
            Vector3 movePosition;
            var offsetX = spacing.x * 2 + cellSize.x / 2;
            var offsetY = spacing.y * 2 + cellSize.y / 2;

            switch (swipeDirection)
            {
                case SwipeDirection.None:
                    throw new Exception("Duration can't be None");
                case SwipeDirection.Up:
                    movePosition = new Vector3(cellPosition.x, cellPosition.y - offsetY, cellPosition.z);
                    break;
                case SwipeDirection.Down:
                    movePosition = new Vector3(cellPosition.x, cellPosition.y + offsetY, cellPosition.z);
                    break;
                case SwipeDirection.Left:
                    movePosition = new Vector3(cellPosition.x - offsetX, cellPosition.y, cellPosition.z);
                    break;
                case SwipeDirection.Right:
                    movePosition = new Vector3(cellPosition.x + offsetX, cellPosition.y, cellPosition.z);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(swipeDirection), swipeDirection, null);
            }

            return movePosition;
        }

        public List<CellController> GetRowByNum(int rowNum)
        {
            if (rowNum > _cellsRowsToColumns.Count)
            {
                throw new Exception("You try get row by num bigger row count");
            }
            return _cellsRowsToColumns[rowNum];
        }

        public async void DeleteMatchedCellsIfNeedAsync()
        {
            var cellsToMatch = new List<CellPositionInGrid>(_cellsRowsToColumns.Count);
            
            for (var i = 0; i < _cellsRowsToColumns[0].Count; i++)
            {
                var column = GetColumn(i);

                var sequenceCellsToMatch = new List<CellPositionInGrid>();
                var targetCellType = column[0].CellModel.CellType;
                sequenceCellsToMatch.Add(column[0].CellModel.CellPositionInGrid);
                for (var j = 1; j < column.Count; j++)
                {
                    if (targetCellType == CellType.None)
                    {
                        targetCellType = column[j].CellModel.CellType;
                        continue;
                    }
                    
                    if (column[j].CellModel.CellType == targetCellType)
                    {
                        sequenceCellsToMatch.Add(column[j].CellModel.CellPositionInGrid);
                    }
                    else
                    {
                        if (sequenceCellsToMatch.Count > 2)
                        {
                            cellsToMatch.AddRange(sequenceCellsToMatch);
                        }
                        
                        sequenceCellsToMatch.Clear();

                        if (column.Count > j + 1)
                        {
                            targetCellType = column[j + 1].CellModel.CellType;
                        }
                    }
                }
            }
            
            for (var i = 1; i < _cellsRowsToColumns.Count; i++)
            {
                var sequenceCellsToMatch = new List<CellPositionInGrid>();
                var targetCellType = _cellsRowsToColumns[i][0].CellModel.CellType;
                for (var j = 1; j < _cellsRowsToColumns[i].Count - 1; j++)
                {
                    var cellController = _cellsRowsToColumns[i][j];
                    if (targetCellType == CellType.None)
                    {
                        targetCellType = cellController.CellModel.CellType;
                        continue;
                    }
                    
                    if (cellController.CellModel.CellType == targetCellType)
                    {
                        sequenceCellsToMatch.Add(cellController.CellModel.CellPositionInGrid);
                    }
                    else
                    {
                        if (sequenceCellsToMatch.Count > 2)
                        {
                            cellsToMatch.AddRange(sequenceCellsToMatch);
                        }
                        
                        sequenceCellsToMatch.Clear();

                        if (_cellsRowsToColumns[i].Count > j + 1)
                        {
                            targetCellType = _cellsRowsToColumns[i][j + 1].CellModel.CellType;
                        }
                    }
                }
            }

            foreach (var cellPositionInGrid in cellsToMatch)
            {
                MatchCell(cellPositionInGrid);
            }
        }

        private void MatchCell(CellPositionInGrid cellPositionInGrid)
        {
            var cellController = GetCell(cellPositionInGrid);
            cellController.DeleteCell();
        }

        public bool IsCanMatchVertical(CellPositionInGrid cellPositionInGrid)
        {
            var columnNum = cellPositionInGrid.columnNum;
            var rowNum = cellPositionInGrid.rowNum;
            var column = GetColumn(columnNum);
            var cellsToMatchCount = 0;
            
            for (var i = rowNum; i < column.Count - 1; i++)
            {
                if (column[i].CellModel.CellType == column[i + 1].CellModel.CellType)
                {
                    cellsToMatchCount++;
                }
                else
                {
                    break;
                }
            }
            
            if (cellsToMatchCount >= 2)
            {
                return true;
            }
            
            for (var i = rowNum; i > 0; i--)
            {
                if (column[i].CellModel.CellType == column[i - 1].CellModel.CellType)
                {
                    cellsToMatchCount++;
                }
                else
                {
                    break;
                }
            }
            
            if (cellsToMatchCount >= 2)
            {
                return true;
            }

            return false;
        }

        public bool IsCanMatchHorizontal(CellPositionInGrid cellPositionInGrid)
        {
            var rowNum = cellPositionInGrid.rowNum;
            var columnNum = cellPositionInGrid.columnNum;
            var row = _cellsRowsToColumns[rowNum];
            var cellsToMatchCount = 0;
            
            for (var i = columnNum; i < row.Count - 1; i++)
            {
                if (row[i].CellModel.CellType == row[i + 1].CellModel.CellType)
                {
                    cellsToMatchCount++;
                }
                else
                {
                    break;
                }
            }

            if (cellsToMatchCount >= 3)
            {
                return true;
            }
            
            for (var i = columnNum; i > 0; i--)
            {
                if (row[i].CellModel.CellType == row[i - 1].CellModel.CellType)
                {
                    cellsToMatchCount++;
                }
                else
                {
                    break;
                }
            }
            
            if (cellsToMatchCount >= 3)
            {
                return true;
            }

            return false;
        }
    }
}