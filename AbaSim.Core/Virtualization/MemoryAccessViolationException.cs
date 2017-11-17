using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization
{
	[Serializable]
	public class MemoryAccessViolationException : Exception
	{
		public MemoryAccessViolationException() : this("The accessed address is not available.") { }
		public MemoryAccessViolationException(string message) : base(message) { }
		public MemoryAccessViolationException(string message, Exception inner) : base(message, inner) { }
		protected MemoryAccessViolationException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }
	}
}
