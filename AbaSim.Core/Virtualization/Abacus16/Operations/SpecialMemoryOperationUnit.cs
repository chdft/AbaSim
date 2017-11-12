using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16.Operations
{
	//BUG: add fixed arguments to support this multiplexing instruction
	//[AbaSim.Core.Compiler.Parsing.AssemblyCode("sync", OpCode, Compiler.Parsing.InstructionType.Store)]
	class SpecialMemoryOperationUnit : StoreOperationUnit
	{
		public const byte OpCode = Bit.B5 + Bit.B4 + Bit.B3 + Bit.B2 + Bit.B1 + Bit.B0;

		public SpecialMemoryOperationUnit(SerialAbacus16Cpu cpu, IReadOnlyRegisterGroup registers)
			: base(registers)
		{
			Cpu = cpu;
		}

		private SerialAbacus16Cpu Cpu;

		protected override void InternalExecute()
		{
			switch (UnsignedConstant)
			{
				//sync
				case 0:
					Cpu.Synchronize();
					break;
				//ovflw
				case Bit.B0:
					throw new NotImplementedException();
				//mvtm
				case Bit.B1:
					throw new NotImplementedException();
				//mvtl
				case Bit.B1 + Bit.B0:
					throw new NotImplementedException();
				default:
					throw new IllegalOperationArgumentException("Invalid c argument for special memory instruction.", Instruction);
			}
		}
	}
}
