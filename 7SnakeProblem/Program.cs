using System;
using System.IO;
using System.Threading;

namespace _7SnakeProblem
{
	//NOTE: In the project's setting the program is set to run with this argument: ".\Samples\sample1.csv"

	class Program
	{
		// Slows down the visualization and the algorithm itself
		private const int VISUALIZATION_DELAY = 1;

		static void Main(string[] args)
		{
			try
			{
				if (args.Length != 1)
				{
					//Print help
					Console.WriteLine("Usage: 7SnakeProblem.exe <file>");
					return;
				}

				string filePath = args[0];
				Console.WriteLine("Loading grid data from file: " + filePath);
				if (!File.Exists(filePath))
				{
					Console.WriteLine("Error: File not found.");
					return;
				}

				ushort[,] grid = CSVFileService.Load(filePath); // [row,col]
				var solver = new SnakeSolver(grid);

				(SevenSnake snake1, SevenSnake snake2) = solver.Solve(snake => PrintSnakeVisual(grid, snake, delay: VISUALIZATION_DELAY));

				//iii)	If no such pair exists the program should output ‘FAIL’. 
				if (snake1 == null || snake2 == null)
				{
					Console.WriteLine("FAIL");
					return;
				}

				Console.Clear();
				Console.WriteLine("Visualization of the solution:");

				//Solution found:
				PrintSnakeVisual(grid, snake1, cls: false);
				PrintSnakeVisual(grid, snake2, cls: false);

				Console.WriteLine($"Solution: {snake1} \n      AND {snake2}");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}

			Console.WriteLine("Press any key to exit...");
			Console.ReadLine();
		}

		/// <summary>
		/// Visualizes grid with a snake on it. Snake is drawn as 'X'-s.
		/// </summary>
		/// <param name="grid"></param>
		/// <param name="snake">Snake to highlight</param>
		/// <param name="delay">Delay in milliseconds</param>
		/// <param name="cls">Clear screen before priniting out</param>
		public static void PrintSnakeVisual(ushort[,] grid, SevenSnake snake, int delay = 0, bool cls = true)
		{
			int N = grid.GetUpperBound(0) + 1;
			if (cls)
			{
				Console.Clear();
			}

			Console.WriteLine("");
			for (int i = 0; i < N; i++) //row
			{
				for (int j = 0; j < N - 1; j++) //col
				{
					if (snake.AllPoints.Contains(new Point2D(j, i)))
					{
						Console.ForegroundColor = ConsoleColor.Red;
						Console.Write(" X ");
						Console.ResetColor();
					}
					else
					{
						Console.Write(" O ");
					}
				}
				Console.WriteLine("");
			}
			Console.WriteLine("");

			Thread.Sleep(delay);
		}
	}
}
