using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization
{
	public class MemoryAggregator<Word> : IMemoryProvider<Word> where Word : IWord
	{
		public MemoryAggregator(uint size)
		{
			Size = size;
		}

		private readonly List<MemoryMapping> Mappings = new List<MemoryMapping>();

		public Word this[uint index]
		{
			get
			{
				var mapping = ResolveMapping(index);
				return mapping.Provider[index - mapping.StartIndex];
			}
			set
			{
				var mapping = ResolveMapping(index);
				mapping.Provider[index - mapping.StartIndex] = value;
			}
		}

		public uint Size
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

		public void AddMapping(uint startIndex, IMemoryProvider<Word> provider)
		{
			Mappings.Add(new MemoryMapping(startIndex, provider));
		}

		public void RemoveMapping(IMemoryProvider<Word> provider)
		{
			Mappings.RemoveAll(mapping => mapping.Provider == provider);
		}

		private MemoryMapping ResolveMapping(uint index) {
			return Mappings.First(mapping => mapping.StartIndex <= index && mapping.EndIndex > index);
		}

		private struct MemoryMapping
		{
			public MemoryMapping(uint startIndex, IMemoryProvider<Word> provider)
			{
				StartIndex = startIndex;
				Provider = provider;
			}

			public readonly uint StartIndex;
			public readonly IMemoryProvider<Word> Provider;
			public uint EndIndex
			{
				get
				{
					return StartIndex + Provider.Size;
				}
			}
		}
	}
}
