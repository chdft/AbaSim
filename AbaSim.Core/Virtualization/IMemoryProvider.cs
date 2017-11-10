using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization
{
	public interface IMemoryProvider<Word> where Word : IWord
	{
		uint Size { get; }

		Word this[uint index] { get; set; }

		void Flush();
	}
}
