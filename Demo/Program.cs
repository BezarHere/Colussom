using Colussom;
using System.Text;

namespace Demo;

internal class Program
{
	static void Main(string[] args)
	{
		ByteReader reader = new([255, 10, 1, 0]);
		Console.WriteLine(reader.ReadUInt());
		reader.Seek(0, SeekOrigin.Begin);
		reader.Endianness = Endianness.Little;
		Console.WriteLine(reader.ReadUInt());
		Console.WriteLine(Encoding.UTF8.GetString(reader.Bytes));
	}
}
