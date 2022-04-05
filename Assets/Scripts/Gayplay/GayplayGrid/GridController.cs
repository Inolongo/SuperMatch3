using System.Collections.Generic;
using Gayplay.Data;
using UnityEngine;
using Random = System.Random;

namespace Gayplay.GayplayGrid
{
    public class GridController : MonoBehaviour
    {
        private const int RowCount = 4;
        private const int ColumnCount = 4;

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
            for (var i = 0; i < RowCount; i++)
            {
                var createdCells = new List<CellController>(ColumnCount);
                for (var j = 0; j < ColumnCount; j++)
                {
                    var tempModel = GetRandomModel();
                    if (IsSafeHorizontal(createdCells, j, tempModel) &&
                        IsSafeVertical(i, j, tempModel))
                    {
                        var piece = Instantiate(cellPrefab, cellContainer);
                        piece.Init(tempModel);
                        createdCells.Add(piece);
                    }
                    else
                    {
                        j -= 1;
                    }
                }

                _gridRowCells[i] = createdCells;
            }
        }

        private CellModel GetRandomModel()
        {
            var randomIndex = _random.Next(0, data.CellModels.Count);
            return data.CellModels[randomIndex];
        }

        private bool IsSafeHorizontal(List<CellController> createdCells, int columnNum, CellModel tempModel)
        {
            if (createdCells.Count <= 1) return true;
            
            return createdCells[columnNum - 1].CellType != createdCells[columnNum - 2].CellType ||
                   tempModel.CellType != createdCells[columnNum - 1].CellType;
        }

        private bool IsSafeVertical(int rowNum, int columnNum, CellModel tempModel)
        {
            if (rowNum <= 1) return true;
            
            var tempList1 = _gridRowCells[rowNum - 1];
            var tempList2 = _gridRowCells[rowNum - 2];
            
            return tempList1[columnNum].CellType != tempModel.CellType ||
                   tempList1[columnNum].CellType != tempList2[columnNum].CellType;
        }
    }
}