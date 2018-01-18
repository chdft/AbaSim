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

		/// <summary>
		/// Gets or sets the value at <paramref name="index"/>.
		/// </summary>
		/// <param name="index">0 based offset</param>
		/// <returns>value at <paramref name="index"/></returns>
		/// <exception cref="MemoryAccessViolationException">When <paramref name="index"/> is not mapped to a value.</exception>
		Word this[int index] { get; set; }

		void Flush();

		IEnumerable<KeyValuePair<int, Word>> GetDebugDump();
	}
}
