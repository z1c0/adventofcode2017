using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

// Day 23

Console.WriteLine("START");
var sw = Stopwatch.StartNew();
Part1();
//
Part2();
// this probably runs "forever".
// line line 3: "jnz a 2" -> line 5 is reached now
//
// line 25: "jnz g -13" always sends us back -> g is "always" != 0
//
// line 30: terminates -> jumps to end

Console.WriteLine($"END (after {sw.Elapsed.TotalSeconds} seconds)");

void Part1()
{
	var cpu = new Cpu(0);
	cpu.Run(ReadInput().ToList());
	System.Console.WriteLine($"Number of multiplications: {cpu.NumberOfMultiplications}");
}

void Part2()
{
	//
	// This is the assembly code translated into C#
  // There are a couple of nested loops and this will "never" terminate
	// The idea is that if there are some values of "d" and "d" for which
	// "d * e == b". In that case, "h" gets incremented.
	// To shortcircuit this we simple check if b is prime number and if not,
	// there always will be such factors and "h" can be incremented.
	//

	var a = 1;
	var b = 0;
	var c = 0;
	var d = 0;
	var e = 0;
	bool f = false;
	var h = 0;
	
	b = c = 84;
	if (a != 0)
	{
		b *= 100;
		b += 100000;
		c = b;
		c += 17000;
	}
	while (true)
	{
		f = true;
		if (!IsPrime(b))
		{
			h++;
		}
		/*
		d = 2;
		do
		{
			e = 2;
			do
			{
				if (d * e == b)
				{
					f = false;
				}
				e++;
			}
			while (e != b);
			d++;
		}
		while (d != b);
		if (!f)
		{
			h++;
		}
		*/
		if (b == c)
		{
			break;
		}
		b += 17;
	}

	System.Console.WriteLine($"register h: {h}");
}

static bool IsPrime(int n)
{
		if (n == 2) {
				return true;
		}
		if (n < 2 || n % 2 == 0) {
				return false;
		}
		for (int i = 3; i <= Math.Sqrt(n); i += 2) {
				if (n % i == 0) {
						return false;
				}
		}
		return true;
}

IEnumerable<(OpCode op, RegOrValue rv1, RegOrValue rv2)> ReadInput()
{
	RegOrValue ParseRegOrValue(string s)
	{
		if (Int32.TryParse(s, out var n)) {
			return new RegOrValue(n);
		}
	return new RegOrValue(s[0]);
	}
	foreach (var line in File.ReadAllLines("input.txt"))
	{
		var tokens = line.Split(' ');
		var opCode = tokens[0] switch
		{
			"set" => OpCode.Set,
			"sub" => OpCode.Sub,
			"mul" => OpCode.Mul,
			"jnz" => OpCode.Jnz,
			_ => throw new InvalidProgramException()
		};
		yield return (
			opCode,
			ParseRegOrValue(tokens[1]),
			ParseRegOrValue(tokens.Last()));
	}
}

class Cpu
{
		public Cpu(int a)
	{
		Pc = 0;
		Registers['a'] = a;
		Registers['b'] = 0;
		Registers['c'] = 0;
		Registers['d'] = 0;
		Registers['e'] = 0;
		Registers['f'] = 0;
		Registers['g'] = 0;
		Registers['h'] = 0;
	}

	public int Pc { get; private set; }
	public int NumberOfMultiplications { get; private set; }
	public Dictionary<char, long> Registers { get; } = new Dictionary<char, long>();

	internal void Run(List<(OpCode op, RegOrValue rv1, RegOrValue rv2)> instructions)
	{
		while (Pc >= 0 && Pc < instructions.Count)
		{
			var i = instructions[Pc];
			//Trace(i, Registers, Pc);
			switch (i.op)
			{
				case OpCode.Set:
					Registers[i.rv1.Register] = GetValue(i.rv2);
					Pc++;
					break;

				case OpCode.Sub:
					Registers[i.rv1.Register] -= GetValue(i.rv2);
					Pc++;
					break;

				case OpCode.Mul:
					Registers[i.rv1.Register] *= GetValue(i.rv2);
					NumberOfMultiplications++;
					Pc++;
					break;

				case OpCode.Jnz:
					if (GetValue(i.rv1) != 0)
					{
						Pc += (int)GetValue(i.rv2);
					}
					else
					{
						Pc++;
					}
					break;

				default:
					throw new InvalidProgramException();
			}
		}
	}

	private static void Trace((OpCode op, RegOrValue rv1, RegOrValue rv2) i, Dictionary<char, long> registers, long pc)
	{
		foreach (var e in registers)
		{
			System.Console.Write($" {e.Key}: {e.Value} |");
		}
		System.Console.WriteLine();
		System.Console.WriteLine($"[{pc + 1}] {i.op} {i.rv1} {i.rv2}");
		System.Console.WriteLine();
	}

	private long GetValue(RegOrValue rv)
	{
		if (rv.IsRegister)
		{
			return Registers.GetValueOrDefault(rv.Register);
		}
		return rv.Value;
	}
}

struct RegOrValue
{
	public RegOrValue(long value) : this()
	{
		Value = value;
		IsRegister = false;
	}
	public RegOrValue(char register) : this()
	{
		Register = register;
		IsRegister = true;
	}

	public override string ToString()
	{
		return IsRegister ? Register.ToString() : Value.ToString();
	}

	public long Value { get; private set; }
	public bool IsRegister { get; private set; }
	public char Register { get; private set; }
}

enum OpCode
{
	Set,
	Sub,
	Mul,
	Jnz,
}
