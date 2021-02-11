using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

// Day 20

Console.WriteLine("START");
var sw = Stopwatch.StartNew();
Part1();
Part2();
Console.WriteLine($"END (after {sw.Elapsed.TotalSeconds} seconds)");

void Part2()
{
	var particles = ReadInput().ToList();
	const int iterations = 100000;
	for (var i = 0; i < iterations; i++)
	{
		var duplicates = particles
			.GroupBy(p => p.Position)
			.Where(group => group.Count() > 1)
			.Select(group => group.Key);
		if (duplicates.Any())
		{
			foreach (var d in duplicates)
			{
				particles.RemoveAll(p => p.Position.Equals(d));
			}
			System.Console.WriteLine($"Number of particles left: {particles.Count}");
		}

		particles.ForEach(p => p.Update());
	}
}
void Part1()
{
	var particles = ReadInput().ToList();
	const int iterations = 10000;
	for (var i = 0; i < iterations; i++)
	{
		particles.ForEach(p => p.Update());
	}

	var minDistance = particles.Min(p => p.Distance);
	var p = particles.First(p => p.Distance == minDistance);
	System.Console.WriteLine($"Particle {p} hast the smallest distance of {p.Distance}.");
}

IEnumerable<Particle> ReadInput()
{
	Vector ParseVector(string s)
	{
		var tokens = s[3..^1].Split(',');
		return new Vector(
			long.Parse(tokens[0]),
			long.Parse(tokens[1]),
			long.Parse(tokens[2])
		);
	}
	var index = 0;
	foreach (var line in File.ReadAllLines("input.txt"))
	{
		var tokens = line.Split(", ");
		yield return new Particle(index++)
		{
			Position = ParseVector(tokens[0]),
			Velocity = ParseVector(tokens[1]),
			Acceleration = ParseVector(tokens[2])
		};
	}
}

struct Vector
{
	long X;
	long Y;
	long Z;

	public Vector(long x, long y, long z) : this()
	{
		X = x;
		Y = y;
		Z = z;
	}

	public long Length { get => Math.Abs(X) + Math.Abs(Y) + Math.Abs(Z); }

	public override string ToString()
	{
		return $"<{X}, {Y}, {Z}>";
	}

	public static Vector operator +(Vector v1, Vector v2)
	{
		return new Vector(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
	}
}

record Particle(int Index)
{
	internal Vector Position { get; set; }
	internal Vector Velocity { get; set; }
	internal Vector Acceleration { get; set; }
	internal void Update()
	{
		Velocity = Velocity + Acceleration;
		Position = Position + Velocity;
	}

	public long Distance { get => Position.Length; }

	public override string ToString()
	{
		return $"#{Index} {Position}";
	}
}
