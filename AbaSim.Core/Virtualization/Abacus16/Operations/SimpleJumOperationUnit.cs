using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16.Operations
{
	[AbaSim.Core.Compiler.Parsing.AssemblyCode("j", OpCode, Compiler.Parsing.InstructionType.Jump)]
	class SimpleJumpOperationUnit : JumpOperationUnit
	{
		public const byte OpCode = Bit.B0 + Bit.B1 + Bit.B5;

		protected override void InternalExecute()
		{
			ProgramCounterChange = SignedConstant;
		}
	}
}
