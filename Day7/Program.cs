using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

// Day 7

Console.WriteLine("START");
var sw = Stopwatch.StartNew();
Part1();


void CheckWeights(Program program, List<Program> programs, int targetWeight)
{
	System.Console.WriteLine($"{program.Name} ({program.Weight}):");
	var weights = new List<(int Weight, string Name)>();
	foreach (var subName in program.SubPrograms)
	{
		var sub = programs.Find(p => p.Name == subName);
		var weight = GetWeight(sub, programs);
		weights.Add((weight, sub.Name));
		System.Console.WriteLine($"{sub.Name} ({sub.Weight}) -> {weight}");
	}
	System.Console.WriteLine();
	var groups = weights.GroupBy(w => w.Weight).ToList();
	if (groups.Count > 1)
	{
		var g1 = groups.First(g => g.Count() == 1);
		var g2 = groups.First(g => g.Count() > 1);
		CheckWeights(programs.Find(p => p.Name == g1.First().Name), programs, g2.First().Weight);
	}
	else
	{
		var diff = targetWeight - program.Weight - groups.First().First().Weight * groups.First().Count();
		System.Console.WriteLine($"Program {program.Name} should have a weight of {program.Weight + diff}");
	}
}

int GetWeight(Program program, List<Program> programs)
{
	int weight = program.Weight;
	program.SubPrograms.ForEach(s => weight += GetWeight(programs.Find(p => p.Name == s), programs));
	return weight;
}

Console.WriteLine($"END (after {sw.Elapsed.TotalSeconds} seconds)");

void Part1()
{
	var programs = ReadInput().ToList();
	foreach (var prog in programs)
	{
		foreach (var s in prog.SubPrograms)
		{
			programs.Find(p => p.Name == s).Referenced = true;
		}
	}

	var root = programs.Single(p => !p.Referenced);
	System.Console.WriteLine($"The bottom program is '{root.Name}'");
	
	// Part 2
	CheckWeights(root, programs, 0);
}

IEnumerable<Program> ReadInput()
{
	foreach (var line in File.ReadAllLines("input.txt"))
	{
		var tokens = line.Split(" -> ");
		var tokens1 = tokens[0].Split(' ');
		var name = tokens1[0];
		var weight = Int32.Parse(tokens1[1][1..^1].ToString());
		var subPrograms = new List<string>();
		if (tokens.Length > 1)
		{
			subPrograms.AddRange(tokens[1].Split(", "));

		}
		yield return new Program(name, weight, subPrograms);
	}
}

internal record Program(string Name, int Weight, List<string> SubPrograms)
{
	internal bool Referenced { get; set; }
}