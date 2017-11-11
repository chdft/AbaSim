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
		public ExecutionException(string message, Word instruction)
			: base(message)
		{
			Instruction = instruction;
		}
		public ExecutionException(string message, Exception inner, Word instruction)
			: base(message, inner)
		{
			Instruction = instruction;
		}
		protected ExecutionException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }

		public Word Instruction { get; private set; }
	}
}
