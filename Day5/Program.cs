using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

// Day 5

Console.WriteLine("START");
var sw = Stopwatch.StartNew();
Part1(true);
Part1(false);
Console.WriteLine($"END (after {sw.Elapsed.TotalSeconds} seconds)");

void Part1(bool simple)
{
	var steps = 0;
	var pc = 0;
	var instructions = ReadInput().ToList();
	while (pc >= 0 && pc < instructions.Count)
	{
		if (simple)
		{
			pc += instructions[pc]++;
		}
		else
		{
			var offs = instructions[pc];
			if (offs >= 3)
			{
				instructions[pc]--;
			}
			else
			{
				instructions[pc]++;
			}
			pc += offs;
		}
		steps++;
	}
	System.Console.WriteLine($"Steps: {steps}");
}

IEnumerable<int> ReadInput()
{
	foreach (var line in File.ReadAllLines("input.txt"))
	{
		yield return Int32.Parse(line);
	}
}
