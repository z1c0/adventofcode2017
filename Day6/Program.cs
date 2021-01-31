using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

// Day 6

Console.WriteLine("START");
var sw = Stopwatch.StartNew();
Part1();
Console.WriteLine($"END (after {sw.Elapsed.TotalSeconds} seconds)");

void Part1()
{
	var iterations = 0;
	var loopSize = 0;
	var banks = ReadInput().ToArray();
	var history = new List<(int[], int)>();
	history.Add((banks.ToArray(), 0));
	while (true)
	{
		iterations++;
		var max = banks.Max();
		var b = Array.IndexOf(banks, max);
		// redistribute
		banks[b] = 0;
		for (var i = 0; i < max; i++)
		{
			b = (b + 1) % banks.Length;
			banks[b]++;
		}
		var prev = history.FirstOrDefault(t => t.Item1.SequenceEqual(banks));
		if (prev.Item1 != null)
		{
			loopSize = iterations - prev.Item2;
			break;
		}
		history.Add((banks.ToArray(), iterations));
	}
	Console.WriteLine($"Iterations: {iterations}, loop size: {loopSize}");
}

IEnumerable<int> ReadInput()
{
	foreach (var t in File.ReadAllText("input.txt").Split('\t'))
	{
		yield return Int32.Parse(t);
	}
}
