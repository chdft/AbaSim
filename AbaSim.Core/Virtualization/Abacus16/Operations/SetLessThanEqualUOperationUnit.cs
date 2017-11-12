using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16.Operations
{
	[AbaSim.Core.Compiler.Parsing.AssemblyCode("sleu", OpCode, Compiler.Parsing.InstructionType.Register)]
	class SetLessThanEqualUOperationUnit : RegisterOperationUnit
	{
		public const byte OpCode = Bit.B4 + Bit.B1 + Bit.B0;

		public SetLessThanEqualUOperationUnit(RegisterGroup register) : base(register) { }

		protected override void InternalExecute()
		{
			UpdateRegister(DestinationRegister, (Registers.Scalar[LeftRegister].UnsignedValue <= Registers.Scalar[RightRegister].UnsignedValue ? Word.True : Word.False));
		}
	}
}
