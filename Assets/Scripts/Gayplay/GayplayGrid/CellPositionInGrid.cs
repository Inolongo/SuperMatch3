using System;

namespace Gayplay.GayplayGrid
{
    public readonly struct CellPositionInGrid : IEquatable<CellPositionInGrid>
    {
        public readonly int rowNum;
        public readonly int columnNum;

        public CellPositionInGrid(int rowNum, int columnNum)
        {
            this.rowNum = rowNum;
            this.columnNum = columnNum;
        }

        public CellPositionInGrid(CellPositionInGrid cellPositionInGrid)
        {
            rowNum = cellPositionInGrid.rowNum;
            columnNum = cellPositionInGrid.columnNum;
        }

        public bool Equals(CellPositionInGrid other)
        {
            return rowNum == other.rowNum && columnNum == other.columnNum;
        }

        public override bool Equals(object obj)
        {
            return obj is CellPositionInGrid other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(rowNum, columnNum);
        }

        public static bool operator ==(CellPositionInGrid first, CellPositionInGrid second) => first.Equals(second);

        public static bool operator !=(CellPositionInGrid first, CellPositionInGrid second) => !first.Equals(second);
    }
}