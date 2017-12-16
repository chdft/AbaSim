using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16.Operations
{
	[AbaSim.Core.Compiler.Parsing.AssemblyCode("muliu", OpCode, Compiler.Parsing.InstructionType.Immediate, ConstantRestriction = AbaSim.Core.Compiler.Parsing.ValueRestriction.Unsigned)]
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
