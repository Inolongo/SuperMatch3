namespace Gayplay.Data
{
    public struct RowColumnPair
    {
        public int ColumnNum;
        public int RowNum;

        public RowColumnPair(int rowNum, int columnNum)
        {
            ColumnNum = columnNum;
            RowNum = rowNum;
        }
        
        public RowColumnPair(RowColumnPair rowColumnPair)
        {
            ColumnNum = rowColumnPair.ColumnNum;
            RowNum = rowColumnPair.RowNum;
        }
    }
}