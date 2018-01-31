using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16.Operations
{
	[AbaSim.Core.Compiler.Abacus16.AssemblyCode("bnz", OpCode, Compiler.Abacus16.InstructionType.Store)]
	class BranchNotZeroOperationUnit : StoreOperationUnit
	{
		public const byte OpCode = Bit.B5 + Bit.B0;

		public BranchNotZeroOperationUnit(RegisterGroup register) : base(register) { }

		protected override void InternalExecute()
		{
			if (Destination != Word.Empty)
			{
				ProgramCounterChange = SignedConstant;
			}
		}
	}
}
