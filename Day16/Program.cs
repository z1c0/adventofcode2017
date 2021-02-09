using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

// Day 16

Console.WriteLine("START");
var sw = Stopwatch.StartNew();
Part1(1);
Part1(1000000000);
Console.WriteLine($"END (after {sw.Elapsed.TotalSeconds} seconds)");

void Part1(int iterations)
{
	var programs = new List<char>();
	for (var i = 0; i < 16; i++)
	{
		programs.Add((char)('a' + i));
	}
	var instructions = ReadInput().ToList();

	var cache = new Dictionary<string, string>();
	for (var i = 0; i < iterations; i++)
	{
		var before = new string(programs.ToArray());
		if (cache.ContainsKey(before))
		{
			programs = cache[before].ToList();
		}
		else
		{
			foreach (var ins in instructions)
			{
				switch (ins[0])
				{
					case 's': {
						var pos = programs.Count - int.Parse(ins.Substring(1));
						var removed = programs.GetRange(pos, programs.Count - pos);
						programs.RemoveRange(pos, programs.Count - pos);
						programs.InsertRange(0, removed);
					}
					break;

					case 'x': {
						var tokens = ins.Substring(1).Split('/');
						var pos1 = int.Parse(tokens[0]);
						var pos2 = int.Parse(tokens[1]);
						var tmp = programs[pos1];
						programs[pos1] = programs[pos2];
						programs[pos2] = tmp;
					}
					break;

					case 'p': {
						ins.Substring(1).Split('/');
						var tokens = ins.Substring(1).Split('/');
						var pos1 = programs.IndexOf(tokens[0].Single());
						var pos2 = programs.IndexOf(tokens[1].Single());
						var tmp = programs[pos1];
						programs[pos1] = programs[pos2];
						programs[pos2] = tmp;
					}
					break;

					default:
						throw new InvalidProgramException();
				}
			}

			cache.Add(before, new string(programs.ToArray()));
		}
	}

	System.Console.WriteLine(new string(programs.ToArray()));
}

IEnumerable<string> ReadInput()
{
	return File.ReadAllText("input.txt").Split(',');
}