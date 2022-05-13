using System;
using System.Collections.Generic;
using Gayplay.Data;
using UnityEngine;
using Random = System.Random;

namespace Gayplay.GayplayGrid
{
    public class MatchThreeGrid : MonoBehaviour
    {
        [SerializeField] private Vector2 cellSize;
        [SerializeField] private Vector2 spacing;
        [SerializeField] private RectTransform content;

        private List<List<CellController>> _cells;
        private RectTransform _rectTransform;

        public void Initialize(PiecesData levelInformation, CellController prefab)
        {
            InitializeCells(levelInformation, prefab);
        }

        private void InitializeCells(PiecesData levelInformation, CellController prefab)
        {
            CreatCells(levelInformation, prefab);
            InitCellsPosition();
        }

        private void InitCellsPosition()
        {
            var cellInRowCount = _cells[0].Count;
            var cellInColumnCount = GetColumn(0).Count;

            var sumSpacingX = (cellInRowCount + 1) * spacing.x;
            var sumSpacingY = (cellInColumnCount + 1) * spacing.y;

            for (var i = 0; i < _cells.Count; i++)
            {
                for (var j = 0; j < _cells[i].Count; j++)
                {
                    var leftUpGridPosition = GetLeftUpPosition();
                    
                    var offsetX = j * (spacing.x + cellSize.x);
                    var cellXPosition = leftUpGridPosition.x + spacing.x + cellSize.x / 2 + offsetX;

                    var offsetY = j * (spacing.y + cellSize.y);
                    var cellYPosition = leftUpGridPosition.y + spacing.y + cellSize.y / 2 + offsetY;

                    _cells[i][j].transform.localPosition =
                        new Vector3(cellXPosition, cellYPosition, content.transform.localPosition.z);
                }
            }
        }

        private Vector2 GetLeftUpPosition()
        {
            var contentRect = ((RectTransform)content.transform);
            var contentLocalPosition = contentRect.localPosition;
            
            var leftUpGridXPosition = contentLocalPosition.x + contentRect.rect.height / 2;
            var leftUpGridYPosition = contentLocalPosition.y - contentRect.rect.width / 2;
            
            var leftUpGridPosition = new Vector2(leftUpGridXPosition, leftUpGridYPosition);

            return leftUpGridPosition;
        }

        private List<CellController> GetColumn(int columnIndex)
        {
            if (columnIndex < 0)
            {
                throw new Exception("Column index can't be less than zero");
            }

            if (columnIndex > _cells.Count - 1)
            {
                throw new Exception("Column index can't be bigger row lenght");
            }
            
            var column = new List<CellController>(_cells.Count);

            foreach (var row in _cells)
            {
                column.Add(row[columnIndex]);
            }

            return column;
        }

        private void CreatCells(PiecesData levelInformation, CellController prefab)
        {
            _cells = new List<List<CellController>>(levelInformation.IsActiveCellRows.Count);

            foreach (var row in levelInformation.IsActiveCellRows)
            {
                var cellsRow = new List<CellController>(row.Row.Count);

                foreach (var isCellActive in row.Row)
                {
                    var cellModel = GetRandomCellModel(levelInformation, isCellActive);
                    var cellController = CreatCell(prefab, cellModel);
                    
                    cellsRow.Add(cellController);
                }
                
                _cells.Add(cellsRow);
            }
        }

        private CellModel GetRandomCellModel(PiecesData levelInformation, bool isCellActive)
        {
            var levelCellDataModel = GetRandomLevelDataModel(levelInformation);

            CellModel cellModel;
            if (isCellActive)
            {
                cellModel = new CellModel(false, levelCellDataModel.PieceIcon,
                    levelCellDataModel.PieceFace, levelCellDataModel.CellType, cellSize);
            }
            else
            {
                cellModel = new CellModel(true, null, null, CellType.None, cellSize);
            }

            return cellModel;
        }

        private CellController CreatCell(CellController prefab, CellModel cellModel)
        {
            var cellController = Instantiate(prefab, content);

            cellController.Init(cellModel, cellSize);

            return cellController;
        }

        private CellDataModel GetRandomLevelDataModel(PiecesData levelInformation)
        {
            Random rand = new();

            var randomIndex = rand.Next(0, levelInformation.CellModels.Count - 1);
            
            return levelInformation.CellModels[randomIndex];
        }
    }
}