using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16
{
	[Serializable]
	public class ExecutionException : CpuException
	{
		public ExecutionException() { }
		public ExecutionException(string message) : base(message) { }
		public ExecutionException(string message, Exception inner) : base(message, inner) { }
		protected ExecutionException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }
	}
}
