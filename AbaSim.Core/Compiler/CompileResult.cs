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
				if (Log.ErrorOccured) { throw new CompilerException(); }

				return _Output;
			}
			private set { _Output = value; }
		}
		private TOutput _Output;

		/// <summary>
		/// Like <see cref="Output"/>, but does not throw even if an critical error occurred.
		/// Note that the result of an <see cref="CompilePipeline"/> may be invalid if a critical error occurred.
		/// </summary>
		public TOutput UnsafeOutput
		{
			get { return _Output; }
		}

		public CompileLog Log { get; private set; }
	}
}
