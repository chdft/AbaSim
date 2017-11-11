using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16.Operations
{
	class DivideUOperationUnit : RegisterOperationUnit
	{
		public const byte OpCode = Bit.B3 + Bit.B2 + Bit.B1 + Bit.B0;

		public DivideUOperationUnit(IReadOnlyRegisterGroup registers) : base(registers) { }

		protected override void InternalExecute()
		{
			if (VectorBit)
			{
				throw new NotImplementedException();
			}
			else
			{
				//CHECK: where is the overflow written to?
				UpdateRegister(DestinationRegister, (Word)(Registers.Scalar[LeftRegister].UnsignedValue / Registers.Scalar[RightRegister].UnsignedValue));
			}
		}
	}
}
