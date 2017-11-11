using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16.Operations
{
	class MoveOperationUnit : StoreOperationUnit
	{
		public const byte OpCode = Bit.B6 + Bit.B5 + Bit.B3 + Bit.B2;

		public MoveOperationUnit(Word[] registers) : base(registers) { }

		protected override void InternalExecute()
		{
			UpdateRegister(DestinationRegister, UnsignedConstant);
		}
	}
}
