using System;
using UnityEngine;

namespace Gayplay.Data
{
    [Serializable]
    public struct CellModel
    {
        [SerializeField] private Sprite pieceIcon;
        [SerializeField] private Sprite pieceFace;
        [SerializeField] private string pieceType;

        public Sprite PieceIcon => pieceIcon;
        public Sprite PieceFace => pieceFace;

        public string PieceType => pieceType;
    }
}