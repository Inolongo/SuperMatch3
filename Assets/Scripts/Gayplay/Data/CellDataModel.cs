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

        public Sprite PieceIcon => pieceIcon;
        public Sprite PieceFace => pieceFace;

        public CellType CellType => cellType;

        public RowColumnPair RowColumnPair { get; private set; }

        public void ChangeRowColumn(int rowNum, int columnNum)
        {
            RowColumnPair = new RowColumnPair(rowNum, columnNum);
        }
        
        public void ChangeRowColumn(RowColumnPair rowColumnPair)
        {
            RowColumnPair = new RowColumnPair(rowColumnPair);
        }
    }
}