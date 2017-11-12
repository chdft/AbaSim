using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Compiler.Lexing
{
	public class Instruction
	{
		public string Label { get; set; }
		public string Operation { get; set; }
		public int Index { get; set; }
		public IReadOnlyList<string> Arguments { get; set; }
		public string Comment { get; set; }
	}
}
