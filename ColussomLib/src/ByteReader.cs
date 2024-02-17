namespace Colussom;

public class ByteReader
{
	private ByteReader()
	{
		Bytes = [];
		Position = -(1 << 30);
	}

	public ByteReader(byte[] data)
	{
		Bytes = data;
		Position = 0;
	}

	public ByteReader(byte[] data, int index, Endianness endianness = Endianness.Big)
	{
		Bytes = data;
		Position = index;
		Endianness = endianness;
	}

	public void Seek(int offset, SeekOrigin origin = SeekOrigin.Begin)
	{
		switch (origin)
		{
			case SeekOrigin.Begin:
				Position = offset;
				return;
			case SeekOrigin.Current:
				Position += offset;
				return;
			case SeekOrigin.End:
				Position = Bytes.Length + offset;
				return;
			default: throw new NotSupportedException();
		}
	}

	public byte ReadByte()
	{
		return Bytes[Position++];
	}

	public sbyte ReadSByte()
	{
		return (sbyte)Bytes[Position++];
	}

	public short ReadShort()
	{
		if (Endianness == Endianness.Big)
		{
			return (short)((Bytes[Position++] << 8) | Bytes[Position++]);
		}
		return (short)(Bytes[Position++] | (Bytes[Position++] << 8));
	}

	public ushort ReadUShort()
	{
		if (Endianness == Endianness.Big)
		{
			return (ushort)((Bytes[Position++] << 8) | Bytes[Position++]);
		}
		return (ushort)(Bytes[Position++] | (Bytes[Position++] << 8));
	}

	public int ReadInt()
	{
		if (Endianness == Endianness.Big)
		{
			return (int)((Bytes[Position++] << 24) | (Bytes[Position++] << 16) | (Bytes[Position++] << 8) | Bytes[Position++]);
		}
		return (int)(Bytes[Position++] | (Bytes[Position++] << 8) | (Bytes[Position++] << 16) | (Bytes[Position++] << 24));
	}

	public uint ReadUInt()
	{
		if (Endianness == Endianness.Big)
		{
			return (uint)((Bytes[Position++] << 24) | (Bytes[Position++] << 16) | (Bytes[Position++] << 8) | Bytes[Position++]);
		}
		return (uint)(Bytes[Position++] | (Bytes[Position++] << 8) | (Bytes[Position++] << 16) | (Bytes[Position++] << 24));
	}

	public long ReadLong()
	{
		if (Endianness == Endianness.Big)
		{
			return ((long)ReadUInt() << 32) | ReadUInt();
		}
		uint low = ReadUInt();
		return ((long)ReadUInt() << 32) | low;
	}

	public ulong ReadULong()
	{
		if (Endianness == Endianness.Big)
		{
			return ((ulong)ReadUInt() << 32) | ReadUInt();
		}
		uint low = ReadUInt();
		return ((ulong)ReadUInt() << 32) | low;
	}

	public Int128 ReadLongLong()
	{
		if (Endianness == Endianness.Big)
		{
			return ((Int128)ReadULong() << 64) | ReadULong();
		}
		ulong low = ReadUInt();
		return ((Int128)ReadULong() << 64) | low;
	}

	public UInt128 ReadULongLong()
	{
		if (Endianness == Endianness.Big)
		{
			return ((UInt128)ReadULong() << 64) | ReadULong();
		}
		ulong low = ReadUInt();
		return ((UInt128)ReadULong() << 64) | low;
	}

	public byte[] Read(int length)
	{
		byte[] bytes = new byte[length];
		Array.Copy(Bytes, Position, bytes, 0, length);
		Position += length;
		return bytes;
	}

	public void Read(byte[] bytes)
	{
		Array.Copy(Bytes, Position, bytes, 0, bytes.Length);
		Position += bytes.Length;
	}

	public void Read(byte[] bytes, uint index, int length)
	{
		Array.Copy(Bytes, Position, bytes, index, length);
		Position += length;
	}

	public Endianness Endianness { get; set; } = Endianness.Big;
	public int Position
	{
		get => _position;
		set
		{
			if (value < 0)
				throw new ArgumentOutOfRangeException(nameof(value), value, "value should be greater then or equal to zero");
			if (value > Bytes.Length)
				throw new ArgumentOutOfRangeException(nameof(value), value, "value should be less or equal to the Bytes length");
			_position = value;
		}
	}

	public uint SpaceLeft { get => (uint)(Bytes.Length - Position); }

	private int _position;
	public readonly byte[] Bytes;
}
