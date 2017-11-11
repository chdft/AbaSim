using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16.Operations
{
	class StoreValueOperationUnit : RegisterOperationUnit
	{
		public const byte OpCode = 2 ^ 6 + 2 ^ 5 + 2 ^ 0;

		public StoreValueOperationUnit(IMemoryProvider<Word> memory, Word[] registers)
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
				throw new IllegalOperationArgumentException("Vectors are not supported for store operations");
			}
			else
			{
				UpdateMemory(Registers[LeftRegister] + Registers[RightRegister], Registers[DestinationRegister]);
			}
		}
	}
}
