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
        private bool _isMatched;

        public bool IsMatched
        {
            get => _isMatched;
            set
            {
                Debug.Log("Set is matched value = " + value + " | Row = " + RowColumnPair.RowNum + ";  Column = " + RowColumnPair.ColumnNum);
                _isMatched = value;
            }
        }

        public Sprite PieceIcon => pieceIcon;
        public Sprite PieceFace => pieceFace;

        public CellType CellType => cellType;

        public RowColumnPair RowColumnPair { get; private set; }
        public Vector2 Size { get; }

        public void ChangeRowColumn(int rowNum, int columnNum)
        {
            RowColumnPair = new RowColumnPair(rowNum, columnNum);
        }
        
        public void ChangeRowColumn(RowColumnPair rowColumnPair)
        {
            RowColumnPair = new RowColumnPair(rowColumnPair);
        }

        public CellDataModel(Sprite pieceIcon, Sprite pieceFace, CellType cellType, RowColumnPair rowColumnPair, Vector2 size)
        {
            Size = size;
            this.pieceIcon = pieceIcon;
            this.pieceFace = pieceFace;
            this.cellType = cellType;
            RowColumnPair = new RowColumnPair(rowColumnPair);
        }
    }
}