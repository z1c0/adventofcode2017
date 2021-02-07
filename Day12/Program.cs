using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

// Day 12

Console.WriteLine("START");
var sw = Stopwatch.StartNew();
Part1();
Part2();
Console.WriteLine($"END (after {sw.Elapsed.TotalSeconds} seconds)");

void Part1()
{
	var dict = ReadInput();
	var zeroProgs = new SortedSet<int>();

	Visit(0, dict, zeroProgs);

	System.Console.WriteLine($"{zeroProgs.Count} programs contain the program 0.");
}

void Part2()
{
	var groupCount = 0;
	var dict = ReadInput();
	while (dict.Any())
	{
		var zeroProgs = new SortedSet<int>();
		Visit(dict.First().Key, dict, zeroProgs);
		foreach (var p in zeroProgs)
		{
			dict.Remove(p);
		}
		groupCount++;
	}
	System.Console.WriteLine($"{groupCount} programs were found.");
}

void Visit(int prog, Dictionary<int, List<int>> dict, SortedSet<int> zeroProgs)
{
	if (!zeroProgs.Contains(prog))
	{
		zeroProgs.Add(prog);
		foreach (var p in dict[prog])
		{
			Visit(p, dict, zeroProgs);
		}
	}
}

Dictionary<int, List<int>> ReadInput()
{
	var dict = new Dictionary<int, List<int>>();
	foreach (var line in File.ReadAllLines("input.txt"))
	{
		var tokens = line.Split(" <-> ");
		var p1 = Int32.Parse(tokens[0]);
		tokens = tokens[1].Split(", ");
		foreach (var t in tokens)
		{
			var p2 = Int32.Parse(t);
			if (!dict.ContainsKey(p1))
			{
				dict[p1] = new List<int>();
			}
			if (!dict[p1].Contains(p2))
			{
				dict[p1].Add(p2);
			}
		}
	}
	return dict;
}