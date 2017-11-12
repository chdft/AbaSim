﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16.Operations
{
	[AbaSim.Core.Compiler.Parsing.AssemblyCode("mulu", OpCode, Compiler.Parsing.InstructionType.Register)]
	[AbaSim.Core.Compiler.Parsing.AssemblyCode("vmulu", OpCode, Compiler.Parsing.InstructionType.VRegister)]
	class MultiplyUOperationUnit : RegisterOperationUnit
	{
		public const byte OpCode = Bit.B3 + Bit.B0;

		public MultiplyUOperationUnit(IReadOnlyRegisterGroup registers) : base(registers) { }

		protected override void InternalExecute()
		{
			if (VectorBit)
			{
				throw new NotImplementedException();
			}
			else
			{
				//CHECK: where is the overflow written to?
				UpdateRegister(DestinationRegister, (Word)(Registers.Scalar[LeftRegister].UnsignedValue * Registers.Scalar[RightRegister].UnsignedValue));
			}
		}
	}
}
