using System.Collections.Generic;
using UnityEngine;

namespace Gayplay.Data
{
    [CreateAssetMenu(fileName = "PiecesData", menuName = "ScriptableObjects/Pieces", order = 1)]
    public class PiecesData : ScriptableObject
    {
        [SerializeField] private List<CellModel> cellModels;

        public List<CellModel> CellModels => cellModels;
    }
}