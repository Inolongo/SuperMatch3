using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
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

        public event Action<CellController> CellCreated;
        
        private PiecesData _data;

        private readonly Dictionary<int, List<CellController>> _gridRowCells = new();

        private readonly Dictionary<int, List<RectTransform>> _decoyDictionary = new();

        private readonly Random _random = new();

        private Tweener _doLocalMove;

     

        public async Task Initialize()
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
            _data = Resources.Load<PiecesData>("GameplayData/FirstLevel");
        }

        public List<CellController> GetRowByNum(int rowNum)
        {
            return _gridRowCells[rowNum];
        }

        public List<CellController> GetColumnList(int columnNum)
        {
            List<CellController> columnList = new();
            foreach (var key in _gridRowCells.Keys)
            {
                foreach (var value in _gridRowCells[key])
                {
                    if (value.CellDataModel.RowColumnPair.ColumnNum == columnNum)
                    {
                        columnList.Add(value);
                        break;
                    }
                }
            }

            return columnList;
        }

        public CellController GetCell(int rowNum, int columnNum)
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
        
        public void SwipeCells(CellController semeCell, CellController ukeCell, Action<CellController, CellController> onSwipeCompleted = null)
        {
            SwapCellsInLists(semeCell, ukeCell);
            var tempPosition = ukeCell.transform.localPosition;

            ukeCell.transform.DOLocalMove(semeCell.transform.localPosition, 0.2f);
            _doLocalMove = semeCell.transform.DOLocalMove(tempPosition, 0.2f);
            
            _doLocalMove.onComplete += () => onSwipeCompleted?.Invoke(semeCell, ukeCell); ;
        }

        private void CreateGridDecoy()
        {
            var rowCount = _data.IsActiveCellRows.Count;
            for (var rowNum = 0; rowNum < rowCount; rowNum++)
            {
                var columnCount = _data.IsActiveCellRows[rowNum].Row.Count;
                var createdCells = new List<RectTransform>(columnCount);
                for (var columnNum = 0; columnNum < columnCount; columnNum++)
                {
                    var cellDecoy = new GameObject().AddComponent<RectTransform>();
                    cellDecoy.name = "Pidor R: " + rowNum + "; C: " + columnNum;
                    cellDecoy.SetParent(decoyGrid);
                    createdCells.Add(cellDecoy);
                }

                _decoyDictionary[rowNum] = createdCells;
            }
        }

        private void CreateGrid() 
        {
            var rowCount = _data.IsActiveCellRows.Count;
            for (var rowNum = 0; rowNum < rowCount; rowNum++)
            {
                var columnCount = _data.IsActiveCellRows[rowNum].Row.Count;
                var createdCells = new List<CellController>(columnCount);

                for (var columnNum = 0; columnNum < columnCount; columnNum++)
                {
                    var randomModel = GetRandomModel();
                    randomModel.ChangeRowColumn(rowNum, columnNum);

                    if (IsSafeHorizontal(createdCells, randomModel.CellType) && IsSafeVertical(randomModel))
                    {
                        if (GetIsCellActive(rowNum, columnNum, _data.IsActiveCellRows))
                        {
                            var decoyRectTransform = _decoyDictionary[rowNum][columnNum];
                            var cellController = Instantiate(cellPrefab, cellContainer);
                            cellController.Init(randomModel, decoyRectTransform);
                            createdCells.Add(cellController);
                            cellController.name = "piece " + "R: " + rowNum + "C: " + columnNum;
                            
                            CellCreated?.Invoke(cellController);
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

        private CellDataModel GetRandomModel()
        {
            var randomIndex = _random.Next(0, _data.CellModels.Count);
            var foundModel = _data.CellModels[randomIndex];
            return new CellDataModel(foundModel.PieceIcon, foundModel.PieceFace, foundModel.CellType,
                foundModel.RowColumnPair);
        }

        private bool IsSafeHorizontal(List<CellController> createdCells, CellType typeCellToCreat)
        {
            var count = createdCells.Count;
            if (count < 2) return true;

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

            CellDataModel lastCellMode = null;

            foreach (var cellController in _gridRowCells[cellModelToCreat.RowColumnPair.RowNum - 1])
            {
                if (cellController.CellDataModel.RowColumnPair.ColumnNum == cellModelToCreat.RowColumnPair.ColumnNum)
                {
                    lastCellMode = cellController.CellDataModel;

                    break;
                }
            }

            if (lastCellMode == null)
            {
                return true;
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

        private bool GetIsCellActive(int rowNum, int columnNum, List<IsActiveCellRow> list)
        {
            var row = list[rowNum].Row;
            return row[columnNum];
        }


        private void SwapCellsInLists(CellController semeCell, CellController ukeCell)
        {
            var semeRow = _gridRowCells[semeCell.CellDataModel.RowColumnPair.RowNum];
            var ukeRow = _gridRowCells[ukeCell.CellDataModel.RowColumnPair.RowNum];

            var tempUke = ukeCell.CellDataModel.RowColumnPair;
            ukeCell.CellDataModel.ChangeRowColumn(semeCell.CellDataModel.RowColumnPair);
            semeCell.CellDataModel.ChangeRowColumn(tempUke);

            var semeIndex = semeRow.IndexOf(semeCell);
            var ukeIndex = ukeRow.IndexOf(ukeCell);

            semeRow.Remove(semeCell);
            semeRow.Insert(semeIndex, ukeCell);

            ukeRow.Remove(ukeCell);
            ukeRow.Insert(ukeIndex, semeCell);
        }
    }
}