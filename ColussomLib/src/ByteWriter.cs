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

	public void WriteByte(byte value)
	{
		if (Position == Bytes.Count)
		{
			Bytes.Add(value);
			Position++;
			return;
		}
		Bytes[Position++] = value;
	}

	public void WriteSByte(sbyte value)
	{
		if (Position == Bytes.Count)
		{
			Bytes.Add((byte)value);
			Position++;
			return;
		}
		Bytes[Position++] = (byte)value;
	}


	public void WriteUShort(ushort value)
	{
		if (Endianness == Endianness.Big)
		{
			WriteByte((byte)((value >> 8) & 0xff));
			WriteByte((byte)(value & 0xff));

		}
		else
		{
			WriteByte((byte)(value & 0xff));
			WriteByte((byte)((value >> 8) & 0xff));
		}
	}

	public void WriteShort(short value)
	{
		unchecked { WriteUShort((ushort)value); }
	}

	public void WriteUInt(uint value)
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

	public void WriteInt(int value)
	{
		unchecked { WriteUInt((uint)value); }
	}

	public void WriteULong(ulong value)
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

	public void WriteLong(long value)
	{
		unchecked { WriteULong((ulong)value); }
	}

	public void WriteULongLong(UInt128 value)
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

	public void WriteLongLong(Int128 value)
	{
		unchecked { WriteULongLong((UInt128)value); }
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
