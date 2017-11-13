using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Compiler
{
	[Serializable]
	public class IllegalReferenceException : CompilerException
	{
		public IllegalReferenceException() { }
		public IllegalReferenceException(string message) : base(message) { }
		public IllegalReferenceException(string message, Exception inner) : base(message, inner) { }
		protected IllegalReferenceException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }

		public string Reference { get; protected set; }
	}
}
