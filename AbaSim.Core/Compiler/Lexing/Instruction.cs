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
		public int SourceLine { get; set; }
		public IReadOnlyList<string> Arguments { get; set; }
		public string Comment { get; set; }

		public override string ToString()
		{
			string template = "{1}";
			if (Arguments.Count > 0)
			{
				template = template + " {2}";
			}
			if (!string.IsNullOrEmpty(Label))
			{
				template = "{0}: " + template;
			}
			if (!string.IsNullOrEmpty(Comment))
			{
				template = template + " //{3}";
			}
			return string.Format(template, Label, Operation, string.Join(", ", Arguments), Comment);
		}
	}
}
