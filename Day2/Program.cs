using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;


// Day 2

Console.WriteLine("START");
var sw = Stopwatch.StartNew();
Part1();
Part2();
Console.WriteLine($"END (after {sw.Elapsed.TotalSeconds} seconds)");

void Part1()
{
	var checksum = 0;
	foreach (var row in ReadInput())
	{
		checksum += row.Max() - row.Min();
	}
	System.Console.WriteLine($"Checksum: {checksum}");
}

void Part2()
{
	var checksum = 0;
	foreach (var row in ReadInput())
	{
		var sorted = row.OrderByDescending(i => i).ToList();
		for (var i = 0; i < sorted.Count; i++)
		{
			for (var j = i + 1; j < sorted.Count; j++)
			{
				if (sorted[i] % sorted[j] == 0)
				{
					checksum += sorted[i] / sorted[j];
				}
			}
		}
	}
	System.Console.WriteLine($"Checksum: {checksum}");
}

int[][] ReadInput()
{
	var lines = File.ReadAllLines("input.txt");
	var sheet = new int[lines.Length][];
	for (var y = 0; y < lines.Length; y++)
	{
		var line = lines[y];
		var tokens = line.Split("\t");
		sheet[y] = new int[tokens.Count()];
		for (var x = 0; x < tokens.Count(); x++)
		{
			sheet[y][x] = Int32.Parse(tokens[x]);
		}
	}
	return sheet;
}
