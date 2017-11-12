using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16.Operations
{
	class LeftShiftOperationUnit : RegisterOperationUnit
	{
		public const byte OpCode = Bit.B4 + Bit.B3 + Bit.B2;

		public LeftShiftOperationUnit(RegisterGroup register) : base(register) { }

		protected override void InternalExecute()
		{
			//BUG: spec is not clear on wheter rr is interpreted as signed or unsigned
			UpdateRegister(DestinationRegister, Registers.Scalar[LeftRegister] << Registers.Scalar[RightRegister].SignedValue);
		}
	}
}
