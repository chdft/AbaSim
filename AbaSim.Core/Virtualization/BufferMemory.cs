using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization
{
	public class BufferMemory16 : IMemoryProvider<Abacus16.Word>
	{
		public BufferMemory16(byte[] buffer)
		{
			if (buffer.Length % 2 != 0) { throw new ArgumentException("buffer must contain an integer amount of Words"); }

			Buffer = new Abacus16.Word[buffer.Length / 2];
			for (int i = 0; i < Buffer.Length; i++)
			{
				Buffer[i] = BitConverter.ToInt16(buffer, i);
			}
		}
		public BufferMemory16(uint size)
		{
			Reset();
		}

		public Abacus16.Word[] Buffer { get; private set; }

		public int Size
		{
			get { return Buffer.Length; }
		}

		public void Flush() { }

		public void Reset()
		{
			Buffer = new Abacus16.Word[Size];
		}


		public Abacus16.Word this[int index]
		{
			get
			{
				return Buffer[index];
			}
			set
			{
				Buffer[index] = value;
			}
		}
	}
}
