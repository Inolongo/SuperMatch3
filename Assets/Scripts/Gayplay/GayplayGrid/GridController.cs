using System;
using System.Collections.Generic;
using System.Linq;
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

        private PiecesData _data;

        private readonly Dictionary<int, List<CellController>> _gridRowCells = new();

        private readonly Dictionary<int, List<RectTransform>> _decoyDictionary = new();

        private readonly Random _random = new();
        private Tweener _doLocalMove;

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
            _data = Resources.Load<PiecesData>("GameplayData/FirstLevel");
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
                            var piece = Instantiate(cellPrefab, cellContainer);
                            piece.Init(randomModel, decoyRectTransform);
                            createdCells.Add(piece);
                            piece.name = "piece " + "R: " + rowNum + "C: " + columnNum;

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
            Debug.Log("OnCellSwiped dir " +swipeDirection);
            switch (swipeDirection)
            {
                case SwipeDirection.Up:
                {
                    var ukeCell = GetCell(rowNum - 1, columnNum);
                    SwipeCells(semeCell, ukeCell);
                    break;
                }
                case SwipeDirection.Down:
                {
                    var ukeCell = GetCell(rowNum + 1, columnNum);
                    SwipeCells(semeCell, ukeCell);
                    break;
                }
                case SwipeDirection.Left:
                {
                    var ukeCell = GetCell(rowNum, columnNum - 1);
                    SwipeCells(semeCell, ukeCell);
                    break;
                }
                case SwipeDirection.Right:
                {
                    var ukeCell = GetCell(rowNum, columnNum + 1);
                    SwipeCells(semeCell, ukeCell);
                    break;
                }
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

        //ToDo: fix list bugs there is some shit
        private void SwipeCells(CellController semeCell, CellController ukeCell)
        {
            Debug.Log("zaletel v SwipeCells");
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

            var tempPosition = ukeCell.transform.localPosition;

            ukeCell.transform.DOLocalMove(semeCell.transform.localPosition, 0.2f);
            _doLocalMove = semeCell.transform.DOLocalMove(tempPosition, 0.2f);
            _doLocalMove.onComplete += () => OnCellSwipeMoveComplete(semeCell, ukeCell);
        }

        private void OnCellSwipeMoveComplete(CellController semeCell, CellController ukeCell)
        {
            if (TryMatch(semeCell, out var matchedCells))
            {
                foreach (var rowColumnPair in matchedCells)
                {
                    var gridRowCell = _gridRowCells[rowColumnPair.RowNum];
                    foreach (var cellController in gridRowCell)
                    {
                        if (cellController.CellDataModel.RowColumnPair.ColumnNum == rowColumnPair.ColumnNum)
                        {
                            cellController.DeleteCell();
                        }
                    }
                }
            }
            else
            {
                var tempPosition = ukeCell.transform.localPosition;

                ukeCell.transform.DOLocalMove(semeCell.transform.localPosition, 0.2f);
                _doLocalMove = semeCell.transform.DOLocalMove(tempPosition, 0.2f);
            }
        }


        private bool TryMatch(CellController semeCell, out List<RowColumnPair> matchedCells)
        {
            matchedCells = new List<RowColumnPair>();
            var horizontalMatch = GetSameNearHorizontalCells(semeCell);
            if (horizontalMatch.Count >= 2)
            {
                matchedCells.AddRange(horizontalMatch);
            }

            var verticalMatch = GetSameNearVerticalCells(semeCell);
            if (verticalMatch.Count >= 2)
            {
                matchedCells.AddRange(verticalMatch);
            }

            matchedCells.Add(semeCell.CellDataModel.RowColumnPair);

            return matchedCells.Count > 1;
        }


        private List<RowColumnPair> GetSameNearHorizontalCells(CellController semeCell)
        {
            var rowNum = semeCell.CellDataModel.RowColumnPair.RowNum;
            var row = _gridRowCells[rowNum];

            var columnCount = row[row.Count - 1].CellDataModel.RowColumnPair.ColumnNum;
            var semeIndex = row.IndexOf(semeCell);


            List<RowColumnPair> potancevalnyList = new();
            for (int i = semeIndex + 1; i < columnCount; i++)
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
            var columnNum = semeCell.CellDataModel.RowColumnPair.ColumnNum;
            var rowNum = semeCell.CellDataModel.RowColumnPair.RowNum;
            var columnList = GetColumnList(columnNum);
            List<RowColumnPair> potancevalnyList = new();
            var semeIndex = columnList.IndexOf(semeCell);
            var rowCount = columnList[columnList.Count - 1].CellDataModel.RowColumnPair.RowNum;

            for (int i = semeIndex + 1; i < rowCount; i++)
            {
                if (i >= columnList.Count - 1 )
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


        private List<CellController> GetColumnList(int columnNum)
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
    }
}