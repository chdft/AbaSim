using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Compiler
{
	[Serializable]
	public class IllegalRegisterReferenceException : IllegalReferenceException
	{
		private const string Message = "\"{0}\" does not refer to a valid register.";

		public IllegalRegisterReferenceException() { }
		public IllegalRegisterReferenceException(string reference)
			: base(string.Format(Message, reference))
		{
			Reference = reference;
		}
		public IllegalRegisterReferenceException(string reference, Exception inner)
			: base(string.Format(Message, reference), inner)
		{
			Reference = reference;
		}
		protected IllegalRegisterReferenceException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }
	}
}
