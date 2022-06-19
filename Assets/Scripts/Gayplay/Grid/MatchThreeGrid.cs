using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using Gayplay.Data;
using Gayplay.Gameplay;
using Gayplay.Gameplay.Cell;
using UnityEngine;
using Random = System.Random;

namespace Gayplay.GayplayGrid
{
    public class MatchThreeGrid : MonoBehaviour
    {
        [SerializeField] private Vector2 spacing;
        [SerializeField] private RectTransform content;
        
        //TODO:Remove after tests
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

        public void CreatCellsFromEditor()
        {
            Initialize(levelDataTest);
        }

        #endregion

        public void Initialize(PiecesData levelData)
        {
            var rowCount = levelData.IsActiveCellRows.Count;
            _cellSize = GetCellSize(levelData.IsActiveCellRows[0].Row.Count, rowCount);

            CreateCells(levelData);
            InitCellsPosition();
        }

        public void SwipeCells(CellController firstCell, CellController secondCell, SwipeDirection swipeDirection,
            Action onCellSwipeAnimationCompleted = null)
        {
            if (firstCell == null && secondCell == null)
            {
                throw new Exception("Can't move null pair cells");
            }

            if (firstCell.Model is not CellModel firstCellModel) return;
            if (secondCell.Model is not CellModel secondCellModel) return;
            
            Vector3 firstPosition;
            Vector3 secondPosition;
            if (firstCell != null)
            {
                firstPosition = firstCell.transform.localPosition;
            }
            else
            {
                var secondCellSize = secondCellModel.Size;
                var secondCellPosition = secondCell.transform.localPosition;
                firstPosition = GetPositionToMove(swipeDirection, secondCellSize, secondCellPosition);
            }

            if (secondCell != null)
            {
                secondPosition = secondCell.transform.localPosition;
            }
            else
            {
                var firstCellSize = firstCellModel.Size;
                var firstCellPosition = firstCell.transform.localPosition;
                secondPosition = GetPositionToMove(swipeDirection, firstCellSize, firstCellPosition);
            }

            var secondCellPositionInGrid = secondCellModel.CellPositionInGrid;
            var firstCellPositionInGrid = firstCellModel.CellPositionInGrid;

            firstCell.transform.DOLocalMove(secondPosition, 0.3f);

            Tween secondCellTween = secondCell.transform.DOLocalMove(firstPosition, 0.3f);
            secondCellTween.onComplete += () =>
            {
                secondCellModel.CellPositionInGrid = firstCellPositionInGrid;
                SwapCells(firstCellPositionInGrid, secondCellPositionInGrid);
                onCellSwipeAnimationCompleted?.Invoke();
            };
        }

