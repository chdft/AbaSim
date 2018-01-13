using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16.Operations
{
	[AbaSim.Core.Compiler.Parsing.AssemblyCode("jmp", OpCode, Compiler.Parsing.InstructionType.Store, FixedConstantValue = 0, ConstantRestriction = Compiler.Parsing.ValueRestriction.Fixed)]
	class AdvancedJumpOperationUnit : StoreOperationUnit
	{
		public const byte OpCode = Bit.B5 + Bit.B1;

		public AdvancedJumpOperationUnit(RegisterGroup register) : base(register) { }

		protected override void InternalExecute()
		{
			ProgramCounterChange = Destination.SignedValue;
		}
	}
}
