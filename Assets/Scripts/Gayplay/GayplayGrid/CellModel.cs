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
        public Vector2 Size { get; }

        public CellPositionInGrid CellPositionInGrid;

        public CellModel
        (
            bool isEmpty,
            Sprite pieceIcon,
            Sprite pieceFace,
            CellType cellType,
            Vector2 cellSize
        )
        {
            Size = cellSize;
            PieceFace = pieceFace;
            PieceIcon = pieceIcon;
            IsEmpty = isEmpty;
            CellType = cellType;
            CellPositionInGrid = new CellPositionInGrid(-1, -1);
        }
    }
}