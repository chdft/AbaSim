﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization
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

		public ulong Misses { get; private set; }

		public ulong WriteBacks { get; private set; }

		public ulong Hits { get; private set; }

		protected IMemoryProvider<Word> BackingMemoryProvider { get; private set; }

		public void Flush()
		{
			FlushToBackingMemory();
			BackingMemoryProvider.Flush();
		}

		protected abstract void FlushToBackingMemory();

		protected void NotifyCacheMiss()
		{
			Misses++;
		}

		protected void NotifyWriteBack()
		{
			WriteBacks++;
		}

		protected void NotifyCacheHit()
		{
			Hits++;
		}

		public abstract IEnumerable<KeyValuePair<int, Word>> GetDebugDump();

		public abstract IEnumerable<KeyValuePair<int, Word>> GetLocalDebugDump();

		public abstract Word GetDebugValue(int index);

		public abstract void SetDebugValue(int index, Word value);
	}
}
