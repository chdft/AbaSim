using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Compiler
{
	[Serializable]
	public class IllegalArgumentListException : CompilerException
	{
		private const string MessageTemplate = "The argument list \"{0}\" is not valid for the operation {1} of type {2}.";

		public IllegalArgumentListException() { }
		public IllegalArgumentListException(string operation, IEnumerable<string> argumentList, Abacus16.InstructionType type) : base(string.Format(MessageTemplate, string.Join(", ", argumentList), operation, type)) { }
		public IllegalArgumentListException(string operation, IEnumerable<string> argumentList, Abacus16.InstructionType type, Exception inner) : base(string.Format(MessageTemplate, string.Join(", ", argumentList), operation, type), inner) { }
		protected IllegalArgumentListException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }
	}
}
