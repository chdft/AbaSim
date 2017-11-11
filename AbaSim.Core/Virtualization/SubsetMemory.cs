using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization
{
	public class SubsetMemory<Word> : IMemoryProvider<Word> where Word : IWord
	{
		public SubsetMemory(IMemoryProvider<Word> source, int startAddress, int size)
		{
			Source = source;
			StartAddress = startAddress;
			Size = size;
		}

		protected IMemoryProvider<Word> Source { get; private set; }

		protected int StartAddress { get; private set; }

		public int Size { get; private set; }

		public Word this[int index]
		{
			get
			{
				return Source[index + StartAddress];
			}
			set
			{
				Source[index + StartAddress] = value;
			}
		}

		public void Flush()
		{
			Source.Flush();
		}
	}
}
