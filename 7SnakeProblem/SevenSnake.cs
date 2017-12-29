using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace _7SnakeProblem
{
	/// <summary>
	/// Represents a discovered incomplete (less than 7 points) or complete (7-points) snake.
	/// </summary>
	public sealed class SevenSnake
	{
		private const int MAX_SNAKE_LEN = 7;
		public HashSet<Point2D> AllPoints { get; set; }
		public int Sum { get; private set; }

		/// <summary>
		/// Indicates wether SevenSnake has all 7 points in it.
		/// </summary>
		public bool IsComplete => PointsCount == MAX_SNAKE_LEN;

		/// <summary>
		/// Number of points this snake contains
		/// </summary>
		public int PointsCount { get; private set; }

		public SevenSnake()
		{
			AllPoints = new HashSet<Point2D>();
		}

		public SevenSnake(SevenSnake snake) : this()
		{
			this.AllPoints = new HashSet<Point2D>(snake.AllPoints); 
			this.PointsCount = snake.PointsCount;
			this.Sum = snake.Sum;
		}

		/// <summary>
		/// Tries to extend snake with (x,y) point.
		/// </summary>
		/// <param name="prevPoint"></param>
		/// <param name="value"></param>
		/// <param name="point"></param>
		/// <returns>Returns 'false' when point violates adjacency rules and therefore cannot be added.</returns>
		public bool TryAddPoint(Point2D point, Point2D prevPoint, int value)
		{
			// Check if point (x,y) is already a part of the snake
			// And check that each cell ci, can only be adjacent to ci-1 or ci+1. Note that this exclude cycles.
			foreach (var existingPoint in AllPoints)
			{
				if (existingPoint != prevPoint)
				{
					//Check all prohibited adjecencies
					if (existingPoint == point.CreateLeft())
						return false;
					if (existingPoint == point.CreateRight())
						return false;
					if (existingPoint == point.CreateTop())
						return false;
					if (existingPoint == point.CreateBottom())
						return false;
				}
			}

			AllPoints.Add(point);
			PointsCount++;
			Sum += value;

			return true;
		}

		/// <summary>
		/// Checks if two snakes have any points they share.
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public bool IsOverlapWith(SevenSnake other)
		{
			if (other == null)
				throw new ArgumentNullException(nameof(other));

			Debug.Assert(this.PointsCount == MAX_SNAKE_LEN);
			Debug.Assert(other.PointsCount == MAX_SNAKE_LEN);
			return this.AllPoints.Overlaps(other.AllPoints);
		}

		private bool Equals(SevenSnake other)
		{
			return AllPoints.SequenceEqual(other.AllPoints)
				&& Sum == other.Sum
				&& PointsCount == other.PointsCount;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((SevenSnake)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = Sum;
				hashCode = (hashCode * 397) ^ PointsCount;

				return hashCode;
			}
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append($"{{Snake sum={Sum} points=");
			sb.Append("[");
			
			if (PointsCount>0)
			{
				var lastPoint = AllPoints.LastOrDefault();
				foreach (var point in AllPoints.TakeWhile(p=> p!= lastPoint))
				{
					sb.Append(point);
					sb.Append(", ");
				}

				sb.Append(AllPoints.LastOrDefault());
			}

			sb.Append("]");

			return sb.ToString();
		}
	}
}
