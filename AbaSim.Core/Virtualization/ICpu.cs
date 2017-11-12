using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core
{
	public interface ICpu
	{
		int ProgramCounter { get; }

		void ClockCycle();

		void Reset();

		void Synchronize();
	}
}
