using System;
using Gayplay.Data;
using MVC;
using UnityEngine;

namespace Gayplay.Gameplay.Cell
{
    public class CellModel : IModel
    {
        public bool IsEmpty { get; }
        public Sprite PieceIcon { get; }
        public Sprite PieceFace { get; }
        public CellType CellType { get; }
        public Vector2 Size { get; }

        public CellPositionInGrid CellPositionInGrid { get; set; }

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