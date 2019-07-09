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
        public List<Cell> Cell { get; set; }

        public string CellData
        {
            get
            {
                var arr = ConvertCellListToMatrixArray(Cell);

                var matrixStr = string.Empty;
                var lineArr = new int[Rows * Cols];
                var k = 0;
                for (var i = 0; i < 5; i++)
                {
                    for (var j = 0; j < 5; j++)
                    {
                        lineArr[k] = arr[i, j];
                        k++;
                    }
                }
                for (var i = 0; i < lineArr.Length; i++)
                {
                    matrixStr += lineArr[i];
                    if (i < Rows * Cols - 1)
                    {
                        matrixStr += ",";
                    }
                }
                return matrixStr;
            }
        }

        public bool Validate(List<Cell> cellsToValidate)
        {
            foreach (var cell in cellsToValidate)
            {
                var thisCell = Cell.Find(p => p.ColIndex == cell.ColIndex && p.RowIndex == cell.RowIndex);
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
            var arr = new int[5, 5];
            var tempArr = new int[Rows * Cols];

            var index = 0;
            for (var i = 0; i < tempArr.Length; i++)
            {
                tempArr[i] = int.Parse(tempArrStr[i]);
            }

            for (var i = 0; i < 5; i++)
            {
                for (var j = 0; j < 5; j++)
                {
                    arr[i, j] = tempArr[index];
                    index++;
                }
            }

            FillCellData(arr);
        }

        private void FillCellData(int[,] array)
        {
            var cells = new List<Cell>();
            var lineArr = new int[Rows * Cols];
            var k = 0;
            for (var i = 0; i < 5; i++)
            {
                for (var j = 0; j < 5; j++)
                {
                    lineArr[k] = array[i, j];
                    var c = new Cell(i, j) { Value = lineArr[k] };
                    cells.Add(c);
                    k++;
                }
            }
            Cell = cells;
        }

        public Card(int rows = 5, int cols = 5)
        {
            Id = Guid.NewGuid();

            Rows = rows;
            Cols = cols;

            var arr = GenerateRandomMatrix(rows, cols);
            FillCellData(arr);
        }

        private static int[,] ConvertCellListToMatrixArray(IReadOnlyList<Cell> cells)
        {
            var arr = new int[5, 5];
            var k = 0;
            for (var i = 0; i < 5; i++)
            {
                for (var j = 0; j < 5; j++)
                {
                    arr[i, j] = cells[k].Value;
                    k++;
                }
            }
            return arr;
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
    }
}
