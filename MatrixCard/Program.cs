using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace MatrixCard
{
    class Program
    {
        static void Main(string[] args)
        {
            var card = new Card(6, 5).GenerateData();
            Console.WriteLine($"Generated Matrix Card:");

            PrintCard(card);
            Console.WriteLine(Environment.NewLine);

            Console.WriteLine($"Matrix Card Data: {Environment.NewLine}{JsonConvert.SerializeObject(card, Formatting.Indented)}");
            Console.WriteLine(Environment.NewLine);

            Console.WriteLine("Load Data:");
            var card2 = new Card(6,5);
            card2.LoadCellData(card.CellData);
            PrintCard(card2);
            Console.WriteLine(Environment.NewLine);

            Console.WriteLine("Matrix Card Validation:");
            var cellsToValidate = card.PickRandomCells(3).ToList();
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

        private static void PrintCard(Card card)
        {
            Console.WriteLine("  | A\tB\tC\tD\tE");
            Console.WriteLine("-------------------------------------------");
            var i = 0;
            for (var k = 0; k < card.Rows; k++)
            {
                Console.Write(k + " | ");
                for (var l = 0; l < card.Cols; l++)
                {
                    Console.Write(card.Cells[i].Value + "\t");
                    i++;
                }

                Console.WriteLine();
            }
        }
    }
}
