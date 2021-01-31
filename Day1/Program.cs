using System;
using System.Linq;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Day1
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("START");
			var sw = Stopwatch.StartNew();
			Part1();
			Part2();
			Console.WriteLine($"END (after {sw.Elapsed.TotalSeconds} seconds)");
		}
		private static void Part1()
		{
			var numbers = ReadInput().ToList();
			numbers.Add(numbers.First());
			var sum = 0;
			for (var i = 0; i < numbers.Count - 1; i++)
			{
				if (numbers[i] == numbers[i + 1])
				{
					sum += numbers[i];
				}
			}
			System.Console.WriteLine($"The solution to the captcha is: {sum}");
		}
		private static void Part2()
		{
			var numbers = ReadInput().ToList();
			var sum = 0;
			for (var i = 0; i < numbers.Count; i++)
			{
				var pos = (i + numbers.Count / 2) % numbers.Count;
				if (numbers[i] == numbers[pos])
				{
					sum += numbers[i];
				}
			}
			System.Console.WriteLine($"The solution to the new captcha is: {sum}");
		}

		private static IEnumerable<int> ReadInput()
		{
			foreach (var c in File.ReadAllText("input.txt"))
			{
				yield return Int32.Parse(c.ToString());
			}
		}
	}
}


