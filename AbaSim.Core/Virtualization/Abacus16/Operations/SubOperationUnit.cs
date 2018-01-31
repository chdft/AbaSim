﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16.Operations
{
	[AbaSim.Core.Compiler.Abacus16.AssemblyCode("sub", OpCode, Compiler.Abacus16.InstructionType.Register)]
	class SubOperationUnit : RegisterOperationUnit
	{
		public const byte OpCode = Bit.B2;

		public SubOperationUnit(IRegisterGroup registers) : base(registers) { }

		protected override void InternalExecute()
		{
			if (VectorBit)
			{
				throw new NotImplementedException();
			}
			else
			{
				//CHECK: where is the overflow written to?
				Destination =  (Word)(Left.SignedValue - Right.SignedValue);
			}
		}
	}
}
