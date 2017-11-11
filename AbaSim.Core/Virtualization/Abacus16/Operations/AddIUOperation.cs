using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16.Operations
{
	class AddIUOperationUnit : ImmediateOperationUnit
	{
		public const byte OpCode = 2 ^ 2 + 2 ^ 1;

		public AddIUOperationUnit(Word[] registers) : base(registers) { }

		protected override void InternalExecute()
		{
			//CHECK: where is the overflow written to?
			UpdateRegister(DestinationRegister, (Word)(Registers[LeftRegister].SignedValue + UnsignedConstant));
		}
	}
}
