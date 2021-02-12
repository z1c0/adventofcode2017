using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

// Day 21

Console.WriteLine("START");
var sw = Stopwatch.StartNew();
Part1(5);
Part1(18);
Console.WriteLine($"END (after {sw.Elapsed.TotalSeconds} seconds)");

void Part1(int iterations)
{
	var square = new char[,] {
		{ '.', '#', '.' },
		{ '.', '.', '#' },
		{ '#', '#', '#' },
	};

	var rules = ReadInput().ToList();

	for (var i = 0; i < iterations; i++)
	{
		var (result, newSquare) = Explode(2, square, rules);
		if (result)
		{
			square = newSquare;
			continue;
		}
		(result, newSquare) = Explode(3, square, rules);
		if (result)
		{
			square = newSquare;
			continue;
		}
		throw new InvalidOperationException();
	}
	Print(square);
}

(bool, char[,]) Explode(int by, char[,] square, List<(char[,] from, char[,] to)> rules)
{
	var len = square.GetLength(0);
	if (len % by == 0)
	{
		var squares = len / by;
		var newLen = squares * (by + 1);
		var newSquare = new char[newLen, newLen];
		for (var y = 0; y < squares; y++)
		{
			for (var x = 0; x < squares; x++)
			{
				var rule = rules.First(r => AreEqual(square, by, x * by, y * by, r.from));
				CopyTo(rule.to, newSquare, x * (by + 1), y * (by + 1));
			}
		}

		//Print(newSquare);

		return (true, newSquare);
	}
	return (false, square);
}

void CopyTo(char[,] from, char[,] square, int offsetX, int offSetY)
{
	var len = from.GetLength(0);
	for (var y = 0; y < len; y++)
	{
		for (var x = 0; x < len; x++)
		{
			square[offSetY + y, offsetX + x] = from[y, x];
		}
	}
}

bool AreEqual(char[,] square1, int len, int offsetX, int offsetY, char[,] square2)
{
	if (len != square2.GetLength(0))
	{
		return false;
	}
	for (var y = 0; y < len; y++)
	{
		for (var x = 0; x < len; x++)
		{
			if (square1[offsetY + y, offsetX + x] != square2[y, x])
			{
				return false;
			}
		}
	}
	return true;
}

void Print(char[,] square)
{
	var pixels = 0;
	var len = square.GetLength(0);
	for (var y = 0; y < len; y++)
	{
		for (var x = 0; x < len; x++)
		{
			//System.Console.Write(square[y, x]);
			if (square[y, x] == '#')
			{
				pixels++;
			}
		}
		//System.Console.WriteLine();
	}
	System.Console.WriteLine($"{pixels} pixels are ON.");
	System.Console.WriteLine();
}

IEnumerable<(char[,] from, char[,] to)> ReadInput()
{
	char[,] ToSquare(IEnumerable<string> lines)
	{
		var square = new char[lines.Count(), lines.First().Length];
		for (var y = 0; y < lines.Count(); y++)
		{
			var line = lines.ElementAt(y);
			for (var x = 0; x < line.Length; x++)
			{
				square[y, x] = line[x];
			}
		}
		return square;
	}
	foreach (var line in File.ReadAllLines("input.txt"))
	{
		var tokens = line.Split(" => ");
		var from1 = ToSquare(tokens[0].Split('/'));
		var from2 = Rotate(from1);
		var from3 = Rotate(from2);
		var from4 = Rotate(from3);
		var to = ToSquare(tokens[1].Split('/'));
		yield return (from1, to);
		yield return (FlipH(from1), to);
		yield return (FlipV(from1), to);
		yield return (from2, to);
		yield return (FlipH(from2), to);
		yield return (FlipV(from2), to);
		yield return (from3, to);
		yield return (FlipH(from3), to);
		yield return (FlipV(from3), to);
		yield return (from4, to);
		yield return (FlipH(from4), to);
		yield return (FlipV(from4), to);
	}
}

char[,] FlipV(char[,] square)
{
	var flipped = (char[,])square.Clone();
	var len = square.GetLength(0);
	for (var i = 0; i < len; i++)
	{
		flipped[i, 0] = square[i, len - 1];
		flipped[i, len - 1] = square[i, 0];
	}
	return flipped;
}

char[,] FlipH(char[,] square)
{
	var flipped = (char[,])square.Clone();
	var len = square.GetLength(0);
	for (var i = 0; i < len; i++)
	{
		flipped[0, i] = square[len - 1, i];
		flipped[len - 1, i] = square[0, i];
	}
	return flipped;
}

char[,] Rotate(char[,] square)
{
	var rotated = (char[,])square.Clone();
	var len = square.GetLength(0);
	for (var i = 0; i < len; i++)
	{
		// up -> right
		rotated[0, i] = square[i, len - 1];
		// right -> down
		rotated[i, len - 1] = square[len - 1, len - 1 - i];
		// down -> left
		rotated[len - 1, len - 1 - i] = square[len - 1 - i, 0];
		// left -> up
		rotated[len - 1 - i, 0] = square[0, i];
	}
	return rotated;
}