using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization
{
	public interface IMemoryProvider<Word> where Word : IWord
	{
		int Size { get; }

		Word this[int index] { get; set; }

		void Flush();
	}
}
