﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16.Operations
{
	[AbaSim.Core.Compiler.Abacus16.AssemblyCode("slt", OpCode, Compiler.Abacus16.InstructionType.Register)]
	class SetLessThanOperationUnit:RegisterOperationUnit
	{
		public const byte OpCode = Bit.B4;

		public SetLessThanOperationUnit(RegisterGroup register) : base(register) { }

		protected override void InternalExecute()
		{
			Destination =  (Left.SignedValue < Right.SignedValue ? Word.True : Word.False);
		}
	}
}
