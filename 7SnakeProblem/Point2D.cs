namespace _7SnakeProblem
{
	/// <summary>
	/// Point struct that represents point coordinates (x,y) on grid.
	/// </summary>
	public struct Point2D
	{
		private readonly int x;
		private readonly int y;

		public int X => x;
		public int Y => y;

		public Point2D(int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		/// <summary>
		/// Returns corresponding adjacent points
		/// </summary>
		/// <returns></returns>
		public Point2D CreateLeft() => new Point2D(x - 1, y);
		public Point2D CreateRight() => new Point2D(x + 1, y);
		public Point2D CreateTop() => new Point2D(x, y - 1);
		public Point2D CreateBottom() => new Point2D(x, y + 1);

		public static bool operator ==(Point2D a, Point2D b)
		{
			return a.x == b.x && a.y == b.y;
		}
		public static bool operator !=(Point2D a, Point2D b)
		{
			return !(a == b);
		}
		
		public bool Equals(Point2D other)
		{
			return x == other.x && y == other.y;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			return obj is Point2D && Equals((Point2D)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (x * 397) ^ y;
			}
		}

		public override string ToString()
		{
			return $"({x},{y})";
		}
	}
}