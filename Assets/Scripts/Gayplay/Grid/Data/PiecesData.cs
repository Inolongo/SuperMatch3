using System;
using System.Collections.Generic;
using Gayplay.Gameplay.Cell;
using UnityEngine;

namespace Gayplay.Data
{
    //TODO:refactoring
    [CreateAssetMenu(fileName = "PiecesData", menuName = "ScriptableObjects/Pieces", order = 1)]
    public class PiecesData : ScriptableObject
    {
        [SerializeField] private List<CellDataModel> cellModels;
        [SerializeField] private List<IsActiveCellRow> isActiveCellsLevel;
        [field: SerializeField] public CellController CellPrefab { get; private set; }

        public List<CellDataModel> CellModels => cellModels;
        public List<IsActiveCellRow> IsActiveCellRows => isActiveCellsLevel;
    }

    [Serializable]
    public struct IsActiveCellRow
    {
        [SerializeField] private List<bool> row;

        public List<bool> Row => row;
    }
}