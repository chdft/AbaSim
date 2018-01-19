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
					NotifyCacheHit();
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
						NotifyWriteBack();
					}
					item.Value = value;
					item.SourceAddress = index;
					item.Valid = true;
					NotifyCacheMiss();
				}
				Cache[index % Cache.Length] = item;
			}
		}

		private CacheItem[] Cache;

		protected override void FlushToBackingMemory()
		{
			foreach (var item in Cache)
			{
				BackingMemoryProvider[item.SourceAddress] = item.Value;
			}
		}

		public override Word GetDebugValue(int index)
		{
			return BackingMemoryProvider.GetDebugValue(index);
		}

		public override void SetDebugValue(int index, Word value)
		{
			BackingMemoryProvider.SetDebugValue(index, value);
		}

		public override IEnumerable<KeyValuePair<int, Word>> GetDebugDump()
		{
			//merge updated local values with changes in cache
			return BackingMemoryProvider.GetDebugDump().Select(item =>
			{
				var citem = Cache.FirstOrDefault(i => i.SourceAddress == item.Key);
				if (!citem.Valid)
				{
					return item;
				}
				else
				{
					return new KeyValuePair<int,Word>(citem.SourceAddress, citem.Value);
				}
			});
		}

		public override IEnumerable<KeyValuePair<int, Word>> GetLocalDebugDump()
		{
			return Cache.Where(item => item.Valid).Select(item => new KeyValuePair<int, Word>(item.SourceAddress, item.Value));
		}

		protected struct CacheItem
		{
			public bool Valid;
			public Word Value;
			public int SourceAddress;
		}
	}
}
