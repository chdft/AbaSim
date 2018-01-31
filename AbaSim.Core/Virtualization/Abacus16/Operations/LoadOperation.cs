using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16.Operations
{
	[AbaSim.Core.Compiler.Abacus16.AssemblyCode("ld", OpCode, Compiler.Abacus16.InstructionType.Register)]
	class LoadOperationUnit : RegisterOperationUnit
	{
		public const byte OpCode = Bit.B5 + Bit.B4;

		public LoadOperationUnit(IMemoryProvider<Word> memory, IRegisterGroup registers)
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
				Destination =  Memory[Left.UnsignedValue + Right.UnsignedValue];
			}
		}
	}
}
