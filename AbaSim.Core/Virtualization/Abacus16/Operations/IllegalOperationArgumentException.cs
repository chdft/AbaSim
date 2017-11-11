using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16.Operations
{
	[Serializable]
	public class IllegalOperationArgumentException : ExecutionException
	{
		public IllegalOperationArgumentException() { }
		public IllegalOperationArgumentException(string message, Word instruction) : base(message, instruction) { }
		public IllegalOperationArgumentException(string message, Exception inner, Word instruction) : base(message, inner, instruction) { }
		protected IllegalOperationArgumentException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }
	}
}
