using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

// Day 17

Console.WriteLine("START");
var sw = Stopwatch.StartNew();
Part1(2017, 2017);
Part1(50000000, 0);
Console.WriteLine($"END (after {sw.Elapsed.TotalSeconds} seconds)");

void Part1(int spins, int after)
{
	var steps = ReadInput();
	var buffer = new LinkedList<int>();
	var node = buffer.AddFirst(0);

	for (var i = 1; i <= spins; i++)
	{
		for (var j = 0; j < steps; j++)
		{
			node = node.Next;
			if (node == null)
			{
				node = buffer.First;
			}
		}

		node = buffer.AddAfter(node, i);
	}

	node = buffer.First;
	while (true)
	{
		if (node.Value == after)
		{
			System.Console.WriteLine($"After {after}: {node.Next.Value}");
			break;
		}
		node = node.Next;
	}
}

int ReadInput()
{
	return int.Parse(File.ReadAllText("input.txt"));
}