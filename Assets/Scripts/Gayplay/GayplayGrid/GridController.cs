using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gayplay.Data;
using UnityEngine;
using Random = System.Random;

namespace Gayplay.GayplayGrid
{
    public class GridController : MonoBehaviour
    {
        [SerializeField] private CellController cellPrefab;
        [SerializeField] private Transform cellContainer;
        [SerializeField] private Transform decoyGrid;

        private PiecesData data;

        private readonly Dictionary<int, List<CellController>> _gridRowCells =
            new Dictionary<int, List<CellController>>();

        private readonly Dictionary<int, List<RectTransform>> _decoyDictionary = new();

        private readonly Random _random = new Random();

        private void Start()
        {
            Initialize();
        }

        private async void Initialize()
        {
            InitData();
            CreateGridDecoy();

            await АааШестьКадроооов();

            CreateGrid();
        }

        private async Task АааШестьКадроооов()
        {
            await Task.Yield();
            await Task.Yield();
            await Task.Yield();
            await Task.Yield();
            await Task.Yield();
            await Task.Yield();
        }

        private void InitData()
        {
            data = Resources.Load<PiecesData>("GameplayData/FirstLevel");
        }

        private void CreateGrid()
        {
            var rowCount = data.IsActiveCellRows.Count;
            for (var rowNum = 0; rowNum < rowCount; rowNum++)
            {
                var columnCount = data.IsActiveCellRows[rowNum].Row.Count;
                var createdCells = new List<CellController>(columnCount);

                for (var columnNum = 0; columnNum < columnCount; columnNum++)
                {
                    var randomModel = GetRandomModel();
                    randomModel.ChangeRowColumn(rowNum, columnNum);

                    if (IsSafeHorizontal(createdCells, randomModel.CellType) && IsSafeVertical(randomModel))
                    {
                        if (GetIsCellActive(rowNum, columnNum, data.IsActiveCellRows))
                        {
                            var decoyRectTransform = _decoyDictionary[rowNum][columnNum];
                            var piece = Instantiate(cellPrefab, cellContainer);
                            piece.Init(randomModel, decoyRectTransform);
                            createdCells.Add(piece);

                            piece.CellSwiped += OnCellSwiped;
                        }
                    }
                    else
                    {
                        columnNum -= 1;
                    }
                }

                _gridRowCells[rowNum] = createdCells;
            }
        }

        private void OnCellSwiped(SwipeDirection swipeDirection, int rowNum, int columnNum)
        {
            var semeCell = GetCell(rowNum, columnNum);
            switch (swipeDirection)
            {
                case SwipeDirection.Up:
                    if (rowNum > 1)
                    {
                        var ukeCell = GetCell(rowNum - 1, columnNum);
                        SwipeCells(semeCell, ukeCell);
                    }
                    
                    break;
                case SwipeDirection.Down:
                    if (rowNum < _gridRowCells.Count - 1)
                    {
                        var ukeCell = GetCell(rowNum + 1, columnNum);
                        SwipeCells(semeCell, ukeCell);

                    }
                    break;
                case SwipeDirection.Left:

                    if (columnNum > 1)
                    {
                        var ukeCell = GetCell(rowNum, columnNum - 1);
                        SwipeCells(semeCell, ukeCell);

                    }
                    break;
                case SwipeDirection.Right:
                    if (columnNum < _gridRowCells[rowNum].Count - 1)
                    {
                        var ukeCell = GetCell(rowNum, columnNum + 1);
                        SwipeCells(semeCell, ukeCell);
                    }
                    
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(swipeDirection), swipeDirection, null);
            }

            GetCell(rowNum, columnNum);
        }

        private CellController GetCell(int rowNum, int columnNum)
        {
            foreach (var cellController in _gridRowCells[rowNum])
            {
                if (cellController.CellDataModel.RowColumnPair.ColumnNum == columnNum)
                {
                    return cellController;
                }
            }

            throw new Exception("no bitches???");
        }

