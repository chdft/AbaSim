using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16.Operations
{
	class AddUOperationUnit : RegisterOperationUnit
	{
		public const byte OpCode = 2^0;

		public AddUOperationUnit(Word[] registers) : base(registers) { }

		protected override void InternalExecute()
		{
			if (VectorBit)
			{
				throw new NotImplementedException();
			}
			else
			{
				//CHECK: where is the overflow written to?
				UpdateRegister(DestinationRegister, (Word)(Registers[LeftRegister].UnsignedValue + Registers[RightRegister].UnsignedValue));
			}
		}
	}
}
