using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization
{
	[Serializable]
	public class ProgramCounterOutOfBoundsException : CpuException
	{
		public ProgramCounterOutOfBoundsException() { }
		public ProgramCounterOutOfBoundsException(string message, int attemptedAddress)
			: base(message)
		{
			AttemptedAddress = attemptedAddress;
		}
		public ProgramCounterOutOfBoundsException(string message, Exception inner, int attemptedAddress)
			: base(message, inner)
		{
			AttemptedAddress = attemptedAddress;
		}
		protected ProgramCounterOutOfBoundsException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }

		public int AttemptedAddress { get; private set; }
	}
}
