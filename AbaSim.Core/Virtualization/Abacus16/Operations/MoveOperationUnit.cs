using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16.Operations
{
	class MoveOperationUnit : StoreOperationUnit
	{
		public const byte OpCode = 2 ^ 6 + 2 ^ 5 + 2 ^ 3 + 2 ^ 2;

		public MoveOperationUnit(Word[] registers) : base(registers) { }

		protected override void InternalExecute()
		{
			UpdateRegister(DestinationRegister, UnsignedConstant);
		}
	}
}
