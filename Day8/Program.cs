using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

// Day 8

Console.WriteLine("START");
var sw = Stopwatch.StartNew();
Part1();
Console.WriteLine($"END (after {sw.Elapsed.TotalSeconds} seconds)");

void Part1()
{
	var cpu = new Cpu();
	cpu.Execute(ReadInput());
	System.Console.WriteLine(cpu);
	var maxValue = cpu.Registers.Values.Max();
	var reg = cpu.Registers.Single(r => r.Value == maxValue);
	System.Console.WriteLine($"Register with maximum value ({maxValue}) is '{reg.Key}'");
}

IEnumerable<Instruction> ReadInput()
{
	foreach (var line in File.ReadAllLines("input.txt"))
	{
		var tokens = line.Split(' ');
		var operationRegister = tokens[0];
		var operation = tokens[1] switch
		{
			"inc" => Operation.Increase,
			"dec" => Operation.Decrease,
			_ => throw new InvalidProgramException()
		};
		var by = Int32.Parse(tokens[2]);
		if (tokens[3] != "if")
		{
			throw new InvalidProgramException();
		}
		var conditionRegister = tokens[4];
		var condition = tokens[5] switch
		{
			"==" => Condition.Equal,
			"!=" => Condition.NotEqual,
			">=" => Condition.GreaterOrEqual,
			">" => Condition.Greater,
			"<=" => Condition.LessOrEqual,
			"<" => Condition.Less,
			_ => throw new InvalidProgramException()
		};
		var with = Int32.Parse(tokens[6]);
		yield return new Instruction(
			operationRegister,
			operation,
			by,
			conditionRegister,
			condition,
			with
		);
	}
}

internal enum Operation
{
	Increase,
	Decrease
}

internal enum Condition
{
	Equal,
	NotEqual,
	GreaterOrEqual,
	Greater,
	LessOrEqual,
	Less,
}

internal record Instruction(string OperationRegister, Operation Operation, int By, string ConditionRegister, Condition Condition, int With);

internal class Cpu
{
	private int _highestValue = Int32.MinValue;
	internal Dictionary<string, int> Registers { get; } = new Dictionary<string, int>();

	public override string ToString()
	{
		var sb = new StringBuilder();
		foreach (var r in Registers)
		{
			sb.AppendLine($"{r.Key}: {r.Value}");
		}
		sb.AppendLine($"Highest value stored in register: {_highestValue}");
		return sb.ToString();
	}

	internal void Execute(IEnumerable<Instruction> instructions)
	{
		foreach (var i in instructions)
		{
			var value = Registers.GetValueOrDefault(i.ConditionRegister);
			var check = false;
			switch (i.Condition)
			{
				case Condition.Equal:
					check = value == i.With;
					break;
				case Condition.NotEqual:
					check = value != i.With;
					break;
				case Condition.GreaterOrEqual:
					check = value >= i.With;
					break;
				case Condition.Greater:
					check = value > i.With;
					break;
				case Condition.LessOrEqual:
					check = value <= i.With;
					break;
				case Condition.Less:
					check = value < i.With;
					break;
			}
			if (check)
			{
				value = Registers.GetValueOrDefault(i.OperationRegister);
				switch (i.Operation)
				{
					case Operation.Increase:
						value += i.By;
						break;
					case Operation.Decrease:
						value -= i.By;
						break;
				}
				_highestValue = Math.Max(value, _highestValue);
				Registers[i.OperationRegister] = value;
			}
		}
	}
}

