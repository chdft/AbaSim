using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Compiler
{
	[Serializable]
	public class UnmappedOperationException : CompilerException
	{
		private const string MessageTemplate = "The assembler operation \"{0}\" is not mapped to a native operation.";

		public UnmappedOperationException() { }
		public UnmappedOperationException(string assmeblerOperation)
			: base(string.Format(MessageTemplate, assmeblerOperation))
		{
			AssemblerOperation = assmeblerOperation;
		}
		public UnmappedOperationException(string assmeblerOperation, Exception inner)
			: base(string.Format(MessageTemplate, assmeblerOperation), inner)
		{
			AssemblerOperation = assmeblerOperation;
		}
		protected UnmappedOperationException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }

		public string AssemblerOperation { get; private set; }
	}
}
