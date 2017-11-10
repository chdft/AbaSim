using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization
{
	[Serializable]
	public class UnknownInstructionException : CpuException
	{
		public UnknownInstructionException() { }
		public UnknownInstructionException(string message) : base(message) { }
		public UnknownInstructionException(string message, Exception inner) : base(message, inner) { }
		protected UnknownInstructionException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }
	}
}
