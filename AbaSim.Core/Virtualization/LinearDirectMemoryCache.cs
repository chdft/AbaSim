using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization
{
	public class LinearDirectMemoryCache<Word> : MemoryCache<Word> where Word : IWord
	{
		public LinearDirectMemoryCache(IMemoryProvider<Word> backingMemoryProvider, int cacheSize)
			: base(backingMemoryProvider)
		{
			Cache = new CacheItem[cacheSize];
		}

		public override Word this[int index]
		{
			get
			{
				var item = Cache[index % Cache.Length];
				if (item.SourceAddress == index && item.Valid)
				{
					return item.Value;
				}
				else
				{
					var value = BackingMemoryProvider[index];
					NotifyCacheMiss();
					item.Value = value;
					item.SourceAddress = index;
					item.Valid = true;
					Cache[index % Cache.Length] = item;
					return value;
				}
			}
			set
			{
				var item = Cache[index % Cache.Length];
				if (item.SourceAddress == index && item.Valid)
				{
					item.Value = value;
				}
				else
				{
					if (item.Valid)
					{
						BackingMemoryProvider[item.SourceAddress] = item.Value;
						//CHECK: count this, even for invalid values?
						NotifyCacheMiss();
					}
					item.Value = value;
					item.SourceAddress = index;
					item.Valid = true;
				}
				Cache[index % Cache.Length] = item;
			}
		}

		protected override void FlushToBackingMemory()
		{
			foreach (var item in Cache)
			{
				BackingMemoryProvider[item.SourceAddress] = item.Value;
			}
		}

		private CacheItem[] Cache;

		protected struct CacheItem
		{
			public bool Valid;
			public Word Value;
			public int SourceAddress;
		}
	}
}
