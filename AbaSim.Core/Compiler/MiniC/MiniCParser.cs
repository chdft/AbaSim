using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Compiler.MiniC
{
	//CHECK: split this into Lexer and Parser?
	public class MiniCParser : ICompileStep<string, MiniCApplication>
	{
		public MiniCApplication Compile(string input, CompileLog log)
		{
			throw new NotImplementedException();
		}
	}
}
