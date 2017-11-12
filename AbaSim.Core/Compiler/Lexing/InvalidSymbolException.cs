using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Compiler.Lexing
{
	[Serializable]
	public class InvalidSymbolException : Exception
	{
		protected const string Message = "The symbol \"{0}\" was not expected at line {1} offset {2}. Expected {3}.";

		public InvalidSymbolException() { }
		public InvalidSymbolException(string symbol, int line, int offset, string expected)
			: base(string.Format(Message, symbol, line, offset, expected))
		{
			Symbol = symbol;
			Line = line;
			Offset = offset;
		}
		public InvalidSymbolException(string symbol, int line, int offset, string expected, Exception inner)
			: base(string.Format(Message, symbol, line, offset, expected), inner)
		{
			Symbol = symbol;
			Line = line;
			Offset = offset;
		}
		protected InvalidSymbolException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }

		public string Symbol { get; private set; }
		public int Line { get; private set; }
		public int Offset { get; private set; }
	}
}
