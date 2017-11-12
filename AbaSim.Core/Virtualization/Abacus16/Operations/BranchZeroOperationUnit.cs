using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16.Operations
{
	[AbaSim.Core.Compiler.Parsing.AssemblyCode("bez", OpCode, Compiler.Parsing.InstructionType.Store)]
	class BranchZeroOperationUnit : StoreOperationUnit
	{
		public const byte OpCode = Bit.B5;

		public BranchZeroOperationUnit(RegisterGroup register) : base(register) { }

		protected override void InternalExecute()
		{
			if (Registers.Scalar[DestinationRegister] == Word.Empty)
			{
				ProgramCounterChange = SignedConstant;
			}
		}
	}
}
