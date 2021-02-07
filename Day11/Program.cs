using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

// Day 11

Console.WriteLine("START");
var sw = Stopwatch.StartNew();
Part1();
Console.WriteLine($"END (after {sw.Elapsed.TotalSeconds} seconds)");

void Part1()
{
	var lastDistance = 0;
	var maxDistance = 0;
	(int x, int y) pos = (0, 0);
	foreach (var d in ReadInput())
	{
		switch (d)
		{
			case "n":
				pos.y -= 10;
				break;
			case "ne":
				pos.x += 10;
				pos.y -= 5;
				break;
			case "se":
				pos.x += 10;
				pos.y += 5;
				break;
			case "s":
				pos.y += 10;
				break;
			case "sw":
				pos.x -= 10;
				pos.y += 5;
				break;
			case "nw":
				pos.x -= 10;
				pos.y -= 5;
				break;
			default:
				throw new InvalidOperationException();
		}
		var dx = Math.Abs(pos.x) / 10.0;
		var dy = Math.Abs(pos.y) / 10.0;
		var dxy = dx * 0.5; // every x move, moves as half an y as well
		lastDistance = (int)(dx + (dy - dxy));
		maxDistance = Math.Max(maxDistance, lastDistance);
	}
	System.Console.WriteLine($"position: {pos}; distance : {lastDistance}; max distance: {maxDistance}");
}

IEnumerable<string> ReadInput()
{
	return File.ReadAllText("input.txt").Split(',');
}