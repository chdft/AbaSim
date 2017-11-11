using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16.Operations
{
	abstract class StoreOperationUnit : OperationUnit
	{
		private static readonly Word DestinationRegisterMask = (short)(2 ^ 7 + 2 ^ 8 + 2 ^ 9);
		private static readonly byte DestinationRegisterShift = Word.Size - 6 - 3;
		private static readonly Word ConstantMask = (short)(2 ^ 0 + 2 ^ 1 + 2 ^ 2 + 2 ^ 3 + 2 ^ 4 + 2 ^ 5 + 2 ^ 6);
		private static readonly byte ConstantShift = Word.Size - 6 - 3 - 7;

		public StoreOperationUnit(Word[] register)
		{
			Registers = register;
		}

		protected RegisterIndex DestinationRegister { get; private set; }

		protected RegisterIndex LeftRegister { get; private set; }

		protected sbyte SignedConstant { get; private set; }

		protected byte UnsignedConstant { get; private set; }

		protected Word[] Registers { get; private set; }

		public override void Decode(Word instruction)
		{
			DestinationRegister = (RegisterIndex)((instruction & DestinationRegisterMask) >> DestinationRegisterShift);
			SignedConstant = (sbyte)((instruction & ConstantMask) >> ConstantShift);
			UnsignedConstant = (byte)((instruction & ConstantMask) >> ConstantShift);
		}
	}
}
