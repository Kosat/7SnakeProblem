using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace _7SnakeProblem
{
	/// <summary>
	/// Solves the 7-snakes problem
	/// </summary>
	public class SnakeSolver
	{
		private readonly ushort[,] _grid;

		public SnakeSolver(ushort[,] grid)
		{
			if (grid == null)
				throw new ArgumentNullException(nameof(grid));

			_grid = grid;
		}

		/// <summary>
		/// Finds the two 7-snakes having equal sums.
		/// </summary>
		/// <returns>Pair of different snakes having equal sums</returns>
		public (SevenSnake, SevenSnake) Solve(Action<SevenSnake> progress = null)
		{
			// Grid dimensions NxN
			int N = _grid.GetUpperBound(0) + 1;

			// Indexes all discovered 7-snakes by their sums
			var snakesDiscovered = new Dictionary<int /*sum*/, /*overlapping snakes*/List<SevenSnake>>();

			//Traverse Grid Left-to-Right and Top-to-Bottom
			//For each point (row,col) check all the permutations in Q4 (Fourth quadrant) relatively to the point
			//Store all found snakes in 'snakesDiscovered' index
			for (int row = 0; row < N; row++)
			{
				for (int col = 0; col < N; col++) // 2-nested-fors O(N^2)
				{
					var snakes = GetSnakePermutations(row, col); // recursive but O(1)

					foreach (var newSnake in snakes) // 64 snakes or less
					{
						Debug.Assert(snakes.Count()<=64);
						
						//Report progress
						progress?.Invoke(newSnake);
						
						if (UpdateIndex(snakesDiscovered, newSnake, out SevenSnake existingSnake))
							return (newSnake, existingSnake); // Found a solution
					}
				}
			}

			return (null, null);
		}

		/// <summary>
		/// Updates snakes index. Returns true if non-overlaping snakes with equal sums were found.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="newSnake"></param>
		/// <param name="existingSnake">One of the existing snakes having the same sum as the newSnake</param>
		/// <returns></returns>
		private bool UpdateIndex(Dictionary<int, List<SevenSnake>> index, SevenSnake newSnake, out SevenSnake existingSnake)
		{
			if (index.ContainsKey(newSnake.Sum)) //O(1) amortized
			{
				IList<SevenSnake> existingSnakes = index[newSnake.Sum]; //O(1) amortized

				foreach (var snake in existingSnakes)
				{
					//i)	The two 7-Snakes must be distinct. They cannot share cells.
					if (!snake.IsOverlapWith(newSnake))
					{
						//ii)	In general there may be more than one pair of 7-Snakes with the required property. 
						// Your program need only find one pair.
						existingSnake = snake;
						return true;
					}
				}

				existingSnakes.Add(newSnake);
			}
			else
			{
				index.Add(newSnake.Sum, new List<SevenSnake>() {newSnake});
			}

			existingSnake = null;
			return false;
		}

		/// <summary>
		/// Recursively checks all possible permutations of snakes starting at (row, col).
		/// </summary>
		/// <param name="row"></param>
		/// <param name="col"></param>
		/// <returns></returns>
		/// <remarks>Recursion depth is limited to 7</remarks>
		private IEnumerable<SevenSnake> GetSnakePermutations(int row, int col)
		{
			return GetSnakePermutationsInternal(new Point2D(col, row), new Point2D(-1, -1), new SevenSnake());
		}

		/// <summary>
		/// Given a starting point (x,y) this method recursively checks all possible permutations of snakes 
		/// starting at (x,y) and satisfying adjacency rules.
		/// </summary>
		/// <param name="point">Current Point</param>
		/// <param name="prevPoint">Previous Point of the snake</param>
		/// <param name="snake">Snake to process</param>
		/// <returns>Collection of discovered snakes starting at <see cref="point"/></returns>
		private IEnumerable<SevenSnake> GetSnakePermutationsInternal(Point2D point, Point2D prevPoint, SevenSnake snake)
		{
			// Check the grid coordinate bounds
			// If point (x,y) is outside the grid, return.
			if (point.X > _grid.GetUpperBound(0)
			    || point.Y > _grid.GetUpperBound(1)
			    || point.X < 0 || point.Y < 0)
			{
				return new List<SevenSnake>();
			}

			if (!snake.TryAddPoint(point, prevPoint, _grid[point.Y/*row*/, point.X/*col*/]))
			{
				//This happen when current point breaks adjacency rules. Finish recursion.
				return new List<SevenSnake>();
			}

			// If snake becomes complete (7-points long) finish recursion.
			if (snake.IsComplete)
			{
				return new List<SevenSnake>() { snake };
			}

			//Check Right
			IEnumerable<SevenSnake> right = GetSnakePermutationsInternal(point.CreateRight(), point, new SevenSnake(snake));

			//Check Bottom
			IEnumerable<SevenSnake> bottom = GetSnakePermutationsInternal(point.CreateBottom(), point, new SevenSnake(snake));

			//Combines all discovered permutations of snakes of length 7 that start/end at (x,y) point
			return Enumerable.Empty<SevenSnake>().Union(right).Union(bottom);
		}

	}
}
