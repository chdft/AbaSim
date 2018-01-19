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
				if (index >= Size || index < 0)
				{
					throw new MemoryAccessViolationException();
				}
				return Source[index + StartAddress];
			}
			set
			{
				if (index >= Size || index < 0)
				{
					throw new MemoryAccessViolationException();
				}
				Source[index + StartAddress] = value;
			}
		}

		public void Flush()
		{
			Source.Flush();
		}


		public IEnumerable<KeyValuePair<int, Word>> GetDebugDump()
		{
			return Source.GetDebugDump().Where(item => item.Key >= StartAddress && item.Key < StartAddress + Size);
		}


		public Word GetDebugValue(int index)
		{
			if (index >= Size || index < 0)
			{
				throw new MemoryAccessViolationException();
			}
			return Source.GetDebugValue(index + StartAddress);
		}

		public void SetDebugValue(int index, Word value)
		{
			if (index >= Size || index < 0)
			{
				throw new MemoryAccessViolationException();
			}
			Source.SetDebugValue(index + StartAddress, value);
		}
	}
}
