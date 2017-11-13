using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16.Operations
{
	[AbaSim.Core.Compiler.Parsing.AssemblyCode("ldi", OpCode, Compiler.Parsing.InstructionType.Immediate, ConstantRestriction = AbaSim.Core.Compiler.Parsing.ValueRestriction.Unsigned)]
	class LoadIOperationUnit : ImmediateOperationUnit
	{
		public const byte OpCode = Bit.B5 + Bit.B4 + Bit.B1;

		public LoadIOperationUnit(IMemoryProvider<Word> memory, IReadOnlyRegisterGroup registers)
			: base(registers)
		{
			Memory = memory;
		}

		protected IMemoryProvider<Word> Memory { get; private set; }

		protected override void InternalExecute()
		{
			UpdateRegister(DestinationRegister, Memory[Registers.Scalar[LeftRegister] + UnsignedConstant]);
		}
	}
}
