using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16.Operations
{
	[AbaSim.Core.Compiler.Abacus16.AssemblyCode("subi", OpCode, Compiler.Abacus16.InstructionType.Immediate)]
	class SubIOperationUnit : ImmediateOperationUnit
	{
		public const byte OpCode = Bit.B2 + Bit.B1;

		public SubIOperationUnit(IRegisterGroup registers) : base(registers) { }

		protected override void InternalExecute()
		{
			//CHECK: where is the overflow written to?
			Destination =  (Word)(Left.SignedValue - SignedConstant);
		}
	}
}
