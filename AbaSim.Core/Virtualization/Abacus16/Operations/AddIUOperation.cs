using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16.Operations
{
	[AbaSim.Core.Compiler.Abacus16.AssemblyCode("addiu", OpCode, Compiler.Abacus16.InstructionType.Immediate, ConstantRestriction = AbaSim.Core.Compiler.Abacus16.ConstantValueRestriction.Unsigned)]
	class AddIUOperationUnit : ImmediateOperationUnit
	{
		public const byte OpCode = Bit.B1 + Bit.B0;

		public AddIUOperationUnit(IRegisterGroup registers) : base(registers) { }

		protected override void InternalExecute()
		{
			//CHECK: where is the overflow written to?
			Destination =  (Word)(Left.UnsignedValue + UnsignedConstant);
		}
	}
}
