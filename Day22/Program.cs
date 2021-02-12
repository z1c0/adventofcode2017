using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

// Day 22

Console.WriteLine("START");
var sw = Stopwatch.StartNew();
Part1();
Part2();
Console.WriteLine($"END (after {sw.Elapsed.TotalSeconds} seconds)");

void Part2()
{
	var map = ReadInput();
	const int bursts = 10000000;
	var infections = 0;
	var dir = Direction.Up;
	(int x, int y) pos = (0, 0);
	for (var i = 0; i < bursts; i++)
	{
		if (map[pos] == State.Clean)
		{
			dir = TurnLeft(dir);
			map[pos] = State.Weakened;
		}
		else if (map[pos] == State.Weakened)
		{
			map[pos] = State.Infected;
			infections++;
		}
		else if (map[pos] == State.Infected)
		{
			dir = TurnRight(dir);
			map[pos] = State.Flagged;
		}
		else if (map[pos] == State.Flagged)
		{
			dir = Reverse(dir);
			map[pos] = State.Clean;
		}
		// Move
		pos = Move(pos, dir, map);
	}

	System.Console.WriteLine($"After {bursts} bursts, {infections} infections happened.");
}

Direction Reverse(Direction dir)
{
	return dir switch {
		Direction.Down => Direction.Up,
		Direction.Right => Direction.Left,
		Direction.Up => Direction.Down,
		Direction.Left => Direction.Right,
		_ => throw new InvalidOperationException(),
	};
}

void Part1()
{
	var map = ReadInput();
	const int bursts = 10000;
	var infections = 0;
	var dir = Direction.Up;
	(int x, int y) pos = (0, 0);
	for (var i = 0; i < bursts; i++)
	{
		// Turn
		if (map[pos] == State.Infected)
		{
			dir = TurnRight(dir);
			// clean
			map[pos] = State.Clean;
		}
		else
		{
			dir = TurnLeft(dir);
			// Infect
			map[pos] = State.Infected;
			infections++;
		}
		// Move
		pos = Move(pos, dir, map);
	}

	System.Console.WriteLine($"After {bursts} bursts, {infections} infections happened.");
}

(int x, int y) Move((int x, int y) pos, Direction dir, Dictionary<(int x, int y), State> map)
{
	switch (dir)
	{
		case Direction.Up:
			pos.y--;
			break;
		case Direction.Right:
			pos.x++;
			break;
		case Direction.Down:
			pos.y++;
			break;
		case Direction.Left:
			pos.x--;
			break;
		default:
			throw new InvalidOperationException();
	}
	map.TryAdd(pos, State.Clean);
	return pos;
}

Direction TurnLeft(Direction dir)
{
	return (Direction)((int)(dir - 1 + 4) % 4);
}

Direction TurnRight(Direction dir)
{
	return (Direction)((int)(dir + 1) % 4);
}

Dictionary<(int x, int y), State> ReadInput()
{
	var map = new Dictionary<(int x, int y), State>();
	var lines = File.ReadAllLines("input.txt");
	var h = lines.Length; 
	for (var y = 0; y < h; y++)
	{
		var line = lines[y];
		var w = line.Length;
		for (var x = 0; x < w; x++)
		{
			map[(x - w / 2, y - h / 2)] = (line[x] == '#') ? State.Infected : State.Clean;
		}
	}
	return map;
}

enum State
{
	Infected,
	Clean,
	Weakened,
	Flagged,
}

enum Direction
{
	Up,
	Right,
	Down,
	Left,
}
