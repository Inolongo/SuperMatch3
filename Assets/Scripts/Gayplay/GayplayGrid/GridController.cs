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
                    var piece = Instantiate(cellPrefab, cellContainer);
                    piece.Init(GetRandomModel());
                    createdCells.Add(piece);
                }

                _gridRowCells[i] = createdCells;
            }
        }

        private CellModel GetRandomModel()
        {
            var randomIndex = _random.Next(0, data.CellModels.Count - 1);
            return data.CellModels[randomIndex];
        }

        //ToDo: check for matching in horizontal and vertical axis
        private bool IsSafeHorizontal()
        {
            return false;
        }

        private bool IsSafeVertical()
        {
            return false;
        }
        
    }
}