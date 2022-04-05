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

        private Random _random = new Random();

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
                        j = j - 1;
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
        
        private bool IsSafeHorizontal(List<CellController> createdCells, int j, CellModel tempModel)
        {
            if (createdCells.Count > 1)
            {
                if (createdCells[j - 1].cellView.type == createdCells[j - 2].cellView.type &&
                    tempModel.PieceType == createdCells[j - 1].cellView.type)
                {
                    return false;
                }

                return true;
            }

            return true;
        }

        private bool IsSafeVertical(int i, int j, CellModel tempModel)
        {
            if (i > 1)
            {
                var tempList1 = _gridRowCells[i - 1];
                var tempList2 = _gridRowCells[i - 2];
                if (tempList1[j].cellView.type == tempModel.PieceType &&
                    tempList1[j].cellView.type == tempList2[j].cellView.type)
                {
                    return false;
                }

                return true;
            }

            return true;
        }
    }
}