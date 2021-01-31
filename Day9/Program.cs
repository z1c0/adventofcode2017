using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

// Day 9

Console.WriteLine("START");
var sw = Stopwatch.StartNew();
Part1();
Console.WriteLine($"END (after {sw.Elapsed.TotalSeconds} seconds)");

void Part1()
{
	var position = 0;
	var (score, garbage) = ProcessGroup(ReadInput(), ref position, 1);
	System.Console.WriteLine($"Score: {score}, Non-canceled garbage: {garbage}");
}

(int score, int garbage) ProcessGroup(string stream, ref int position, int groupScore)
{
	var totalScore = groupScore;
	var garbage = 0;
	if (stream[position++] != '{')
	{
		throw new InvalidOperationException();
	}
	while (true)
	{
		var c = stream[position];
		if (c == '{')
		{
			var t = ProcessGroup(stream, ref position, groupScore + 1);
			totalScore += t.score;
			garbage += t.garbage;
		}
		else if (c == '}')
		{
			position++;
			return (totalScore, garbage);
		}
		else if (c == '<')
		{
			garbage += ProcessGarbage(stream, ref position);
		}
		else
		{
			position++;
		}
	}
	// should never be reached
	throw new InvalidProgramException();
}

int ProcessGarbage(string stream, ref int position)
{
	var garbage = 0;
	if (stream[position++] != '<')
	{
		throw new InvalidOperationException();
	}
	while (true)
	{
		var c = stream[position];
		if (c == '>')
		{
			position++;
			break;
		}
		else if (c == '!')
		{
			position += 2;
		}
		else
		{
			garbage++;
			position++;
		}
	}
	return garbage;
}

string ReadInput()
{
	return File.ReadAllText("input.txt");
}
