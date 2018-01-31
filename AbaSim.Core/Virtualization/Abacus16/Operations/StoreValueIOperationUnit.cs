using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16.Operations
{
	[AbaSim.Core.Compiler.Abacus16.AssemblyCode("sti", OpCode, Compiler.Abacus16.InstructionType.Immediate)]
	class StoreValueIOperationUnit : ImmediateOperationUnit
	{
		public const byte OpCode = Bit.B5 + Bit.B4 + Bit.B1 + Bit.B0;

		public StoreValueIOperationUnit(IRegisterGroup registers) : base(registers) { }

		protected override void InternalExecute()
		{
			ScheduleMemoryChange(Left.UnsignedValue + UnsignedConstant, Destination);
		}
	}
}
