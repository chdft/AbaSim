using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Compiler
{
	public class CompileResult<TOutput>
	{
		public CompileResult(TOutput output, CompileLog log)
		{
			Output = output;
			Log = log;
		}

		public TOutput Output { get; private set; }

		public CompileLog Log { get; private set; }
	}
}
