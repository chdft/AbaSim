using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization
{
	public class MemoryAggregator<Word> : IMemoryProvider<Word> where Word : IWord
	{
		public MemoryAggregator(int size)
		{
			if (size < 0) { throw new ArgumentException("size must be greater than or equal to 0."); }
			Size = size;
		}

		private readonly List<MemoryMapping> Mappings = new List<MemoryMapping>();

		/// <summary>
		/// Gets or sets the value at <paramref name="index"/>.
		/// </summary>
		/// <param name="index">0 based offset</param>
		/// <returns>value at <paramref name="index"/></returns>
		/// <exception cref="MemoryAccessException">When <paramref name="index"/> is not mapped to a value.</exception>
		public Word this[int index]
		{
			get
			{
				var mapping = ResolveMapping(index);
				if (mapping == null) { throw new MemoryAccessViolationException(); }

				return mapping.Provider[index - mapping.StartIndex];
			}
			set
			{
				var mapping = ResolveMapping(index);
				if (mapping == null) { throw new MemoryAccessViolationException(); }

				mapping.Provider[index - mapping.StartIndex] = value;
			}
		}

		public int Size
		{
			get;
			private set;
		}

		public void Flush()
		{
			foreach (var mapping in Mappings)
			{
				mapping.Provider.Flush();
			}
		}

		public void AddMapping(int startIndex, IMemoryProvider<Word> provider)
		{
			Mappings.Add(new MemoryMapping(startIndex, provider));
		}

		public void RemoveMapping(IMemoryProvider<Word> provider)
		{
			Mappings.RemoveAll(mapping => mapping.Provider == provider);
		}

		private MemoryMapping ResolveMapping(int index) {
			return Mappings.FirstOrDefault(mapping => mapping.StartIndex <= index && mapping.EndIndex > index);
		}

		private class MemoryMapping
		{
			public MemoryMapping(int startIndex, IMemoryProvider<Word> provider)
			{
				StartIndex = startIndex;
				Provider = provider;
			}

			public readonly int StartIndex;
			public readonly IMemoryProvider<Word> Provider;
			public int EndIndex
			{
				get
				{
					return StartIndex + Provider.Size;
				}
			}
		}


		public IEnumerable<KeyValuePair<int, Word>> GetDebugDump()
		{
			foreach (var mapping in Mappings)
			{
				foreach (var item in mapping.Provider.GetDebugDump())
				{
					yield return new KeyValuePair<int, Word>(item.Key + mapping.StartIndex, item.Value);
				}
			}
		}
	}
}
