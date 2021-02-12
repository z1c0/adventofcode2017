using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

// Day 25

Console.WriteLine("START");
var sw = Stopwatch.StartNew();
Part1();
Console.WriteLine($"END (after {sw.Elapsed.TotalSeconds} seconds)");

void Part1()
{
	const int steps = 12399302;
	var state = 'A';
	var tape = new Dictionary<int, bool>();
	var pos = 0;
	for (var i = 0; i < steps; i++)
	{
		tape.TryAdd(pos, false);
		
		switch (state)
		{
			case 'A':
				if (!tape[pos])
				{
					tape[pos] = true;
					pos++;
					state = 'B';
				}
				else
				{
					tape[pos] = false;
					pos++;
					state = 'C';
				}
				break;

			case 'B':
				if (!tape[pos])
				{
					tape[pos] = false;
					pos--;
					state = 'A';
				}
				else
				{
					tape[pos] = false;
					pos++;
					state = 'D';
				}
				break;

			case 'C':
				if (!tape[pos])
				{
					tape[pos] = true;
					pos++;
					state = 'D';
				}
				else
				{
					tape[pos] = true;
					pos++;
					state = 'A';
				}
				break;

			case 'D':
				if (!tape[pos])
				{
					tape[pos] = true;
					pos--;
					state = 'E';
				}
				else
				{
					tape[pos] = false;
					pos--;
					state = 'D';
				}
				break;

			case 'E':
				if (!tape[pos])
				{
					tape[pos] = true;
					pos++;
					state = 'F';
				}
				else
				{
					tape[pos] = true;
					pos--;
					state = 'B';
				}
				break;

			case 'F':
				if (!tape[pos])
				{
					tape[pos] = true;
					pos++;
					state = 'A';
				}
				else
				{
					tape[pos] = true;
					pos++;
					state = 'E';
				}
				break;

			default:
				throw new InvalidOperationException();
		}
	}
	System.Console.WriteLine($"The checksum is: {tape.Count(c => c.Value)}");
}

