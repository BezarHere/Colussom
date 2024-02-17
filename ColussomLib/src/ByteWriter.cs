namespace Colussom;

public class ByteWriter
{
	public ByteWriter()
	{

	}

	public ByteWriter(byte[] bytes) 
	{
		Bytes = bytes;
		Position = 0;
		Endianness = Endianness.Big;
	}

	public ByteWriter(byte[] bytes, long index, Endianness endianness = Endianness.Big) 
	{
		Bytes = bytes;
		Position = index;
		Endianness = endianness;
	}

	

	public Endianness Endianness { get; set; } = Endianness.Big;
	public long Position
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

	private long _position;
	public required byte[] Bytes;
}
