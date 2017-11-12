using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization
{
	public class ClockCycleScheduledEventArgs : EventArgs
	{
		public ClockCycleScheduledEventArgs(int programCounter)
		{
			ProgramCounter = programCounter;
		}

		public int ProgramCounter { get; private set; }
	}
}
