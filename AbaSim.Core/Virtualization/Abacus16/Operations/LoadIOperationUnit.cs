﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16.Operations
{
	[AbaSim.Core.Compiler.Abacus16.AssemblyCode("ldi", OpCode, Compiler.Abacus16.InstructionType.Immediate, ConstantRestriction = AbaSim.Core.Compiler.Abacus16.ConstantValueRestriction.Unsigned)]
	class LoadIOperationUnit : ImmediateOperationUnit
	{
		public const byte OpCode = Bit.B5 + Bit.B4 + Bit.B1;

		public LoadIOperationUnit(IMemoryProvider<Word> memory, IRegisterGroup registers)
			: base(registers)
		{
			Memory = memory;
		}

		protected IMemoryProvider<Word> Memory { get; private set; }

		protected override void InternalExecute()
		{
			Destination =  Memory[Left.UnsignedValue + UnsignedConstant];
		}
	}
}
