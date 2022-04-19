using System.Collections.Generic;
using Gayplay.Data;
using UnityEngine;
using Random = System.Random;

namespace Gayplay.GayplayGrid
{
    public class GridController : MonoBehaviour
    {
        [SerializeField] private CellController cellPrefab;
        [SerializeField] private Transform cellContainer;

        private PiecesData data;

        private readonly Dictionary<int, List<CellController>> _gridRowCells =
            new Dictionary<int, List<CellController>>();

        private readonly Random _random = new Random();

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            InitData();
            CreateGrid();
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
                    var tempModel = GetRandomModel();
                    if (IsSafeHorizontal(createdCells, columnNum, tempModel) &&
                        IsSafeVertical(rowNum, columnNum, tempModel))
                    {
                        var piece = Instantiate(cellPrefab, cellContainer);
                        tempModel.IsActive = GetIsCellActive(rowNum, columnNum, data.IsActiveCellRows);
                        piece.Init(tempModel);
                        createdCells.Add(piece);
                    }
                    else
                    {
                        columnNum -= 1;
                    }
                }

                _gridRowCells[rowNum] = createdCells;
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

        private bool IsSafeHorizontal(List<CellController> createdCells, int columnNum, CellDataModel tempDataModel)
        {
            if (createdCells.Count <= 1) return true;

            return createdCells[columnNum - 1].CellType != createdCells[columnNum - 2].CellType ||
                   tempDataModel.CellType != createdCells[columnNum - 1].CellType;
        }

        private bool IsSafeVertical(int rowNum, int columnNum, CellDataModel tempDataModel)
        {
            if (rowNum <= 1) return true;

            var tempList1 = _gridRowCells[rowNum - 1];
            var tempList2 = _gridRowCells[rowNum - 2];

            return tempList1[columnNum].CellType != tempDataModel.CellType ||
                   tempList1[columnNum].CellType != tempList2[columnNum].CellType;
        }
    }
}