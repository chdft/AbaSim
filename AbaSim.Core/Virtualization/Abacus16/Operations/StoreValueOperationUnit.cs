using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16.Operations
{
	class StoreValueOperationUnit : RegisterOperationUnit
	{
		public const byte OpCode = Bit.B6 + Bit.B5 + Bit.B0;

		public StoreValueOperationUnit(IMemoryProvider<Word> memory, IReadOnlyRegisterGroup registers)
			: base(registers)
		{
			Memory = memory;
		}

		protected IMemoryProvider<Word> Memory { get; private set; }

		protected override void InternalExecute()
		{
			if (VectorBit)
			{
				//UpdateVRegister(DestinationRegister, Memory[Registers[LeftRegister] + Registers[RightRegister]]);
				throw new IllegalOperationArgumentException("Vectors are not supported for store operations", Instruction);
			}
			else
			{
				UpdateMemory(Registers.Scalar[LeftRegister] + Registers.Scalar[RightRegister], Registers.Scalar[DestinationRegister]);
			}
		}
	}
}
