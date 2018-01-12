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
			if (log == null) { throw new ArgumentNullException("log"); }

			Output = output;
			Log = log;
		}

		public TOutput Output
		{
			get
			{
				if (Log.CriticalErrorOccured) { throw new CompilerException(); }

				return _Output;
			}
			private set { _Output = value; }
		}
		private TOutput _Output;

		public CompileLog Log { get; private set; }
	}
}
