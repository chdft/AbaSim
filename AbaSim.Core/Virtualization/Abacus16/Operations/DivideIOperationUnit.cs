using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16.Operations
{
	[AbaSim.Core.Compiler.Abacus16.AssemblyCode("divi", OpCode, Compiler.Abacus16.InstructionType.Immediate)]
	class DivideIOperationUnit : ImmediateOperationUnit
	{
		public const byte OpCode = Bit.B3 + Bit.B2 + Bit.B1;

		public DivideIOperationUnit(IRegisterGroup registers) : base(registers) { }

		protected override void InternalExecute()
		{
			Destination =  (Word)(Left.SignedValue / SignedConstant);
			Overflow = (Word)(Left.SignedValue % SignedConstant);
		}
	}
}
