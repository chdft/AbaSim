using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization
{
	public class ClockCycleScheduledEventArgs : EventArgs
	{
		public ClockCycleScheduledEventArgs(ICpu cpu)
		{
			Cpu = cpu;
		}

		public ICpu Cpu { get; private set; }
	}
}
