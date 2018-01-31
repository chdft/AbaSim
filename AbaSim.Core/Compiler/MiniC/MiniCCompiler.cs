using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Compiler.MiniC
{
	public class MiniCCompiler : ICompileStep<MiniCApplication, IEnumerable<TACode.TACodeInstruction>>
	{
		public IEnumerable<TACode.TACodeInstruction> Compile(MiniCApplication input, CompileLog log)
		{
			throw new NotImplementedException();
		}
	}
}
