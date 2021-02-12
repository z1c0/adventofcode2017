using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

// Day 24

Console.WriteLine("START");
var sw = Stopwatch.StartNew();
Part1();
Console.WriteLine($"END (after {sw.Elapsed.TotalSeconds} seconds)");

void Part1()
{
	var maxStrength = 0;
	(int length, int strength) maxLength = (0, 0);
	FindNext(ReadInput().ToList(), new List<(int, int)>(), 0, ref maxStrength, ref maxLength);
	System.Console.WriteLine($"Strongest bridge: {maxStrength}");
	System.Console.WriteLine($"Strength of longest {maxLength.length} bridge: {maxLength.strength}");
}

void FindNext(List<(int port1, int port2)> portsAvailable, List<(int port1, int port2)> portsUsed, int need, ref int maxStrength, ref (int length, int strength) maxLength)
{
	foreach (var p in portsAvailable)
	{
		if (p.port1 == need || p.port2 == need)
		{
			var other = (p.port1 == need) ? p.port2 : p.port1;
			var copyAvailable = portsAvailable.ToList();
			copyAvailable.Remove(p);
			var copyUsed = portsUsed.ToList();
			copyUsed.Add((need * -1, other)); // * -1 => mark port as used
			FindNext(copyAvailable, copyUsed, other, ref maxStrength, ref maxLength);
		}
	}
	
	// score
	var strength = 0;
	foreach (var p in portsUsed)
	{
		strength += Math.Abs(p.port1);
		strength += Math.Abs(p.port2);
	}
	var length = portsUsed.Count;
	if (length > maxLength.length || (length == maxLength.length && strength > maxLength.strength))
	{
		maxLength.length = length;
		maxLength.strength = strength;
	}

	maxStrength = Math.Max(maxStrength, strength);
}

IEnumerable<(int port1, int port2)> ReadInput()
{
	foreach (var line in File.ReadAllLines("input.txt"))
	{
		var tokens = line.Split('/');
		yield return (int.Parse(tokens[0]), int.Parse(tokens[1]));
	}
}

