﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Compiler
{
	public interface ICompileStep<TInput, TOutput>
	{
		TOutput Compile(TInput input, CompileLog log);
	}
}
