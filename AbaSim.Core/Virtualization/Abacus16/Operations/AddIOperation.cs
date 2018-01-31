using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16.Operations
{
	[AbaSim.Core.Compiler.Abacus16.AssemblyCode("addi", OpCode, Compiler.Abacus16.InstructionType.Immediate)]
	class AddIOperationUnit : ImmediateOperationUnit
	{
		public const byte OpCode = Bit.B1;

		public AddIOperationUnit(IRegisterGroup registers) : base(registers) { }

		protected override void InternalExecute()
		{
			//CHECK: where is the overflow written to?
			Destination =  (Word)(Left.SignedValue + SignedConstant);
		}
	}
}
