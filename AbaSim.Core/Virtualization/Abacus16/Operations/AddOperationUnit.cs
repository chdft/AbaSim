using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16.Operations
{
	class AddOperationUnit : RegisterOperationUnit
	{
		public const byte OpCode = 0;

		public AddOperationUnit(Word[] registers) : base(registers) { }

		protected override void InternalExecute()
		{
			if (VectorBit)
			{
				throw new NotImplementedException();
			}
			else
			{
				//CHECK: where is the overflow written to?
				UpdateRegister(DestinationRegister, (Word)(Registers[LeftRegister].SignedValue + Registers[RightRegister].SignedValue));
			}
		}
	}
}
