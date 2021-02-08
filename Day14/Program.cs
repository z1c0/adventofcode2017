using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

// Day 14

Console.WriteLine("START");
var sw = Stopwatch.StartNew();
Part1();
Part2();
Console.WriteLine($"END (after {sw.Elapsed.TotalSeconds} seconds)");


void Part1()
{
	var count = 0;
	var grid = CalculateGrid();
	for (var y = 0; y < grid.GetLength(0); y++)
	{
		for (var x = 0; x < grid.GetLength(1); x++)
		{
			if (grid[y, x] != 0)
			{
				count++;
			}
		}
	}
	System.Console.WriteLine($"{count} squares are used.");
}

void Part2()
{
	var grid = CalculateGrid();
	var region = 1;
	for (var y = 0; y < grid.GetLength(0); y++)
	{
		for (var x = 0; x < grid.GetLength(1); x++)
		{
			if (Mark(grid, x, y, region))
			{
				region++;
			}
		}
	}
	System.Console.WriteLine($"Number of regions: {region - 1}");
	//Print(grid, 8, 8);
}

bool Mark(int[,] grid, int x, int y, int region)
{
	var h = grid.GetLength(0);
	var w = grid.GetLength(1);
	if (y >= 0 && y < h && x >= 0 && x < w && grid[y, x] == -1)
	{
		grid[y, x] = region;
		Mark(grid, x - 1, y    , region);
		Mark(grid, x    , y - 1, region);
		Mark(grid, x + 1, y    , region);
		Mark(grid, x    , y + 1, region);
		return true;
	}
	return false;
}

int[,] CalculateGrid()
{
	var grid = new int[128, 128];
	var input = ReadInput();
	for (var y = 0; y < 128; y++)
	{
		var hash = KnotHash.Calculate($"{input}-{y}");
		var x = 0;
		for (var j = 0; j < 16; j++)
		{
			var b = hash[j];
			for (var k = 0; k < 8; k++)
			{
				grid[y, x++] = ((b & 0b_1000_0000) == 0b_1000_0000) ? -1 : 0;
				b <<= 1;
			}
		}
	}
	return grid;
}

void Print(int[,] grid, int rows, int cols)
{
	for (var y = 0; y < rows; y++)
	{
		for (var x = 0; x < cols; x++)
		{
			var i = grid[y, x];
			if (i == 0)
			{
				System.Console.Write('.');
			}
			else if (i == -1)
			{
				System.Console.Write('#');
			}
			else
			{
				System.Console.Write(i);
			}
		}
		System.Console.WriteLine();
	}
}

string ReadInput()
{
	return File.ReadAllText("input.txt");
}