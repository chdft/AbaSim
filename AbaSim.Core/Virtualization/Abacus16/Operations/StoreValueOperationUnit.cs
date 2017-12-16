using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16.Operations
{
	[AbaSim.Core.Compiler.Parsing.AssemblyCode("st", OpCode, Compiler.Parsing.InstructionType.Register)]
	class StoreValueOperationUnit : RegisterOperationUnit
	{
		public const byte OpCode = Bit.B5 + Bit.B4 + Bit.B0;

		public StoreValueOperationUnit(IMemoryProvider<Word> memory, IRegisterGroup registers)
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
				ScheduleMemoryChange(Left + Right, Destination);
			}
		}
	}
}
