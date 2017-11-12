using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16.Operations
{
	[AbaSim.Core.Compiler.Parsing.AssemblyCode("sti", OpCode, Compiler.Parsing.InstructionType.Immediate)]
	class StoreValueIOperationUnit : ImmediateOperationUnit
	{
		public const byte OpCode = Bit.B5 + Bit.B4 + Bit.B1 + Bit.B0;

		public StoreValueIOperationUnit(IMemoryProvider<Word> memory, IReadOnlyRegisterGroup registers)
			: base(registers)
		{
			Memory = memory;
		}

		protected IMemoryProvider<Word> Memory { get; private set; }

		protected override void InternalExecute()
		{
			UpdateMemory(Registers.Scalar[LeftRegister] + UnsignedConstant, Registers.Scalar[DestinationRegister]);
		}
	}
}
