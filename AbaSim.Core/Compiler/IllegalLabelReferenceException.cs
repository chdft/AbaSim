using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Compiler
{
	[Serializable]
	public class IllegalLabelReferenceException : IllegalReferenceException
	{
		private const string MessageTemplate = "\"{0}\" does not refer to a valid label. Labels may not contain whitespace.";

		public IllegalLabelReferenceException() { }
		public IllegalLabelReferenceException(string reference)
			: base(string.Format(MessageTemplate, reference))
		{
			Reference = reference;
		}
		public IllegalLabelReferenceException(string reference, Exception inner)
			: base(string.Format(MessageTemplate, reference), inner)
		{
			Reference = reference;
		}
		protected IllegalLabelReferenceException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }
	}
}
