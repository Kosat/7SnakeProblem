using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace _7SnakeProblem
{
	/// <summary>
	/// Loads CSV files with input data
	/// </summary>
	public class CSVFileService
	{
		/// <summary>
		/// Loads grid from CVS file
		/// </summary>
		/// <returns></returns>
		public static ushort[,] Load(string filePath)
		{
			try
			{
				using (var file = new StreamReader(filePath))
				{
					int linesRead = 0;
					string line = file.ReadLine();

					if (string.IsNullOrWhiteSpace(line))
						throw new CSVFileServiceException("File format exception. File is empty.");

					// Assume that file contains [N,N] grid where N is a number of integers found in the first line.
					// Alternatively, I could just add N as the first line of CSV file but the task doesn't say if I can do it.
					var N = Tokenizer(line).Count();

					// There's no requirement about time and space complexities so I assume I can load the entire grid into memory. 
					// Because task says that grid may contain integers 0..256 I couldn't use 'byte' as it is 0..255
					// v)	In general, the input grid can be any (square) size. 
					ushort[,] grid = new ushort[N, N];

					while (line != null)
					{
						linesRead++;

						int col = 0;
						foreach (var item in Tokenizer(line))
						{
							if (!ushort.TryParse(item, out ushort num))
								throw new CSVFileServiceException($"File format exception. Failed to parse '{item}'. Expected integer 0..256.");

							if(num > 256)
								throw new CSVFileServiceException($"File format exception. Integer {num} is too big. Expected integer 0..256.");

							grid[linesRead - 1, col] = num;
							col++;
						}

						line = file.ReadLine();
					}

					// v)	In general, the input grid can be any (square) size. 
					if (linesRead != N)
						throw new CSVFileServiceException($"File format exception. Read {linesRead} lines while expected {N}.");

					return grid;
				}
			}
			catch (CSVFileServiceException)
			{
				throw;
			}
			catch (Exception ex)
			{
				throw new CSVFileServiceException("Failed to load CVS file.", ex);
			}
		}

		/// <summary>
		/// KDEBUG
		/// </summary>
		/// <param name="grid"></param>
		public static void Save(int[,] grid)
		{
			var csv = new StringBuilder();
			int N = grid.GetUpperBound(0) + 1;

			for (int i = 0; i < N; i++) //row
			{
				for (int j = 0; j < N -1; j++) //col
				{
					csv.Append(grid[i, j]);
					csv.Append(",");
				}

				csv.Append(grid[i, N - 1]);
				csv.AppendLine();
			}

			File.WriteAllText(
				DateTime.Now.Minute.ToString() + "_" + 
				DateTime.Now.Second.ToString() + "_" +
				DateTime.Now.Millisecond.ToString() + ".csv", csv.ToString());
		}

		/// <summary>
		/// Returns values from CSV string
		/// </summary>
		public static IEnumerable<string> Tokenizer(string str, char delimiter = ',')
		{
			int shipStartIdx = 0;
			for (int i = 0; i < str.Length; i++)
			{
				char ch = str[i];
				if (ch == delimiter)
				{
					yield return str.Substring(shipStartIdx, i - shipStartIdx);
					shipStartIdx = i + 1;
				}
			}

			yield return str.Substring(shipStartIdx, str.Length - shipStartIdx);
		}
	}
}
