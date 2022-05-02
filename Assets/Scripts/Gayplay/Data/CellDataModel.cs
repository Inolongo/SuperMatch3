using System;
using UnityEngine;

namespace Gayplay.Data
{
    [Serializable]
    public class CellDataModel
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

        public CellDataModel(Sprite pieceIcon, Sprite pieceFace, CellType cellType, RowColumnPair rowColumnPair)
        {
            this.pieceIcon = pieceIcon;
            this.pieceFace = pieceFace;
            this.cellType = cellType;
            RowColumnPair = new RowColumnPair(rowColumnPair);
        }
    }
}