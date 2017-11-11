using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16.Operations
{
	class SubIOperationUnit : ImmediateOperationUnit
	{
		public const byte OpCode = 2 ^ 3 + 2 ^ 2;

		public SubIOperationUnit(Word[] registers) : base(registers) { }

		protected override void InternalExecute()
		{
			//CHECK: where is the overflow written to?
			UpdateRegister(DestinationRegister, (Word)(Registers[LeftRegister].SignedValue - SignedConstant));
		}
	}
}
