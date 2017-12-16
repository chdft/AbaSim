using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16.Operations
{
	abstract class ImmediateOperationUnit : OperationUnit
	{
		private const int ConstantSize = 3;
		private const int RegisterSize = 3;
		private static readonly Word DestinationRegisterMask = (short)(Bit.S7 + Bit.S8 + Bit.S9);
		private static readonly byte DestinationRegisterShift = Word.Size - OpCodeSize - 1 * RegisterSize;
		private static readonly Word LeftRegisterMask = (short)(Bit.S4 + Bit.S5 + Bit.S6);
		private static readonly byte LeftRegisterShift = Word.Size - OpCodeSize - 2 * RegisterSize;
		private static readonly Word ConstantMask = (short)(Bit.S1 + Bit.S2 + Bit.S3);
		private static readonly byte ConstantShift = Word.Size - OpCodeSize - 2 * RegisterSize - ConstantSize;

		public ImmediateOperationUnit(IRegisterGroup register)
		{
			Registers = register;
		}

		protected Word Destination { get; set; }

		protected Word Left { get; private set; }

		protected Word Overflow { get; set; }

		private RegisterIndex DestinationIndex { get; set; }

		private RegisterIndex LeftIndex { get; set; }

		protected sbyte SignedConstant { get; private set; }

		protected byte UnsignedConstant { get; private set; }

		private IRegisterGroup Registers { get; set; }

		protected override void InternalDecode()
		{
			DestinationIndex = (RegisterIndex)((Instruction & DestinationRegisterMask) >> DestinationRegisterShift);
			LeftIndex = (RegisterIndex)((Instruction & LeftRegisterMask) >> LeftRegisterShift);

			Destination = Registers.Scalar[DestinationIndex];
			Left = Registers.Scalar[LeftIndex];

			Overflow = Registers.Overflow;

			SignedConstant = (sbyte)((Instruction & ConstantMask) >> ConstantShift).SignExtend(ConstantSize).SignedValue;
			UnsignedConstant = (byte)((Instruction & ConstantMask) >> ConstantShift).UnsignedValue;
		}

		protected override void InternalReset() { }

		protected override void InternalWriteRegisterChanges()
		{
			Registers.Scalar[DestinationIndex] = Destination;
			//CHECK: will always rewriting overflow (even for instructions not setting it per spec) cause problems?
			Registers.Overflow = Overflow;
		}
	}
}
