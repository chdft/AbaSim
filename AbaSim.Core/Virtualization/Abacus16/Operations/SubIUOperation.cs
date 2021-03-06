﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16.Operations
{
	[AbaSim.Core.Compiler.Parsing.AssemblyCode("subiu", OpCode, Compiler.Parsing.InstructionType.Immediate, ConstantRestriction = AbaSim.Core.Compiler.Parsing.ConstantValueRestriction.Unsigned)]
	class SubIUOperationUnit : ImmediateOperationUnit
	{
		public const byte OpCode = Bit.B2 + Bit.B1 + Bit.B0;

		public SubIUOperationUnit(IRegisterGroup registers) : base(registers) { }

		protected override void InternalExecute()
		{
			//CHECK: where is the overflow written to?
			Destination =  (Word)(Left.UnsignedValue - UnsignedConstant);
		}
	}
}
