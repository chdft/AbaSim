using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16.Operations
{
	class LoadOperationUnit : RegisterOperationUnit
	{
		public const byte OpCode = 2 ^ 6 + 2 ^ 5;

		public LoadOperationUnit(IMemoryProvider<Word> memory, Word[] registers)
		{
			Memory = memory;
			Registers = registers;
		}

		protected IMemoryProvider<Word> Memory { get; private set; }

		protected Word[] Registers { get; private set; }

		protected override void InternalExecute()
		{
			if (VectorBit)
			{
				//UpdateVRegister(DestinationRegister, Memory[Registers[LeftRegister] + Registers[RightRegister]]);
				throw new IllegalOperationArgumentException("Vectors are not supported for load operations");
			}
			else
			{
				UpdateRegister(DestinationRegister, Memory[Registers[LeftRegister] + Registers[RightRegister]]);
			}
		}
	}
}
