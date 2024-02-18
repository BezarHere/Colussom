
namespace Colussom;

public class ByteStream : Stream
{
	public Stream Stream { get; set; }

	public override bool CanRead => Stream.CanRead;

	public override bool CanSeek => Stream.CanSeek;

	public override bool CanWrite => Stream.CanWrite;

	public override long Length => Stream.Length;

	public override long Position { get => Stream.Position; set => Stream.Position = value; }

	public override void Flush()
	{
		Stream.Flush();
	}

	public override int Read(byte[] buffer, int offset, int count) => Stream.Read(buffer, offset, count);

	public byte[] Read(int length)
	{
		byte[] bytes = new byte[length];
		Read(bytes, 0, length);
		return bytes;
	}

	public byte[] ReadAll()
	{
		Seek(0, SeekOrigin.Begin);
		return Read((int)Length);
	}

	public override long Seek(long offset, SeekOrigin origin) => Stream.Seek(offset, origin);

	public override void SetLength(long value) => Stream.SetLength(value);

	public override void Write(byte[] buffer, int offset, int count) => Stream.Write(buffer, offset, count);

	public void Write(byte[] buffer) => Write(buffer, 0, buffer.Length);

	public void WriteAll(byte[] buffer, int offset, int count)
	{
		SetLength(count);
		Seek(0, SeekOrigin.Begin);
		Write(buffer, offset, count);
	}

}