        public List<CellController> GetRowByNum(int rowNum)
        {
            if (rowNum > _cellsRowsToColumns.Count)
            {
                throw new Exception("You try get row by num bigger row count");
            }

            return _cellsRowsToColumns[rowNum];
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

        public List<CellController> GetColumn(int columnIndex)
        {
            if (columnIndex < 0)
            {
                throw new Exception("Column index can't be less than zero");
            }

            if (columnIndex > ((CellModel)_cellsRowsToColumns[0][^1].Model).CellPositionInGrid.columnNum)
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

        public CellController GetCell(CellPositionInGrid cellPositionInGrid)
        {
            if (_cellsRowsToColumns.Count < cellPositionInGrid.rowNum ||
                _cellsRowsToColumns[0].Count < cellPositionInGrid.columnNum)
            {
                return null;
            }

            return _cellsRowsToColumns[cellPositionInGrid.rowNum][cellPositionInGrid.columnNum];
        }

        private void SwapCells(CellPositionInGrid newCellPosition, CellPositionInGrid oldCellPosition)
        {
            var firstCell = GetCell(oldCellPosition);
            var secondCell = GetCell(newCellPosition);
            
            if (firstCell.Model is not CellModel firstCellModel) return;
            if (secondCell.Model is not CellModel secondCellModel) return;
            
            // ReSharper disable once InlineTemporaryVariable
            var temp = secondCell;
            _cellsRowsToColumns[newCellPosition.rowNum][newCellPosition.columnNum] = firstCell;
            _cellsRowsToColumns[oldCellPosition.rowNum][oldCellPosition.columnNum] = temp;

            firstCellModel.CellPositionInGrid = new CellPositionInGrid(newCellPosition);
            secondCellModel.CellPositionInGrid = new CellPositionInGrid(oldCellPosition);
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

            var rect = contentRect.rect;
            var leftUpGridXPosition = contentLocalPosition.x - rect.width / 2;
            var leftUpGridYPosition = contentLocalPosition.y + rect.height / 2;

            var leftUpGridPosition = new Vector2(leftUpGridXPosition, leftUpGridYPosition);

            return leftUpGridPosition;
        }

        private Vector2 GetCellSize(int rowCellsCount, int columnCellsCount)
        {
            var contentRect = ((RectTransform)content.transform).rect;

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

        private void CreateCells(PiecesData levelData)
        {
            _cellsRowsToColumns = new List<List<CellController>>(levelData.IsActiveCellRows.Count);

            for (var i = 0; i < levelData.IsActiveCellRows.Count; i++)
            {
                var row = levelData.IsActiveCellRows[i];
                var cellsRow = new List<CellController>(row.Row.Count);

                for (var j = 0; j < row.Row.Count; )
                {
                    var isCellActive = row.Row[j];

                    var cellModel = GetRandomCellModel(levelData, isCellActive);

                    cellModel.CellPositionInGrid = new CellPositionInGrid(i, j);
                    if (!IsSafeToSpawn(cellModel, cellsRow))
                    {
                        continue;
                    }

                    var cellController = CreatCell(levelData.CellPrefab, cellModel); 
                    
                    cellsRow.Add(cellController);

                    j++;
                }

                _cellsRowsToColumns.Add(cellsRow);
            }
        }

        private bool IsSafeToSpawn(CellModel cellModel, List<CellController> row)
        {
            var cellPositionInGrid = cellModel.CellPositionInGrid;

            var columnNum = cellPositionInGrid.columnNum;
            var rowNum = cellPositionInGrid.rowNum;

            var isSafeVerticalToSpawn = IsSafeVerticalToSpawn(columnNum, rowNum, cellModel.CellType);
            var isSafeHorizontalToSpawn = IsSafeHorizontalToSpawn(row, columnNum, cellModel.CellType);
            var isSafeToSpawn = isSafeVerticalToSpawn && isSafeHorizontalToSpawn;

            return isSafeToSpawn;
        }

        private bool IsSafeHorizontalToSpawn(List<CellController> row, int columnNum, CellType cellType)
        {
            if (cellType == CellType.None)
            {
                return true;
            }
  
            if (columnNum < 2)
            {
                return true;
            }

            if (row[columnNum - 1].Model is not CellModel firstCellModel)
            {
                throw new Exception("Incorrect model type");
            }

            if (row[columnNum - 2].Model is not CellModel secondCellModel)
            {
                throw new Exception("Incorrect model type");
            }

            var isSafeHorizontalToSpawn =
                cellType != firstCellModel.CellType ||
                cellType != secondCellModel.CellType;

            return isSafeHorizontalToSpawn;
        }

        private bool IsSafeVerticalToSpawn(int columnNum, int rowNum, CellType cellType)
        {
            if (cellType == CellType.None)
            {
                return true;
            }

            if (rowNum < 2)
            {
                return true;
            }

            var column = GetColumn(columnNum);

            if (column[rowNum - 1].Model is not CellModel firstCellModel)
            {
                throw new Exception("Incorrect model type");
            }

            if (column[rowNum - 2].Model is not CellModel secondCellModel)
            {
                throw new Exception("Incorrect model type");
            }
            
            var isSafe = 
                cellType != firstCellModel.CellType || cellType != secondCellModel.CellType;
  
            return isSafe;
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
            cellController.Initialize(cellModel);
            CellCreated?.Invoke(cellController);

            return cellController;
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

        #region MatchRegion

        public async Task DeleteMatchedCellsIfNeedAsync()
        {
            var cellsToMatch = await Task.Run(GetCellsToMatch);

            foreach (var cellPositionInGrid in cellsToMatch)
            {
                MatchCell(cellPositionInGrid);
            }
        }

        public void DeleteMatchedCellsIfNeed()
        {
            var cellsToMatch = GetCellsToMatch();

            foreach (var cellPositionInGrid in cellsToMatch)
            {
                MatchCell(cellPositionInGrid);
            }
        }

        private List<CellPositionInGrid> GetCellsToMatch()
        {
            var cellsToMatch = new List<CellPositionInGrid>(_cellsRowsToColumns.Count);

            var verticalCellsToMatch = GetVerticalCellsToMatch();
            cellsToMatch.AddRange(verticalCellsToMatch);

            var horizontalCellsToMatch = GetHorizontalCellsToMatch();
            
            foreach (var cellPositionInGrid in horizontalCellsToMatch)
            {
                if (!cellsToMatch.Contains(cellPositionInGrid))
                {
                    cellsToMatch.Add(cellPositionInGrid);
                }
            }

            return cellsToMatch;
        }

        private List<CellPositionInGrid> GetHorizontalCellsToMatch()
        {
            var cellsToMatch = new List<CellPositionInGrid>(_cellsRowsToColumns.Count);

            for (var i = 0; i < _cellsRowsToColumns.Count; i++)
            {
                var sequenceCellsToMatch = new List<CellPositionInGrid>();
                var targetCellType = ((CellModel)_cellsRowsToColumns[i][0].Model).CellType;
                
                for (var j = 0; j < _cellsRowsToColumns[i].Count - 1; j++)
                {
                    var cellModel = (CellModel)_cellsRowsToColumns[i][j].Model;

                    if (cellModel.CellType == targetCellType)
                    {
                        if (targetCellType != CellType.None)
                        {
                            sequenceCellsToMatch.Add(cellModel.CellPositionInGrid);
                        }
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
                            targetCellType = cellModel.CellType;
                            sequenceCellsToMatch.Add(cellModel.CellPositionInGrid);
                        }
                    }
                }
            }

            return cellsToMatch;
        }

        private List<CellPositionInGrid> GetVerticalCellsToMatch()
        {
            var cellsToMatch = new List<CellPositionInGrid>(_cellsRowsToColumns.Count);
            
            for (var i = 0; i < _cellsRowsToColumns[0].Count; i++)
            {
                var column = GetColumn(i);
                var firstCellModel = ((CellModel)column[0].Model);
                var sequenceCellsToMatch = new List<CellPositionInGrid>();
                
                var targetCellType = firstCellModel.CellType;
                sequenceCellsToMatch.Add(firstCellModel.CellPositionInGrid);
                
                for (var j = 0; j < column.Count; j++)
                {
                    var cellModel = (CellModel)column[j].Model;
                    
                    if (cellModel.CellType == targetCellType)
                    {
                        if (targetCellType != CellType.None)
                        {
                            sequenceCellsToMatch.Add(cellModel.CellPositionInGrid);
                        }
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
                            targetCellType = cellModel.CellType;
                            sequenceCellsToMatch.Add(cellModel.CellPositionInGrid);
                        }
                    }
                }
            }

            return cellsToMatch;
        }

        private void MatchCell(CellPositionInGrid cellPositionInGrid)
        {
            var cellController = GetCell(cellPositionInGrid);
            cellController.DeleteCell();
        }

        public bool IsCanMatchVertical(CellPositionInGrid cellPositionInGrid)
        {
            var firstCellModelToCompare = (CellModel)GetCell(cellPositionInGrid).Model;
            if (firstCellModelToCompare.CellType == CellType.None) return false;

            var columnNum = cellPositionInGrid.columnNum;
            var rowNum = cellPositionInGrid.rowNum;
            var column = GetColumn(columnNum);

            var isCanMatchVerticalDown = IsCanMatchVerticalDown(rowNum, column);
            var isCanMatchVerticalUp = IsCanMatchVerticalUp(rowNum, column);

            return isCanMatchVerticalDown || isCanMatchVerticalUp;
        }

        private bool IsCanMatchVerticalUp(int rowNum, List<CellController> column)
        {
            var cellsToMatchCount = 0;
            
            for (var i = rowNum; i > 0; i--)
            {
                if (((CellModel)column[i].Model).CellType == ((CellModel)column[i - 1].Model).CellType)
                {
                    cellsToMatchCount++;
                }
                else
                {
                    break;
                }
            }

            return cellsToMatchCount >= 2;
        }

        private bool IsCanMatchVerticalDown(int rowNum, List<CellController> column)
        {
            var cellsToMatchCount = 0;
            
            for (var i = rowNum; i < column.Count - 1; i++)
            {
                if (((CellModel)column[i].Model).CellType == ((CellModel)column[i + 1].Model).CellType)
                {
                    cellsToMatchCount++;
                }
                else
                {
                    break;
                }
            }

            return cellsToMatchCount >= 2;
        }

        public bool IsCanMatchHorizontal(CellPositionInGrid cellPositionInGrid)
        {
            var firstCellModelToCompare = (CellModel)GetCell(cellPositionInGrid).Model;
            if (firstCellModelToCompare.CellType == CellType.None) return false;

            var rowNum = cellPositionInGrid.rowNum;
            var columnNum = cellPositionInGrid.columnNum;
            var row = _cellsRowsToColumns[rowNum];

            var isCanMatchHorizontalLeft = IsCanMatchHorizontalLeft(columnNum, row);
            var isCanMatchHorizontalRight = IsCanMatchHorizontalRight(columnNum, row);

            return isCanMatchHorizontalLeft || isCanMatchHorizontalRight;
        }

        private bool IsCanMatchHorizontalRight(int columnNum, List<CellController> row)
        {
            var cellsToMatchCount = 0;
            
            for (var i = columnNum; i > 0; i--)
            {
                if (((CellModel)row[i].Model).CellType == ((CellModel)row[i - 1].Model).CellType)
                {
                    cellsToMatchCount++;
                }
                else
                {
                    break;
                }
            }

            return cellsToMatchCount >= 2;
        }

        private bool IsCanMatchHorizontalLeft(int columnNum, List<CellController> row)
        {
            var cellsToMatchCount = 0;
            
            for (var i = columnNum; i < row.Count - 1; i++)
            {
                if (((CellModel)row[i].Model).CellType == ((CellModel)row[i + 1].Model).CellType)
                {
                    cellsToMatchCount++;
                }
                else
                {
                    break;
                }
            }

            return cellsToMatchCount >= 2;
        }
        
        #endregion
    }
}