using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16.Operations
{
	abstract class StoreOperationUnit : OperationUnit
	{
		private const int ConstantSize = 7;
		private const int RegisterSize = 3;
		private static readonly Word DestinationRegisterMask = (short)(Bit.S7 + Bit.S8 + Bit.S9);
		private static readonly byte DestinationRegisterShift = Word.Size - OpCodeSize - RegisterSize;
		private static readonly Word ConstantMask = (short)(Bit.S0 + Bit.S1 + Bit.S2 + Bit.S3 + Bit.S4 + Bit.S5 + Bit.S6);
		private static readonly byte ConstantShift = Word.Size - OpCodeSize - RegisterSize - ConstantSize;

		public StoreOperationUnit(IReadOnlyRegisterGroup register)
		{
			Registers = register;
		}

		protected RegisterIndex DestinationRegister { get; private set; }

		protected RegisterIndex LeftRegister { get; private set; }

		protected sbyte SignedConstant { get; private set; }

		protected byte UnsignedConstant { get; private set; }

		protected IReadOnlyRegisterGroup Registers { get; private set; }

		public override void Decode(Word instruction)
		{
			DestinationRegister = (RegisterIndex)((instruction & DestinationRegisterMask) >> DestinationRegisterShift);
			SignedConstant = (sbyte)((instruction & ConstantMask) >> ConstantShift).SignExtend(ConstantSize).SignedValue;
			UnsignedConstant = (byte)((instruction & ConstantMask) >> ConstantShift).UnsignedValue;
		}
	}
}
