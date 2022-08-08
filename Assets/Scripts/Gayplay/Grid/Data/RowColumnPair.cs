namespace Gayplay.Data
{
    public struct RowColumnPair
    {
        public readonly int columnNum;
        public readonly int rowNum;

        public RowColumnPair(int rowNum, int columnNum)
        {
            this.columnNum = columnNum;
            this.rowNum = rowNum;
        }
        
        public RowColumnPair(RowColumnPair rowColumnPair)
        {
            columnNum = rowColumnPair.columnNum;
            rowNum = rowColumnPair.rowNum;
        }
    }
}