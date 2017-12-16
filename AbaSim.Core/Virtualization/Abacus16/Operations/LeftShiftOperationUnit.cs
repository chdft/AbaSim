using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16.Operations
{
	[AbaSim.Core.Compiler.Parsing.AssemblyCode("sft", OpCode, Compiler.Parsing.InstructionType.Register)]
	class LeftShiftOperationUnit : RegisterOperationUnit
	{
		public const byte OpCode = Bit.B4 + Bit.B3 + Bit.B2;

		public LeftShiftOperationUnit(RegisterGroup register) : base(register) { }

		protected override void InternalExecute()
		{
			//CHECK: should this be a NOP for a 0 shift value?
			if (Right.SignedValue >= 0)
			{
				Destination =  Left << Right.SignedValue;
			}
			else
			{
				Destination =  Left >> -Right.SignedValue;
			}
		}
	}
}
