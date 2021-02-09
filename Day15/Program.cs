using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

// Day 15

Console.WriteLine("START");
var sw = Stopwatch.StartNew();
Part1();
Part2();

void Part2()
{
	var (a, b) = ReadInput();
	var count = 0;
	for (var i = 0; i < 5_000_000; i++)
	{
		a = GeneratePicky(a, 16807, 4);
		b = GeneratePicky(b, 48271, 8);
		if ((a & 0xFFFF) == (b &0xFFFF))
		{
			count++;
		}
		//System.Console.WriteLine($"{a} - {b}");
	}
	System.Console.WriteLine($"{count} (picky) matching pairs were found.");
}

Console.WriteLine($"END (after {sw.Elapsed.TotalSeconds} seconds)");

void Part1()
{
	var (a, b) = ReadInput();
	var count = 0;
	for (var i = 0; i < 40_000_000; i++)
	{
		a = Generate(a, 16807);
		b = Generate(b, 48271);
		if ((a & 0xFFFF) == (b &0xFFFF))
		{
			count++;
		}
		//System.Console.WriteLine($"{a} - {b}");
	}
	System.Console.WriteLine($"{count} matching pairs were found.");
}

long GeneratePicky(long n, int factor, int divisor)
{
	while (true)
	{
		n = Generate(n, factor);
		if (n % divisor == 0)
		{
			return n;
		}
	}
	throw new InvalidOperationException();
}

long Generate(long n, int factor)
{
	n *= factor;
	n %= 2147483647;
	return n;
}

(long, long) ReadInput()
{
	var lines = File.ReadAllLines("input.txt");
	return (
		Int32.Parse(lines[0].Split(' ').Last()),
		Int32.Parse(lines[1].Split(' ').Last())
	);
}