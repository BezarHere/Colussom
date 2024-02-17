using System.Text;

namespace Colussom;

public class ByteWriter
{
	public ByteWriter()
	{
		Bytes = [];
		Position = 0;
		Endianness = Endianness.Big;
	}

	public ByteWriter(byte[] bytes)
	{
		Bytes = new(bytes);
		Position = 0;
		Endianness = Endianness.Big;
	}

	public ByteWriter(byte[] bytes, int index, Endianness endianness = Endianness.Big)
	{

		Bytes = new(bytes);
		Position = index;
		Endianness = endianness;
	}

	public void Write(byte[] bytes, int source_index, int length)
	{
		int writable_slots = Bytes.Count - Position;
		if (writable_slots < length)
			Bytes.AddRange(Bytes[(source_index + writable_slots)..]);

		for (int i = 0; i < writable_slots; i++)
		{
			Bytes[Position + i] = bytes[source_index + i];
		}
		Position += length;
	}

	public void Write(byte[] bytes) => Write(bytes, 0, bytes.Length);

	public void Write(byte value)
	{
		if (Position == Bytes.Count)
		{
			Bytes.Add(value);
			Position++;
			return;
		}
		Bytes[Position++] = value;
	}

	public void Write(sbyte value)
	{
		if (Position == Bytes.Count)
		{
			Bytes.Add((byte)value);
			Position++;
			return;
		}
		Bytes[Position++] = (byte)value;
	}


	public void Write(ushort value)
	{
		if (Endianness == Endianness.Big)
		{
			Write((byte)((value >> 8) & 0xff));
			Write((byte)(value & 0xff));

		}
		else
		{
			Write((byte)(value & 0xff));
			Write((byte)((value >> 8) & 0xff));
		}
	}

	public void Write(short value)
	{
		unchecked { Write((ushort)value); }
	}

	public void Write(uint value)
	{
		if (Endianness == Endianness.Big)
		{
			for (int i = 0; i < 4; i++)
			{
				if (i + Position == Bytes.Count)
					Bytes.Add((byte)((value >> (8 * (3 - i))) & 0xff));
				else
					Bytes[Position] = (byte)((value >> (8 * (3 - i))) & 0xff);
			}
		}
		else
		{
			for (int i = 0; i < 4; i++)
			{
				if (i + Position == Bytes.Count)
					Bytes.Add((byte)((value >> (8 * i)) & 0xff));
				else
					Bytes[Position] = (byte)((value >> (8 * i)) & 0xff);
			}
		}
		Position += 4;
	}

	public void Write(int value)
	{
		unchecked { Write((uint)value); }
	}

	public void Write(ulong value)
	{
		if (Endianness == Endianness.Big)
		{
			for (int i = 0; i < 8; i++)
			{
				if (i + Position == Bytes.Count)
					Bytes.Add((byte)((value >> (8 * (3 - i))) & 0xff));
				else
					Bytes[Position] = (byte)((value >> (8 * (3 - i))) & 0xff);
			}
		}
		else
		{
			for (int i = 0; i < 8; i++)
			{
				if (i + Position == Bytes.Count)
					Bytes.Add((byte)((value >> (8 * i)) & 0xff));
				else
					Bytes[Position] = (byte)((value >> (8 * i)) & 0xff);
			}
		}
		Position += 8;
	}

	public void Write(long value)
	{
		unchecked { Write((ulong)value); }
	}

	public void Write(UInt128 value)
	{
		if (Endianness == Endianness.Big)
		{
			for (int i = 0; i < 16; i++)
			{
				if (i + Position == Bytes.Count)
					Bytes.Add((byte)((value >> (8 * (3 - i))) & 0xff));
				else
					Bytes[Position] = (byte)((value >> (8 * (3 - i))) & 0xff);
			}
		}
		else
		{
			for (int i = 0; i < 16; i++)
			{
				if (i + Position == Bytes.Count)
					Bytes.Add((byte)((value >> (8 * i)) & 0xff));
				else
					Bytes[Position] = (byte)((value >> (8 * i)) & 0xff);
			}
		}
		Position += 16;
	}

	public void Write(Int128 value)
	{
		unchecked { Write((UInt128)value); }
	}

	public void WriteString(string value) => Write(Encoding.UTF8.GetBytes(value));

	public Endianness Endianness { get; set; } = Endianness.Big;
	public int Position
	{
		get => _position;
		set
		{
			if (value < 0)
				throw new ArgumentOutOfRangeException(nameof(value), value, "value should be greater then or equal to zero");
			if (value > Bytes.Count)
				throw new ArgumentOutOfRangeException(nameof(value), value, "value should be less or equal to the Bytes length");
			_position = value;
		}
	}

	public uint SpaceLeft { get => (uint)(Bytes.Count - Position); }

	private int _position;
	public List<byte> Bytes = [];
}
