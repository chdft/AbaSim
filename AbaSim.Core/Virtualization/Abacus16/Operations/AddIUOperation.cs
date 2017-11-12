using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16.Operations
{
	[AbaSim.Core.Compiler.Parsing.AssemblyCode("addiu", OpCode, Compiler.Parsing.InstructionType.Immediate)]
	class AddIUOperationUnit : ImmediateOperationUnit
	{
		public const byte OpCode = Bit.B2 + Bit.B1;

		public AddIUOperationUnit(IReadOnlyRegisterGroup registers) : base(registers) { }

		protected override void InternalExecute()
		{
			//CHECK: where is the overflow written to?
			UpdateRegister(DestinationRegister, (Word)(Registers.Scalar[LeftRegister].SignedValue + UnsignedConstant));
		}
	}
}
