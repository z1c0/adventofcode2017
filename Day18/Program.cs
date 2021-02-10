using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

// Day 18

Console.WriteLine("START");
var sw = Stopwatch.StartNew();
Part1();
Part2();
Console.WriteLine($"END (after {sw.Elapsed.TotalSeconds} seconds)");

void Part1()
{
	var cpu = new Cpu();
	cpu.Run(ReadInput().ToList());
}

void Part2()
{
	var code = ReadInput().ToList(); 
	var cpu0 = new Cpu(0);
	var cpu1 = new Cpu(1);

	while (true)
	{
		var ret0 = cpu0.Run(code, cpu1);
		var ret1 = cpu1.Run(code, cpu0);

		if (ret0 == ExitCode.Waiting && !cpu0.Queue.Any() && ret1 == ExitCode.Waiting && !cpu1.Queue.Any())
		{
			break;
		}
	}

	Console.WriteLine($"CPU0 had {cpu0.NumberOfSendOperations} send operations.");
	Console.WriteLine($"CPU1 had {cpu1.NumberOfSendOperations} send operations.");
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
			"snd" => OpCode.Snd,
			"set" => OpCode.Set,
			"add" => OpCode.Add,
			"mul" => OpCode.Mul,
			"mod" => OpCode.Mod,
			"rcv" => OpCode.Rcv,
			"jgz" => OpCode.Jgz,
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
	private readonly Dictionary<char, long> _registers = new Dictionary<char, long>();
	private long _lastFrequency;
	internal List<long> Queue { get; } = new List<long>();

	public Cpu(long p = 0)
	{
		Pc = 0;
		_registers['p'] = p;
	}

	public int NumberOfSendOperations { get; set; }
	public int Pc { get; private set; }

	internal ExitCode Run(List<(OpCode op, RegOrValue rv1, RegOrValue rv2)> instructions, Cpu otherCpu = null)
	{
		while (Pc >= 0 && Pc < instructions.Count)
		{
			var i = instructions[Pc];
			switch (i.op)
			{
				case OpCode.Set:
					_registers[i.rv1.Register] = GetValue(i.rv2);
					Pc++;
					break;

				case OpCode.Add:
					_registers[i.rv1.Register] = _registers.GetValueOrDefault(i.rv1.Register) + GetValue(i.rv2);
					Pc++;
					break;

				case OpCode.Mul:
					_registers[i.rv1.Register] = _registers.GetValueOrDefault(i.rv1.Register) * GetValue(i.rv2);
					Pc++;
					break;

				case OpCode.Mod:
					_registers[i.rv1.Register] = _registers.GetValueOrDefault(i.rv1.Register) % GetValue(i.rv2);
					Pc++;
					break;

				case OpCode.Jgz:
					if (GetValue(i.rv1) > 0)
					{
						Pc += (int)GetValue(i.rv2);
					}
					else
					{
						Pc++;
					}
					break;

				case OpCode.Snd:
					if (otherCpu != null)
					{
						otherCpu.Queue.Add(GetValue(i.rv1));
						NumberOfSendOperations++;
					}
					else
					{
						_lastFrequency = GetValue(i.rv1);
					}
					Pc++;
					break;

				case OpCode.Rcv:
					if (otherCpu != null)
					{
						if (Queue.Any())
						{
							_registers[i.rv1.Register] = Queue.First();
							Queue.RemoveAt(0);
							Pc++;
						}
						else
						{
							return ExitCode.Waiting;
						}
					}
					else
					{
						if (GetValue(i.rv1) != 0)
						{
							System.Console.WriteLine($"Recovered frequency {_lastFrequency}");
							return ExitCode.Done;
						}
						Pc++;
					}
					break;

				default:
					throw new InvalidProgramException();
			}
		}
		return ExitCode.Done;
	}

	private long GetValue(RegOrValue rv)
	{
		if (rv.IsRegister)
		{
			return _registers.GetValueOrDefault(rv.Register);
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

	public long Value { get; private set; }
	public bool IsRegister { get; private set; }
	public char Register { get; private set; }
}

enum OpCode
{
	Snd,
	Set,
	Add,
	Mul,
	Mod,
	Rcv,
	Jgz,
}

enum ExitCode
{
	Waiting,
	Done,
}