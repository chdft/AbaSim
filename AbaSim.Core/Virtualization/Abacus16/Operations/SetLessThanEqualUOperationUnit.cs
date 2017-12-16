using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16.Operations
{
	[AbaSim.Core.Compiler.Parsing.AssemblyCode("sleu", OpCode, Compiler.Parsing.InstructionType.Register, ConstantRestriction = AbaSim.Core.Compiler.Parsing.ValueRestriction.Unsigned)]
	class SetLessThanEqualUOperationUnit : RegisterOperationUnit
	{
		public const byte OpCode = Bit.B4 + Bit.B1 + Bit.B0;

		public SetLessThanEqualUOperationUnit(RegisterGroup register) : base(register) { }

		protected override void InternalExecute()
		{
			Destination =  (Left.UnsignedValue <= Right.UnsignedValue ? Word.True : Word.False);
		}
	}
}
