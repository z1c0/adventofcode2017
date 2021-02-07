using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

// Day 13

Console.WriteLine("START");
var sw = Stopwatch.StartNew();
Part1();
Part2();
Console.WriteLine($"END (after {sw.Elapsed.TotalSeconds} seconds)");

void Part2()
{
	var layerState = InitLayers(ReadInput());
	var copyLayerState = new (int range, int pos, bool down)[layerState.Length];
	var delay = 0;
	while (true)
	{
		for (var i = 0; i < layerState.Length; i++)
		{
			copyLayerState[i] = layerState[i];
		}

		if (!Trip(copyLayerState).caught)
		{
			break;
		}

		delay++;
		MoveScanners(layerState);
	}
	Console.WriteLine($"The minimum delay required is: {delay}");
}

void Part1()
{
	var (_, severity) = Trip(InitLayers(ReadInput()));
	System.Console.WriteLine($"The severity of the trip was: {severity}");
}

(int range, int pos, bool down)[] InitLayers(Dictionary<int, int> layerConfig)
{
	var layerState = new (int range, int pos, bool down)[layerConfig.Keys.Max() + 1];
	foreach (var l in layerConfig)
	{
		layerState[l.Key] = (l.Value, 0, true);
	}
	return layerState;
}

(bool caught, int severity) Trip((int range, int pos, bool down)[] layerState)
{
	var caught = false;
	var severity = 0;
	var pos = 0;
	while (pos < layerState.Length)
	{
		if (layerState[pos].range > 0 && layerState[pos].pos == 0)
		{
			severity += (pos * layerState[pos].range);
			caught = true;
		}
		
		MoveScanners(layerState);

		// move
		pos++;
	}
	return (caught, severity);
}

void MoveScanners((int range, int pos, bool down)[] layerState)
{
	for (var i = 0; i < layerState.Length; i++)
	{
		if (layerState[i].range > 0)
		{
			var down = layerState[i].down;
			var v = layerState[i].pos;
			if (down)
			{
				if (v < layerState[i].range - 1)
				{
					v++;
				}
				else
				{
					down = false;
					v--;
				}
			}
			else
			{
				if (v > 0)
				{
					v--;
				}
				else
				{
					down = true;
					v++;
				}
			}
			layerState[i].pos = v;
			layerState[i].down = down;
		}
	}
}

Dictionary<int, int> ReadInput()
{
	var dict = new Dictionary<int, int>();
	foreach (var line in File.ReadAllLines("input.txt"))
	{
		var tokens = line.Split(": ");
		var depth = Int32.Parse(tokens[0]);
		var range = Int32.Parse(tokens[1]);
		dict[depth] = range;
	}
	return dict;
}