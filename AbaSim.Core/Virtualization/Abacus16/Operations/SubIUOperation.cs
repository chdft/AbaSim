using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16.Operations
{
	class SubIUOperationUnit : ImmediateOperationUnit
	{
		public const byte OpCode = 2 ^ 3 + 2 ^ 2 + 2 ^ 1;

		public SubIUOperationUnit(Word[] registers) : base(registers) { }

		protected override void InternalExecute()
		{
			if (VectorBit)
			{
				throw new NotImplementedException();
			}
			else
			{
				//CHECK: where is the overflow written to?
				UpdateRegister(DestinationRegister, (Word)(Registers[LeftRegister].SignedValue - UnsignedConstant));
			}
		}
	}
}
