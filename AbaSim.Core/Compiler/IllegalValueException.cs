using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Compiler
{
	[Serializable]
	public class ValueOutOfBoundsException : CompilerException
	{
		private const string Message = "The value {0} is out of bounds. Expected value in bounds [{1};{2}]";

		public ValueOutOfBoundsException() { }
		public ValueOutOfBoundsException(int value, int min, int max) : base(string.Format(Message, value, min, max)) { }
		public ValueOutOfBoundsException(int value, int min, int max, Exception inner) : base(string.Format(Message, value, min, max), inner) { }
		protected ValueOutOfBoundsException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }
	}
}
