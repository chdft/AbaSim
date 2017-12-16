using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16
{
	public abstract class MemoryCache<Word> : IMemoryProvider<Word> where Word : IWord
	{
		public MemoryCache(IMemoryProvider<Word> backingMemoryProvider)
		{
			BackingMemoryProvider = backingMemoryProvider;
		}

		public int Size
		{
			get { return BackingMemoryProvider.Size; }
		}

		public abstract Word this[int index]
		{
			get;
			set;
		}

		protected IMemoryProvider<Word> BackingMemoryProvider { get; private set; }

		public void Flush()
		{
			FlushToBackingMemory();
			BackingMemoryProvider.Flush();
		}

		protected abstract void FlushToBackingMemory();
	}
}
