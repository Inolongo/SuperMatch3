using Gayplay.Data;
using UnityEngine;

namespace Gayplay.GayplayGrid
{
    public struct CellModel
    {
        public bool IsEmpty { get; }
        public Sprite PieceIcon { get; }
        public Sprite PieceFace { get; }

        public CellType CellType { get; }

        public Vector2 CellSize { get; }
        public CellModel(bool isEmpty, Sprite pieceIcon, Sprite pieceFace, CellType cellType, Vector2 cellSize)
        {
            CellSize = cellSize;
            PieceFace = pieceFace;
            PieceIcon = pieceIcon;
            IsEmpty = isEmpty;
            CellType = cellType;
        }
    }
}