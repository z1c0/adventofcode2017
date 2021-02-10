using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

// Day 19

Console.WriteLine("START");
var sw = Stopwatch.StartNew();
Part1();
Console.WriteLine($"END (after {sw.Elapsed.TotalSeconds} seconds)");

void Part1()
{
	var letters = new StringBuilder();
	var grid = ReadInput();
	var dir = Direction.Down;
	var y = 0;
	var x = 0;
	var steps = 0;
	for (x = 0; x < grid.GetLength(1); x++)
	{
		if (grid[0, x] == '|')
		{
			break;
		}
	}
	while (true)
	{
		steps++;
		var c = grid[y, x];
		if (c >= 'A' && c <= 'Z')
		{
			letters.Append(c);
		}

		if (dir == Direction.Down)
		{
			if (!GoDown(grid, x, ref y, ref dir))
			{
				if (!GoRight(grid, ref x, y, ref dir))
				{
					if (!GoLeft(grid, ref x, y, ref dir))
					{
						break;
					}
				}
			}
		}
		else if (dir == Direction.Right)
		{
			if (!GoRight(grid, ref x, y, ref dir))
			{
				if (!GoUp(grid, x, ref y, ref dir))
				{
					if (!GoDown(grid, x, ref y, ref dir))
					{
						break;
					}
				}
			}
		}
		else if (dir == Direction.Left)
		{
			if (!GoLeft(grid, ref x, y, ref dir))
			{
				if (!GoUp(grid, x, ref y, ref dir))
				{
					if (!GoDown(grid, x, ref y, ref dir))
					{
						break;
					}
				}
			}
		}
		else if (dir == Direction.Up)
		{
			if (!GoUp(grid, x, ref y, ref dir))
			{
				if (!GoLeft(grid, ref x, y, ref dir))
				{
					if (!GoRight(grid, ref x, y, ref dir))
					{
						break;
					}
				}
			}
		}
	}

	System.Console.WriteLine(letters);
	System.Console.WriteLine($"{steps} steps");
}

bool Go(char[,] grid, ref int x, ref int y, int incX, int incY, ref Direction dir, Direction newDir)
{
	var h = grid.GetLength(0);
	var w = grid.GetLength(1);
	if (x + incX >= 0 && y + incY >= 0 && x + incX < w && y + incY < h && grid[y + incY, x +incX] != ' ')
	{
		x += incX;
		y += incY;
		dir = newDir;
		return true;
	}
	return false;
}

bool GoLeft(char[,] grid, ref int x, int y, ref Direction dir)
{
	return Go(grid, ref x, ref y, -1, 0, ref dir, Direction.Left);
}

bool GoRight(char[,] grid, ref int x, int y, ref Direction dir)
{
	return Go(grid, ref x, ref y, 1, 0, ref dir, Direction.Right);
}

bool GoUp(char[,] grid, int x, ref int y, ref Direction dir)
{
	return Go(grid, ref x, ref y, 0, -1, ref dir, Direction.Up);
}

bool GoDown(char[,] grid, int x, ref int y, ref Direction dir)
{
	return Go(grid, ref x, ref y, 0, 1, ref dir, Direction.Down);
}

char[,] ReadInput()
{
	var lines = File.ReadAllLines("input.txt");
	var grid = new char[lines.Length, lines.First().Length];
	for (var y = 0; y < grid.GetLength(0); y++)
	{
		for (var x = 0; x < grid.GetLength(1); x++)
		{
			grid[y, x] = lines[y][x];
		}
	}
	return grid;
}

enum Direction
{
	Up,
	Down,
	Left,
	Right
}
