using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16.Operations
{
	[AbaSim.Core.Compiler.Abacus16.AssemblyCode("st", OpCode, Compiler.Abacus16.InstructionType.Register)]
	class StoreValueOperationUnit : RegisterOperationUnit
	{
		public const byte OpCode = Bit.B5 + Bit.B4 + Bit.B0;

		public StoreValueOperationUnit(IRegisterGroup registers) : base(registers) { }

		protected override void InternalExecute()
		{
			if (VectorBit)
			{
				//UpdateVRegister(DestinationRegister, Memory[Registers[LeftRegister] + Registers[RightRegister]]);
				throw new IllegalOperationArgumentException("Vectors are not supported for store operations", Instruction);
			}
			else
			{
				ScheduleMemoryChange(Left.UnsignedValue + Right.UnsignedValue, Destination);
			}
		}
	}
}
