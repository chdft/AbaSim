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
		public IllegalOperationArgumentException(string message) : base(message) { }
		public IllegalOperationArgumentException(string message, Exception inner) : base(message, inner) { }
		protected IllegalOperationArgumentException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }
	}
}
