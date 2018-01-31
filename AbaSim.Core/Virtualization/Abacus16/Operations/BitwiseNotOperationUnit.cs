using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16.Operations
{
	[AbaSim.Core.Compiler.Abacus16.AssemblyCode("neg", OpCode, Compiler.Abacus16.InstructionType.Register)]
	[AbaSim.Core.Compiler.Abacus16.AssemblyCode("not", OpCode, Compiler.Abacus16.InstructionType.Register, Dialect = Compiler.Abacus16.Dialects.ChDFT)]
	class BitwiseNotOperationUnit : RegisterOperationUnit
	{
		public const byte OpCode = Bit.B4 + Bit.B3 + Bit.B1;

		public BitwiseNotOperationUnit(RegisterGroup register) : base(register) { }

		protected override void InternalExecute()
		{
			Destination =  ~Left;
		}
	}
}
