using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16.Operations
{
	[AbaSim.Core.Compiler.Abacus16.AssemblyCode("sltu", OpCode, Compiler.Abacus16.InstructionType.Register, ConstantRestriction = AbaSim.Core.Compiler.Abacus16.ConstantValueRestriction.Unsigned)]
	class SetLessThanUOperationUnit : RegisterOperationUnit
	{
		public const byte OpCode = Bit.B4 + Bit.B0;

		public SetLessThanUOperationUnit(RegisterGroup register) : base(register) { }

		protected override void InternalExecute()
		{
			Destination =  (Left.UnsignedValue < Right.UnsignedValue ? Word.True : Word.False);
		}
	}
}
