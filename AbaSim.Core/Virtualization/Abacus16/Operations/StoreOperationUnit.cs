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

		public StoreOperationUnit(IRegisterGroup register)
		{
			Registers = register;
		}

		protected Word Destination { get; set; }

		private RegisterIndex DestinationIndex { get; set; }

		protected sbyte SignedConstant { get; private set; }

		protected byte UnsignedConstant { get; private set; }

		protected IRegisterGroup Registers { get; private set; }

		protected override void InternalDecode()
		{
			DestinationIndex = (RegisterIndex)((Instruction & DestinationRegisterMask) >> DestinationRegisterShift);

			Destination = Registers.Scalar[DestinationIndex];

			SignedConstant = (sbyte)((Instruction & ConstantMask) >> ConstantShift).SignExtend(ConstantSize).SignedValue;
			UnsignedConstant = (byte)((Instruction & ConstantMask) >> ConstantShift).UnsignedValue;
		}

		protected override void InternalReset() { }

		protected override void InternalWriteRegisterChanges()
		{
			Registers.Scalar[DestinationIndex] = Destination;
		}
	}
}
