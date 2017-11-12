using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16.Operations
{
	[AbaSim.Core.Compiler.Parsing.AssemblyCode("add", OpCode, Compiler.Parsing.InstructionType.Register)]
	[AbaSim.Core.Compiler.Parsing.AssemblyCode("vadd", OpCode, Compiler.Parsing.InstructionType.VRegister)]
	class AddOperationUnit : RegisterOperationUnit
	{
		public const byte OpCode = 0;

		public AddOperationUnit(IReadOnlyRegisterGroup registers) : base(registers) { }

		protected override void InternalExecute()
		{
			if (VectorBit)
			{
				throw new NotImplementedException();
			}
			else
			{
				//CHECK: where is the overflow written to?
				UpdateRegister(DestinationRegister, (Word)(Registers.Scalar[LeftRegister].SignedValue + Registers.Scalar[RightRegister].SignedValue));
			}
		}
	}
}
