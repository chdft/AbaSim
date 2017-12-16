using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16.Operations
{
	[AbaSim.Core.Compiler.Parsing.AssemblyCode("and", OpCode, Compiler.Parsing.InstructionType.Register)]
	class BitwiseAndOperationUnit : RegisterOperationUnit
	{
		public const byte OpCode = Bit.B4 + Bit.B3;

		public BitwiseAndOperationUnit(RegisterGroup register) : base(register) { }

		protected override void InternalExecute()
		{
			Destination =  Left & Right;
		}
	}
}
