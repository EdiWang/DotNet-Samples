using System;
using System.Collections.Generic;
using System.Text;

namespace MatrixCard
{
    class Program
    {
        static void Main(string[] args)
        {
            var card = new Card();
            Console.WriteLine($"Generated Matrix Card with Id '{card.Id}':");

            Console.WriteLine("  | A\tB\tC\tD\tE");
            Console.WriteLine("-------------------------------------------");

            var i = 0;
            for (var k = 0; k < card.Rows; k++)
            {
                Console.Write(k + " | ");
                for (var l = 0; l < card.Cols; l++)
                {
                    Console.Write(card.Cell[i].Value + "\t");
                    i++;
                }
                Console.WriteLine();
            }

            Console.WriteLine(Environment.NewLine);

            Console.WriteLine("Matrix Card Validation:");
            var cellsToValidate = card.PickRandomCells(3);

            var sb = new StringBuilder();
            foreach (var t in cellsToValidate)
            {
                sb.Append($"[{t.ColumnName}{t.RowIndex}] ");
            }
            Console.WriteLine($"Please Input Card Number At {sb}:");
            var userInput = Console.ReadLine();
            if (userInput != null)
            {
                var inputArr = userInput.Split(",");

                var isValid = card.Validate(new List<Cell>
                {
                    new Cell(cellsToValidate[0].RowIndex, cellsToValidate[0].ColIndex, int.Parse(inputArr[0])),
                    new Cell(cellsToValidate[1].RowIndex, cellsToValidate[1].ColIndex, int.Parse(inputArr[1])),
                    new Cell(cellsToValidate[2].RowIndex, cellsToValidate[2].ColIndex, int.Parse(inputArr[2]))
                });

                Console.WriteLine(isValid);
            }

            Console.ReadKey();
        }

    }
}
