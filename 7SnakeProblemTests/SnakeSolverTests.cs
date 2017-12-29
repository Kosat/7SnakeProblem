using System;
using System.IO;
using Xunit;
using _7SnakeProblem;

namespace _7SnakeProblemTests
{
    public class SnakeSolverTests
    {
	    [Fact]
	    public void SnakeSolver_Sample1()
	    {
		    //Arrange
			var sampleGrid = CSVFileService.Load(GetSampleFilePath("sample1.csv"));
			var solver = new SnakeSolver(sampleGrid);

		    //Act
			(var snake1, var snake2) = solver.Solve();

		    //Assert
			Assert.Equal(snake1.Sum, snake2.Sum);
	    }

	    [Fact]
	    public void SnakeSolver_Sample2()
	    {
			//Arrange
		    var sampleGrid = CSVFileService.Load(GetSampleFilePath("sample2.csv"));
		    var solver = new SnakeSolver(sampleGrid);

			//Act
		    (var snake1, var snake2) = solver.Solve();

			//Assert
		    Assert.Equal(snake1.Sum, snake2.Sum);
	    }

	    [Fact]
	    public void SnakeSolver_Sample3()
	    {
		    //Arrange
			var sampleGrid = CSVFileService.Load(GetSampleFilePath("sample3.csv"));
		    var solver = new SnakeSolver(sampleGrid);

			//Act
			(var snake1, var snake2) = solver.Solve();

		    //Assert
			Assert.Equal(snake1.Sum, snake2.Sum);
	    }

	    [Fact]
	    public void SnakeSolver_Randomized()
	    {
			//Arrange
		    ushort[,] grid = new ushort[100, 100];

		    int N = grid.GetUpperBound(0) + 1;

		    var random = new Random();
		    var solver = new SnakeSolver(grid);

			int k = 100;
			while (k > 0)
		    {
			    
			    for (int i = 0; i < N; i++)
			    {
				    for (int j = 0; j < N; j++)
				    {
					    grid[i, j] = (ushort)random.Next(100, 256);
				    }
			    }

				//Act
				(var snake1, var snake2) = solver.Solve();

			    //Assert
				Assert.Equal(snake1.Sum, snake2.Sum);

				k--;

			}
	    }

		string GetSampleFilePath(string fileName)
	    {
		    return Path.Combine(@"Samples", fileName);
	    }
    }
}
