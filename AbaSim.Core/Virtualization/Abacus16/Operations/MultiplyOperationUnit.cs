using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16.Operations
{
	class MultiplyOperationUnit : RegisterOperationUnit
	{
		public const byte OpCode = Bit.B3;

		public MultiplyOperationUnit(IReadOnlyRegisterGroup registers) : base(registers) { }

		protected override void InternalExecute()
		{
			if (VectorBit)
			{
				throw new NotImplementedException();
			}
			else
			{
				//CHECK: where is the overflow written to?
				UpdateRegister(DestinationRegister, (Word)(Registers.Scalar[LeftRegister].SignedValue * Registers.Scalar[RightRegister].SignedValue));
			}
		}
	}
}
