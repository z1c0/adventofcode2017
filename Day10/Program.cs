using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

// Day 10

Console.WriteLine("START");
var sw = Stopwatch.StartNew();
Part1();
Part2();
Console.WriteLine($"END (after {sw.Elapsed.TotalSeconds} seconds)");

void Part2()
{
	var list = new byte[256];
	for (var i = 0; i < list.Length; i++)
	{
		list[i] = (byte)i;
	}
	var lengths = ReadInputEx();

	var current = 0;
	var skip = 0;
	for (var i = 0; i < 64; i++)
	{
		KnotHash(list, lengths, ref current, ref skip);
	}

	// dense hash
	var dense = new byte[16];
	for (var i = 0; i < 16; i++)
	{
		dense[i] = list.Skip(i * 16).Take(16).Aggregate((b1, b2) => (byte)(b1 ^ b2));
	}
	
	// textify
	var text = BitConverter.ToString(dense).Replace("-", string.Empty).ToLower();

	System.Console.WriteLine($"The full knot hash is '{text}'.");
}

void Part1()
{
	var list = new byte[256];
	for (var i = 0; i < list.Length; i++)
	{
		list[i] = (byte)i;
	}
	var lengths = ReadInput();
	
	var current = 0;
	var skip = 0;
	KnotHash(list, lengths, ref current, ref skip);

	System.Console.WriteLine($"Element 1: {list[0]} * Element 2: {list[1]} -> {list[0] * list[1]}");
}

void KnotHash(byte[] list, IEnumerable<int> lengths, ref int current, ref int skip)
{
	//Print(current, ring);
	foreach (var l in lengths)
	{
		// reverse
		var x = current;
		var y = (current + l - 1) % list.Length;
		for (var i = 0; i < l / 2; i++)
		{
			var tmp = list[x];
			list[x] = list[y];
			list[y] = tmp;
			x = (x + 1) % list.Length;
			y = (list.Length + y - 1) % list.Length;
		}
		// move current
		current = (current + l + skip) % list.Length;
		// increase skip size
		skip++;
		//Print(current, ring);
	}
}

void Print(int current, List<int> ring)
{
	for (var i = 0; i < ring.Count; i++)
	{
		if (i == current)
		{
			System.Console.Write($"[{ring[i]}] ");
		}
		else
		{
			System.Console.Write($"{ring[i]} ");
		}
	}
	System.Console.WriteLine();
}

IEnumerable<int> ReadInput()
{
	return File.ReadAllText("input.txt").Split(',').Select(t => Int32.Parse(t));
}

IEnumerable<int> ReadInputEx()
{
	var lengths = new List<int>();
	foreach (var c in File.ReadAllText("input.txt"))
	{
		lengths.Add((int)c);
	}
	lengths.Add(17);
	lengths.Add(31);
	lengths.Add(73);
	lengths.Add(47);
	lengths.Add(23);
	return lengths;
}
