using System;
using UnityEngine;

namespace Gayplay.Data
{
    [Serializable]
    public struct CellDataModel
    {
        [SerializeField] private Sprite pieceIcon;
        [SerializeField] private Sprite pieceFace;
        [SerializeField] private CellType cellType;

        public bool IsActive { get; set; }
        public Sprite PieceIcon => pieceIcon;
        public Sprite PieceFace => pieceFace;

        public CellType CellType => cellType;
    }
}