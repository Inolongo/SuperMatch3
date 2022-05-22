namespace Gayplay.GayplayGrid
{
    public struct CellPositionInGrid
    {
        public int RowNum;
        public int ColumnNum;

        public CellPositionInGrid(int rowNum, int columnNum)
        {
            RowNum = rowNum;
            ColumnNum = columnNum;
        }
    }
}