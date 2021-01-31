using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;


// Day 3

Console.WriteLine("START");
var sw = Stopwatch.StartNew();
Part1();
Part2();

void Part2()
{
	var n = ReadInput();
	var grid = new Dictionary<(int x, int y), int>();
	var left = 1;
	var up = 1;
	var right = 2;
	var down = 2;
	var x = 0;
	var y = 0;
	var v = 0;
	grid[(x, y)] = 1;
	while (true)
	{
		// move left
		for (var j = 0; j < left; j++)
		{
			v = GetAdjacentSum(grid, ++x, y);
			if (v > n)
			{
				System.Console.WriteLine(v);
				return;
			}
			grid[(x, y)] = v;
		}
		// move up
		for (var j = 0; j < up; j++)
		{
			v = GetAdjacentSum(grid, x, ++y);
			if (v > n)
			{
				System.Console.WriteLine(v);
				return;
			}
			grid[(x, y)] = v;
		}
		// move right
		for (var j = 0; j < right; j++)
		{
			v = GetAdjacentSum(grid, --x, y);
			if (v > n)
			{
				System.Console.WriteLine(v);
				return;
			}
			grid[(x, y)] = v;
		}
		// move down
		for (var j = 0; j < down; j++)
		{
			v = GetAdjacentSum(grid, x, --y);
			if (v > n)
			{
				System.Console.WriteLine(v);
				return;
			}
			grid[(x, y)] = v;
		}

		left += 2;
		up += 2;
		right += 2;
		down += 2;
	}
}

int GetAdjacentSum(Dictionary<(int x, int y), int> grid, int x, int y)
{
	var sum = 0;
	var i = 0;
	if (grid.TryGetValue((x + 1, y), out i))
	{
		sum += i;
	}
	if (grid.TryGetValue((x + 1, y + 1), out i))
	{
		sum += i;
	}
	if (grid.TryGetValue((x, y + 1), out i))
	{
		sum += i;
	}
	if (grid.TryGetValue((x - 1, y + 1), out i))
	{
		sum += i;
	}
	if (grid.TryGetValue((x - 1, y), out i))
	{
		sum += i;
	}
	if (grid.TryGetValue((x - 1, y - 1), out i))
	{
		sum += i;
	}
	if (grid.TryGetValue((x, y - 1), out i))
	{
		sum += i;
	}
	if (grid.TryGetValue((x + 1, y - 1), out i))
	{
		sum += i;
	}
	return sum;
}

Console.WriteLine($"END (after {sw.Elapsed.TotalSeconds} seconds)");

void Part1()
{
	var n = ReadInput();
	var grid = new Dictionary<int, (int x, int y)>();
	var left = 1;
	var up = 1;
	var right = 2;
	var down = 2;
	var i = 1;
	var x = 0;
	var y = 0;
	grid[i++] = (x, y);
	while (i < n)
	{
		// move left
		for (var j = 0; j < left; j++)
		{
			grid[i++] = (++x, y);
		}
		// move up
		for (var j = 0; j < up; j++)
		{
			grid[i++] = (x, ++y);
		}
		// move right
		for (var j = 0; j < right; j++)
		{
			grid[i++] = (--x, y);
		}
		// move down
		for (var j = 0; j < down; j++)
		{
			grid[i++] = (x, --y);
		}

		left += 2;
		up += 2;
		right += 2;
		down += 2;
	}

	var dest = grid[n];
	System.Console.WriteLine($"Distance to destination {n}: {Math.Abs(dest.x) + Math.Abs(dest.y)}");
}

int ReadInput()
{
	return Int32.Parse(File.ReadAllText("input.txt"));
}
