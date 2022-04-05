using Gayplay.Data;
using UnityEngine;

namespace Gayplay.GayplayGrid
{
    public class CellController : MonoBehaviour
    {
        [SerializeField] private CellView cellView;
        
        public CellType CellType => _cellModel.CellType;
        
        private CellModel _cellModel;

        public void Init(CellModel cellModel)
        {
            _cellModel = cellModel;
            cellView.Init(cellModel);
        }
    }
}