using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16.Operations
{
	class BitwiseXorOperationUnit : RegisterOperationUnit
	{
		public const byte OpCode = Bit.B4 + Bit.B3 + Bit.B1 + Bit.B0;

		public BitwiseXorOperationUnit(RegisterGroup register) : base(register) { }

		protected override void InternalExecute()
		{
			UpdateRegister(DestinationRegister, Registers.Scalar[LeftRegister] ^ Registers.Scalar[RightRegister]);
		}
	}
}
