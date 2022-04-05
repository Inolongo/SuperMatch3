using Gayplay.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Gayplay.GayplayGrid
{
    public class CellView : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private Image face;

        public void Init(CellModel cellModel)
        {
            icon.sprite = cellModel.PieceIcon;
            face.sprite = cellModel.PieceFace;
        }
    }
}