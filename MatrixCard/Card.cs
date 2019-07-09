using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MatrixCard
{
    public class Card
    {
        public Guid Id { get; set; }

        public int Rows { get; set; }

        public int Cols { get; set; }

        [JsonIgnore]
        public List<Cell> Cells { get; set; }

        public string CellData
        {
            get
            {
                var arr = new int[Rows, Cols];
                var k = 0;
                for (var i = 0; i < Rows; i++)
                {
                    for (var j = 0; j < Cols; j++)
                    {
                        arr[i, j] = Cells[k].Value;
                        k++;
                    }
                }

                var lineArr = new int[Rows * Cols];
                var index = 0;
                for (var row = 0; row < Rows; row++)
                {
                    for (var col = 0; col < Cols; col++)
                    {
                        lineArr[index] = arr[row, col];
                        index++;
                    }
                }

                return string.Join(',', lineArr);
            }
        }

        public Card(int rows = 5, int cols = 5)
        {
            Id = Guid.NewGuid();

            Rows = rows;
            Cols = cols;
        }

        public Card GenerateData()
        {
            var arr = GenerateRandomMatrix(Rows, Cols);
            FillCellData(arr);
            return this;
        }

        public bool Validate(List<Cell> cellsToValidate)
        {
            foreach (var cell in cellsToValidate)
            {
                var thisCell = Cells.Find(p => p.ColIndex == cell.ColIndex && p.RowIndex == cell.RowIndex);
                var matches = thisCell.Value == cell.Value;
                if (!matches)
                {
                    return false;
                }
            }
            return true;
        }

        public List<Cell> PickRandomCells(int howMany)
        {
            var r = new Random();
            var cells = new List<Cell>();
            for (var i = 0; i < howMany; i++)
            {
                var randomCol = r.Next(0, Cols);
                var randomRow = r.Next(0, Rows);
                var c = new Cell(randomRow, randomCol);
                cells.Add(c);
            }
            return cells;
        }

        public void LoadCellData(string strMatrix)
        {
            var tempArrStr = strMatrix.Split(',');
            var arr = new int[Rows, Cols];
            var tempArr = new int[Rows * Cols];

            var index = 0;
            for (var i = 0; i < tempArr.Length; i++)
            {
                tempArr[i] = int.Parse(tempArrStr[i]);
            }

            for (var row = 0; row < Rows; row++)
            {
                for (var col = 0; col < Cols; col++)
                {
                    arr[row, col] = tempArr[index];
                    index++;
                }
            }

            FillCellData(arr);
        }

        #region Private Methods

        private void FillCellData(int[,] array)
        {
            var cells = new List<Cell>();
            var lineArr = new int[Rows * Cols];
            var index = 0;
            for (var row = 0; row < Rows; row++)
            {
                for (var col = 0; col < Cols; col++)
                {
                    lineArr[index] = array[row, col];
                    var c = new Cell(row, col) { Value = lineArr[index] };
                    cells.Add(c);
                    index++;
                }
            }
            Cells = cells;
        }

        private static int[,] GenerateRandomMatrix(int rows, int cols)
        {
            var r = new Random();
            var arr = new int[rows, cols];
            for (var row = 0; row < rows; row++)
            {
                for (var col = 0; col < cols; col++)
                {
                    arr[row, col] = r.Next(0, 100);
                }
            }
            return arr;
        }

        #endregion
    }
}
