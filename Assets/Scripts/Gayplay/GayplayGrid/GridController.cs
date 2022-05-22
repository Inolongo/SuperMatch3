// using System;
// using System.Collections.Generic;
// using System.Threading.Tasks;
// using DG.Tweening;
// using Gayplay.Data;
// using Unity.Collections;
// using UnityEngine;
// using Random = System.Random;
//
// namespace Gayplay.GayplayGrid
// {
//     public class GridController : MonoBehaviour
//     {
//         private const float SwipeCellsDuration = 0.2f;
//         
//         [SerializeField] private CellController cellPrefab;
//         [SerializeField] private Transform cellContainer;
//         [SerializeField] private Transform decoyGrid;
//
//         public event Action<CellController> CellCreated;
//         
//         private PiecesData _data;
//
//         [ReadOnly]
//         private readonly Dictionary<int, List<CellController>> _gridRowCells = new();
//         [ReadOnly]
//         private readonly Dictionary<int, List<RectTransform>> _decoyDictionary = new();
//         
//         private readonly Random _random = new();
//         private bool _isCellsMoving;
//         private Tweener _doLocalMove;
//
//      
//
//         public async Task Initialize()
//         {
//             InitData();
//             CreateGridDecoy();
//
//             await АааШестьКадроооов();
//
//             CreateGrid();
//         }
//
//         private void InitData()
//         {
//             _data = Resources.Load<PiecesData>("GameplayData/FirstLevel");
//         }
//
//         public List<CellController> GetRowByNum(int rowNum)
//         {
//             return _gridRowCells[rowNum];
//         }
//
//         public void SwipeCells(CellController seme, CellController uke, Action<CellController, CellController> onSwipeCompleted = null, bool isForce = false)
//         {
//             SwapCells(seme, uke);
//             
//             var semeRowColumnPair = seme.CellDataModel.RowColumnPair;
//             var newSemePosition = _decoyDictionary[semeRowColumnPair.RowNum][semeRowColumnPair.ColumnNum].localPosition;
//             
//             var ukeRowColumnPair = uke.CellDataModel.RowColumnPair;
//             var newUkePosition = _decoyDictionary[ukeRowColumnPair.RowNum][ukeRowColumnPair.ColumnNum].localPosition;
//
//             if (isForce)
//             {
//                 seme.transform.localPosition = newSemePosition;
//                 uke.transform.localPosition = newUkePosition;
//                 onSwipeCompleted?.Invoke(seme, uke);
//             }
//             else
//             {
//                 seme.transform.DOLocalMove(newSemePosition, SwipeCellsDuration).onComplete += () => onSwipeCompleted?.Invoke(seme, uke);
//                 uke.transform.DOLocalMove(newUkePosition, SwipeCellsDuration);
//             }
//         }
//
//         public bool TryGetCell(int rowNum, int columnNum, out CellController cellController)
//         {
//             cellController = null;
//             
//             if (!_gridRowCells.ContainsKey(rowNum))
//             {
//                 Debug.LogWarning("Can't get row by num " + rowNum);
//                 return false;
//             }
//
//             foreach (var cell in _gridRowCells[rowNum])
//             {
//                 if (cell.CellDataModel.RowColumnPair.ColumnNum == columnNum)
//                 {
//                     cellController = cell;
//                     
//                     return true;
//                 }
//             }
//
//             return false;
//         }
//
//         public List<CellController> GetColumn(int columnNum)
//         {
//             List<CellController> columnList = new();
//             foreach (var key in _gridRowCells.Keys)
//             {
//                 for (var i = 0; i < _gridRowCells[key].Count; i++)
//                 {
//                     var value = _gridRowCells[key][i];
//                     if (value.CellDataModel.RowColumnPair.ColumnNum == columnNum)
//                     {
//                         columnList.Add(value);
//                         break;
//                     }
//                 }
//             }
//
//             return columnList;
//         }
//
//         public async void DeleteMatchedCellsIfNeedAsync()
//         { 
//             await MoveCellOnEmptyPlaceIfNeed();
//         }
//
//         private void CreateGrid() 
//         {
//             var rowCount = _data.IsActiveCellRows.Count;
//             for (var rowNum = 0; rowNum < rowCount; rowNum++)
//             {
//                 var columnCount = _data.IsActiveCellRows[rowNum].Row.Count;
//                 var createdCells = new List<CellController>(columnCount);
//
//                 for (var columnNum = 0; columnNum < columnCount; columnNum++)
//                 {
//                     var randomModel = GetRandomModel();
//                     randomModel.ChangeRowColumn(rowNum, columnNum);
//
//                     if (IsSafeHorizontalToSpawn(createdCells, randomModel.CellType) && IsSafeVerticalToSpawn(randomModel))
//                     {
//                         if (GetIsCellActive(rowNum, columnNum, _data.IsActiveCellRows))
//                         {
//                             var decoyRectTransform = _decoyDictionary[rowNum][columnNum];
//                             var cellController = Instantiate(cellPrefab, cellContainer);
//                             cellController.Init(randomModel, decoyRectTransform);
//                             createdCells.Add(cellController);
//                             
// #if UNITY_EDITOR
//                             cellController.name = "piece " + "R: " + rowNum + "C: " + columnNum;
// #endif
//                             
//                             CellCreated?.Invoke(cellController);
//                         }
//                     }
//                     else
//                     {
//                         columnNum -= 1;
//                     }
//                 }
//
//                 _gridRowCells[rowNum] = createdCells;
//             }
//         }
//
//         private bool IsHaveCellsToMatch()
//         {
//             foreach (var key in _gridRowCells.Keys)
//             {
//                 foreach (var cellController in _gridRowCells[key])
//                 {
//                     var rowColumnPair = cellController.CellDataModel.RowColumnPair;
//                     if (!IsSafeHorizontal(rowColumnPair) || !IsSafeVertical(rowColumnPair))
//                     {
//                         return true;
//                     }
//                 }
//             }
//
//             return false;
//         }
//         
//         private async Task MoveCellOnEmptyPlaceIfNeed()
//         {
//             for (var i = _gridRowCells.Keys.Count - 1; i > 0; i--)
//             {
//                 if(!_gridRowCells.ContainsKey(i) || !_gridRowCells.ContainsKey(i - 1)) continue;
//
//                 for (var j = 0; j < _gridRowCells[i].Count; j++)
//                 {
//                     var firstCell = _gridRowCells[i][j];
//                     if (firstCell.CellDataModel.IsMatched)
//                     {
//                         var rowColumnPair = firstCell.CellDataModel.RowColumnPair;
//                         
//                         if (TryGetFirstNotMatchedCellInColumn(rowColumnPair.RowNum - 1, rowColumnPair.ColumnNum, out var secondCell))
//                         {
//                             if (firstCell.CellDataModel.CellType == CellType.None ||
//                                 secondCell.CellDataModel.CellType == CellType.None)
//                             {
//                                 continue;
//                             }
//
//                             await Task.Yield();
//
//                             SwipeCells(firstCell, secondCell);
//                         }
//                     }
//                 }
//             }
//         }
//
//         private bool TryGetFirstNotMatchedCellInColumn(int startRowIndex, int columnNum, out CellController cellController)
//         {
//             cellController = null;
//             var column = GetColumn(columnNum);
//
//             for (var i = column.Count - 1; i >= 0; i--)
//             {
//                 if(column[i].CellDataModel.RowColumnPair.RowNum > startRowIndex) continue;
//                 
//                 if (!column[i].CellDataModel.IsMatched)
//                 {
//                     cellController = column[i];
//                     return true;
//                 }
//             }
//
//             return false;
//         }
//
//         private void CreateGridDecoy()
//         {
//             var rowCount = _data.IsActiveCellRows.Count;
//             for (var rowNum = 0; rowNum < rowCount; rowNum++)
//             {
//                 var columnCount = _data.IsActiveCellRows[rowNum].Row.Count;
//                 var createdCells = new List<RectTransform>(columnCount);
//                 for (var columnNum = 0; columnNum < columnCount; columnNum++)
//                 {
//                     var cellDecoy = new GameObject().AddComponent<RectTransform>();
//                     
//                     #if UNITY_EDITOR
//                     cellDecoy.name = "Pidor R: " + rowNum + "; C: " + columnNum;
//                     #endif
//                     
//                     cellDecoy.SetParent(decoyGrid);
//                     createdCells.Add(cellDecoy);
//                 }
//
//                 _decoyDictionary[rowNum] = createdCells;
//             }
//         }
//
//         private CellDataModel GetRandomModel()
//         {
//             var randomIndex = _random.Next(0, _data.CellModels.Count);
//             var foundModel = _data.CellModels[randomIndex];
//             return new CellDataModel(foundModel.PieceIcon, foundModel.PieceFace, foundModel.CellType,
//                 foundModel.RowColumnPair, Vector2.zero);
//         }
//
//         private bool IsSafeHorizontal(RowColumnPair rowColumnPair)
//         {
//             if (rowColumnPair.ColumnNum <= 1) return true;
//             
//             var gridRowCell = _gridRowCells[rowColumnPair.RowNum];
//
//             for (var i = 1; i < gridRowCell.Count - 2; i++)
//             {
//                 if (gridRowCell[i].CellType == gridRowCell[i - 1].CellType && gridRowCell[i] == gridRowCell[i + 1])
//                 {
//                     return false;
//                 }
//             }
//
//             return true;
//         }
//
//         private bool IsSafeVertical(RowColumnPair rowColumnPair)
//         {
//             if (rowColumnPair.RowNum <= 1) return true;
//
//             var column = GetColumn(rowColumnPair.ColumnNum);
//             
//             for (var i = 1; i < column.Count - 2; i++)
//             {
//                 if (column[i].CellType == column[i - 1].CellType && column[i].CellType == column[i + 1].CellType)
//                 {
//                     return true;
//                 }
//             }
//
//             return false;
//         }
//
//         private bool IsSafeHorizontalToSpawn(List<CellController> createdCells, CellType typeCellToCreat)
//         {
//             var count = createdCells.Count;
//             if (count < 2) return true;
//
//             if (createdCells[count - 1].CellType == typeCellToCreat)
//             {
//                 if (createdCells[count - 2].CellType == typeCellToCreat)
//                 {
//                     return false;
//                 }
//             }
//
//             return true;
//         }
//
//         private bool IsSafeVerticalToSpawn(CellDataModel cellModelToCreat)
//         {
//             if (cellModelToCreat.RowColumnPair.RowNum <= 1) return true;
//
//             if (TryGetCell(cellModelToCreat.RowColumnPair.RowNum - 1, cellModelToCreat.RowColumnPair.ColumnNum,
//                 out var previousCell))
//             {
//                 if (TryGetCell(cellModelToCreat.RowColumnPair.RowNum -2 , cellModelToCreat.RowColumnPair.ColumnNum,
//                     out var nextCell))
//                 {
//                     if (previousCell.CellType == cellModelToCreat.CellType &&
//                         nextCell.CellType == cellModelToCreat.CellType)
//                     {
//                         return false;
//                     }
//                 }
//             }
//
//             return true;
//         }
//
//         private bool GetIsCellActive(int rowNum, int columnNum, List<IsActiveCellRow> list)
//         {
//             var row = list[rowNum].Row;
//             return row[columnNum];
//         }
//
//
//         private void SwapCells(CellController semeCell, CellController ukeCell)
//         {
//             var semeRow = _gridRowCells[semeCell.CellDataModel.RowColumnPair.RowNum];
//             var ukeRow = _gridRowCells[ukeCell.CellDataModel.RowColumnPair.RowNum];
//
//             var tempUke = ukeCell.CellDataModel.RowColumnPair;
//             ukeCell.CellDataModel.ChangeRowColumn(semeCell.CellDataModel.RowColumnPair);
//             semeCell.CellDataModel.ChangeRowColumn(tempUke);
//
//             var semeIndex = semeRow.IndexOf(semeCell);
//             var ukeIndex = ukeRow.IndexOf(ukeCell);
//
//             semeRow.Remove(semeCell);
//             semeRow.Insert(semeIndex, ukeCell);
//
//             ukeRow.Remove(ukeCell);
//             ukeRow.Insert(ukeIndex, semeCell);
//         }
//
//         private async Task АааШестьКадроооов()
//         {
//             await Task.Yield();
//             await Task.Yield();
//             await Task.Yield();
//             await Task.Yield();
//             await Task.Yield();
//             await Task.Yield();
//         }
//     }
// }