        private void SwipeCells(CellController semeCell, CellController ukeCell)
        {
            var tempPosition = ukeCell.transform.localPosition;
            var semeRow = _gridRowCells[semeCell.CellDataModel.RowColumnPair.RowNum];
            var ukeRow = _gridRowCells[ukeCell.CellDataModel.RowColumnPair.RowNum];

            for (var i = 0; i < semeRow.Count; i++)
            {
                if (semeRow[i].CellDataModel.RowColumnPair.ColumnNum == semeCell.CellDataModel.RowColumnPair.ColumnNum)
                {
                    for (var j = 0; j < ukeRow.Count; j++)
                    {
                        var uke = ukeRow[j];
                        if (uke.CellDataModel.RowColumnPair.ColumnNum == ukeCell.CellDataModel.RowColumnPair.ColumnNum)
                        {
                            var tempUke = uke.CellDataModel.RowColumnPair;
                            uke.CellDataModel.ChangeRowColumn(semeCell.CellDataModel.RowColumnPair);
                            semeCell.CellDataModel.ChangeRowColumn(ukeCell.CellDataModel.RowColumnPair);
                            
                            (semeRow[i], ukeRow[j]) = (ukeRow[j], semeRow[i]);
                            
                        }
                    }
                }
            }

            semeCell.transform.localPosition = ukeCell.transform.localPosition;
            ukeCell.transform.localPosition = tempPosition;
        }

        private void CreateGridDecoy()
        {
            var rowCount = data.IsActiveCellRows.Count;
            for (var rowNum = 0; rowNum < rowCount; rowNum++)
            {
                var columnCount = data.IsActiveCellRows[rowNum].Row.Count;
                var createdCells = new List<RectTransform>(columnCount);
                for (var columnNum = 0; columnNum < columnCount; columnNum++)
                {
                    var cellDecoy = new GameObject().AddComponent<RectTransform>();
                    cellDecoy.name = "Pidor";
                    cellDecoy.SetParent(decoyGrid);
                    createdCells.Add(cellDecoy);
                }

                _decoyDictionary[rowNum] = createdCells;
            }
        }

        private bool GetIsCellActive(int rowNum, int columnNum, List<IsActiveCellRow> list)
        {
            var row = list[rowNum].Row;
            return row[columnNum];
        }

        private CellDataModel GetRandomModel()
        {
            var randomIndex = _random.Next(0, data.CellModels.Count);
            return data.CellModels[randomIndex];
        }

        private bool IsSafeHorizontal(List<CellController> createdCells, CellType typeCellToCreat)
        {
            var count = createdCells.Count;
            if (count <= 2) return true;

            if (createdCells[count - 1].CellType == typeCellToCreat)
            {
                if (createdCells[count - 2].CellType == typeCellToCreat)
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsSafeVertical(CellDataModel cellModelToCreat)
        {
            if (cellModelToCreat.RowColumnPair.RowNum <= 1) return true;

            CellDataModel lastCellMode = default;

            foreach (var cellController in _gridRowCells[cellModelToCreat.RowColumnPair.RowNum - 1])
            {
                if (cellController.CellDataModel.RowColumnPair.ColumnNum == cellModelToCreat.RowColumnPair.ColumnNum)
                {
                    lastCellMode = cellController.CellDataModel;

                    break;
                }
            }

            if (lastCellMode.CellType != cellModelToCreat.CellType) return true;

            CellDataModel previousCellMode = default;

            foreach (var cellController in _gridRowCells[cellModelToCreat.RowColumnPair.RowNum - 1])
            {
                if (cellController.CellDataModel.RowColumnPair.ColumnNum == cellModelToCreat.RowColumnPair.ColumnNum)
                {
                    previousCellMode = cellController.CellDataModel;

                    break;
                }
            }

            if (previousCellMode.CellType != cellModelToCreat.CellType) return true;


            return false;
        }
    }
}