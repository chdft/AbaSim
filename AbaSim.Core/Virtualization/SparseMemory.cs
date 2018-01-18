using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization
{
	public class SparseMemory<Word> : IMemoryProvider<Word>
	{
		public SparseMemory(Word[] initialContent, int size)
		{
			Size = size;
			BackingStore = new Dictionary<int, Word>(2 * initialContent.Length);
			for (int i = 0; i < initialContent.Length; i++)
			{
				BackingStore.Add(i, initialContent[i]);
			}
		}

		public SparseMemory(int size)
		{
			Size = size;
			BackingStore = new Dictionary<int, Word>();
		}

		protected IDictionary<int, Word> BackingStore { get; private set; }

		public int Size
		{
			get;
			private set;
		}

		public Word this[int index]
		{
			get
			{
				if (index >= Size || index < 0)
				{
					throw new MemoryAccessViolationException();
				}
				if (BackingStore.ContainsKey(index))
				{
					return BackingStore[index];
				}
				else
				{
					return default(Word);
				}
			}
			set
			{
				if (index >= Size || index < 0)
				{
					throw new MemoryAccessViolationException();
				}
				if (BackingStore.ContainsKey(index))
				{
					BackingStore[index] = value;
				}
				else
				{
					BackingStore.Add(index, value);
				}
			}
		}

		public void Flush() { }

		public IEnumerable<KeyValuePair<int, Word>> GetDebugDump()
		{
			return BackingStore;
		}
	}
}
