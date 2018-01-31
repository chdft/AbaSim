using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Compiler.TACode
{
	public class TACodeCompiler:ICompileStep<IEnumerable<TACodeInstruction>, IEnumerable<Abacus16.AssemblerInstruction>>
	{
		public IEnumerable<Abacus16.AssemblerInstruction> Compile(IEnumerable<TACodeInstruction> input, CompileLog log)
		{
			throw new NotImplementedException();
		}
	}
}
