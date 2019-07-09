using System;
using System.Collections.Generic;
using System.Text;

namespace MatrixCard
{
    public class Cell
    {
        public int RowIndex { get; }

        public int ColIndex { get; }

        public ColumnCode ColumnName => (ColumnCode)ColIndex;

        public int Value { get; set; }

        public Cell(int rowIndex, int colIndex, int val = 0)
        {
            RowIndex = rowIndex;
            ColIndex = colIndex;
            Value = val;
        }
    }
}
