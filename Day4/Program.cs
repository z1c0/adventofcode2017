using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;


// Day 4

Console.WriteLine("START");
var sw = Stopwatch.StartNew();
Part1(false);
Part1(true);
Console.WriteLine($"END (after {sw.Elapsed.TotalSeconds} seconds)");

void Part1(bool sort)
{
	var sum = 0;
	foreach (var line in ReadInput())
	{
		var l = line;
		if (sort)
		{
			var newLine = new List<string>();
			foreach (var w in line)
			{
				newLine.Add(new string(w.OrderBy(c => c).ToArray()));
			}
			l = newLine;
		}
		if (l.Distinct().Count() == l.Count())
		{
			sum++;
		}
	}
	System.Console.WriteLine($"{sum} passphrases are valid");
}

IEnumerable<IEnumerable<string>> ReadInput()
{
	foreach (var line in File.ReadAllLines("input.txt"))
	{
		yield return line.Split(' ');
	}
}
