using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Compiler
{
	[Serializable]
	public class CompilerException : Exception
	{
		public CompilerException() { }
		public CompilerException(string message) : base(message) { }
		public CompilerException(string message, Exception inner) : base(message, inner) { }
		protected CompilerException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }
	}
}
