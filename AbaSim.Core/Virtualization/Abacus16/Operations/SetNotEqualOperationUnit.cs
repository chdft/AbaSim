using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16.Operations
{
	class SetNotEqualOperationUnit : RegisterOperationUnit
	{
		public const byte OpCode = Bit.B4 + Bit.B2 + Bit.B0;

		public SetNotEqualOperationUnit(RegisterGroup register) : base(register) { }

		protected override void InternalExecute()
		{
			UpdateRegister(DestinationRegister, (Registers.Scalar[LeftRegister].SignedValue != Registers.Scalar[RightRegister].SignedValue ? Word.True : Word.False));
		}
	}
}
