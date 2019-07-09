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
            WriteMessage($"Generate Matrix Card:", ConsoleColor.Yellow);
            var card = new Card().GenerateData();
            PrintCard(card);

            Console.WriteLine($"{JsonConvert.SerializeObject(card, Formatting.Indented)}");
            Console.WriteLine();

            WriteMessage("Load Data:", ConsoleColor.Yellow);
            var card2 = new Card().LoadCellData(card.CellData);
            PrintCard(card2);
            Console.WriteLine();

            WriteMessage("Matrix Card Validation:", ConsoleColor.Yellow);
            var cellsToValidate = card.PickRandomCells(3).ToList();
            var sb = new StringBuilder();
            foreach (var t in cellsToValidate)
            {
                sb.Append($"[{t.ColumnName}{t.RowIndex}] ");
            }
            Console.WriteLine($"Please input number(s) at {sb} (use ',' to seperate values):");
            var userInput = Console.ReadLine();
            if (userInput != null)
            {
                var inputArr = userInput.Split(",");
                if (inputArr.Length != cellsToValidate.Count)
                {
                    WriteMessage($"Invalid input, numbers doesn't match, must input {cellsToValidate.Count} numbers.", ConsoleColor.Red);
                }
                else
                {
                    var isValid = card.Validate(new List<Cell>
                    {
                        new Cell(cellsToValidate[0].RowIndex, cellsToValidate[0].ColIndex, int.Parse(inputArr[0])),
                        new Cell(cellsToValidate[1].RowIndex, cellsToValidate[1].ColIndex, int.Parse(inputArr[1])),
                        new Cell(cellsToValidate[2].RowIndex, cellsToValidate[2].ColIndex, int.Parse(inputArr[2]))
                    });
                    WriteMessage(isValid.ToString(), isValid ? ConsoleColor.Green : ConsoleColor.Red);
                }
            }

            Console.ReadKey();
        }

        private static void PrintCard(Card card)
        {
            Console.WriteLine("==============================================");
            Console.WriteLine("  |\tA\tB\tC\tD\tE\t");
            Console.WriteLine("----------------------------------------------");
            var i = 0;
            for (var k = 0; k < card.Rows; k++)
            {
                Console.Write(k + " |\t");
                for (var l = 0; l < card.Cols; l++)
                {
                    Console.Write(card.Cells[i].Value + "\t");
                    i++;
                }

                Console.WriteLine();
            }
            Console.WriteLine("==============================================");
        }

        static void WriteMessage(string message, ConsoleColor color = ConsoleColor.White, bool resetColor = true)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            if (resetColor)
            {
                Console.ResetColor();
            }
        }
    }
}
