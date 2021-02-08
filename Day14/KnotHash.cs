using System;
using System.Collections.Generic;
using System.Linq;

internal class KnotHash
{
	internal static byte[] Calculate(string s)
	{
		var list = new byte[256];
		for (var i = 0; i < list.Length; i++)
		{
			list[i] = (byte)i;
		}
		var lengths = ProcessInput(s);

		var current = 0;
		var skip = 0;
		for (var i = 0; i < 64; i++)
		{
			Crunch(list, lengths, ref current, ref skip);
		}

		// dense hash
		var dense = new byte[16];
		for (var i = 0; i < 16; i++)
		{
			dense[i] = list.Skip(i * 16).Take(16).Aggregate((b1, b2) => (byte)(b1 ^ b2));
		}
		
		return dense;
	}

	private static void Crunch(byte[] list, IEnumerable<int> lengths, ref int current, ref int skip)
	{
		foreach (var l in lengths)
		{
			// reverse
			var x = current;
			var y = (current + l - 1) % list.Length;
			for (var i = 0; i < l / 2; i++)
			{
				var tmp = list[x];
				list[x] = list[y];
				list[y] = tmp;
				x = (x + 1) % list.Length;
				y = (list.Length + y - 1) % list.Length;
			}
			// move current
			current = (current + l + skip) % list.Length;
			// increase skip size
			skip++;
		}
	}

	private static List<int> ProcessInput(string s)
	{
		var lengths = new List<int>();
		foreach (var c in s)
		{
			lengths.Add((int)c);
		}
		lengths.Add(17);
		lengths.Add(31);
		lengths.Add(73);
		lengths.Add(47);
		lengths.Add(23);
		return lengths;
	}
}