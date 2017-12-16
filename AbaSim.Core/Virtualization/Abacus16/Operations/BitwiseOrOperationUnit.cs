using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16.Operations
{
	[AbaSim.Core.Compiler.Parsing.AssemblyCode("or", OpCode, Compiler.Parsing.InstructionType.Register)]
	class BitwiseOrOperationUnit : RegisterOperationUnit
	{
		public const byte OpCode = Bit.B4 + Bit.B3 + Bit.B0;

		public BitwiseOrOperationUnit(RegisterGroup register) : base(register) { }

		protected override void InternalExecute()
		{
			Destination =  Left | Right;
		}
	}
}
