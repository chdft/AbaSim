using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16.Operations
{
	class SpecialMemoryOperationUnit : StoreOperationUnit
	{
		public SpecialMemoryOperationUnit(SerialAbacus16Cpu cpu, Word[] registers)
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
				case 2 ^ 0:

					break;
				//mvtm
				case 2 ^ 1:

					break;
				//mvtl
				case 2 ^ 1 + 2 ^ 0:

					break;
				default:
					throw new IllegalOperationArgumentException("Invalid c argument for special memory instruction.", Instruction);
			}
		}
	}
}
