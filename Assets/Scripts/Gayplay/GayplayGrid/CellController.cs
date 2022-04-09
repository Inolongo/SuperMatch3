using Gayplay.Data;
using UnityEngine;

namespace Gayplay.GayplayGrid
{
    public class CellController : MonoBehaviour
    {
        [SerializeField] private CellView cellView;
        
        public CellType CellType => _cellDataModel.CellType;
        
        private CellDataModel _cellDataModel;

        public void Init(CellDataModel cellDataModel)
        {
            _cellDataModel = cellDataModel;
            cellView.Init(cellDataModel);
        }
    }
}