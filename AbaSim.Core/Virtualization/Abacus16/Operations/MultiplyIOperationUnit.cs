using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16.Operations
{
	[AbaSim.Core.Compiler.Parsing.AssemblyCode("muli", OpCode, Compiler.Parsing.InstructionType.Immediate)]
	class MultiplyIOperationUnit : ImmediateOperationUnit
	{
		public const byte OpCode = Bit.B3 + Bit.B1;

		public MultiplyIOperationUnit(IRegisterGroup registers) : base(registers) { }

		protected override void InternalExecute()
		{
			Destination =  (Word)(Left.SignedValue * SignedConstant);
			Overflow = (Word)((Left.SignedValue * SignedConstant) - short.MaxValue);
		}
	}
}
