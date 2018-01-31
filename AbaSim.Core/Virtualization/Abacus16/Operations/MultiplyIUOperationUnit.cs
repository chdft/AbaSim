using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16.Operations
{
	[AbaSim.Core.Compiler.Abacus16.AssemblyCode("muliu", OpCode, Compiler.Abacus16.InstructionType.Immediate, ConstantRestriction = AbaSim.Core.Compiler.Abacus16.ConstantValueRestriction.Unsigned)]
	class MultiplyIUOperationUnit : ImmediateOperationUnit
	{
		public const byte OpCode = Bit.B3 + Bit.B1 + Bit.B0;

		public MultiplyIUOperationUnit(IRegisterGroup registers) : base(registers) { }

		protected override void InternalExecute()
		{
			Destination =  (Word)(Left.UnsignedValue * UnsignedConstant);
			Overflow = (Word)((Left.UnsignedValue * UnsignedConstant) - ushort.MaxValue);
		}
	}
}
