using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16.Operations
{
	class LoadOperationUnit : RegisterOperationUnit
	{
		public const byte OpCode = Bit.B6 + Bit.B5;

		public LoadOperationUnit(IMemoryProvider<Word> memory, IReadOnlyRegisterGroup registers)
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
				throw new IllegalOperationArgumentException("Vectors are not supported for load operations", Instruction);
			}
			else
			{
				UpdateRegister(DestinationRegister, Memory[Registers.Scalar[LeftRegister] + Registers.Scalar[RightRegister]]);
			}
		}
	}
}
