using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16.Operations
{
	abstract class RegisterOperationUnit : OperationUnit
	{
		private const int RegisterSize = 3;
		private const int VectorBitSize = 1;
		private static readonly Word DestinationRegisterMask = (ushort)(Bit.S7 + Bit.S8 + Bit.S9);
		private static readonly byte DestinationRegisterShift = Word.Size - OpCodeSize - 1 * RegisterSize;
		private static readonly Word LeftRegisterMask = (short)(Bit.S4 + Bit.S5 + Bit.S6);
		private static readonly byte LeftRegisterShift = Word.Size - OpCodeSize - 2 * RegisterSize;
		private static readonly Word RightRegisterMask = (short)(Bit.S1 + Bit.S2 + Bit.S3);
		private static readonly byte RightRegisterShift = Word.Size - OpCodeSize - 3 * RegisterSize;
		private static readonly Word VectorBitMask = (short)(Bit.S0);
		private static readonly byte VectorBitShift = Word.Size - OpCodeSize - 3 * RegisterSize - VectorBitSize;

		public RegisterOperationUnit(IRegisterGroup register)
		{
			Registers = register;
		}

		private RegisterIndex DestinationIndex { get; set; }

		private RegisterIndex LeftIndex { get; set; }

		private RegisterIndex RightIndex { get; set; }

		protected Word Destination { get; set; }

		protected Word Left { get; private set; }

		protected Word Right { get; private set; }

		protected Word Overflow { get; set; }

		protected bool VectorBit { get; private set; }

		protected IRegisterGroup Registers { get; private set; }

		protected override void InternalDecode()
		{
			DestinationIndex = (RegisterIndex)((Instruction & DestinationRegisterMask) >> DestinationRegisterShift);
			LeftIndex = (RegisterIndex)((Instruction & LeftRegisterMask) >> LeftRegisterShift);
			RightIndex = (RegisterIndex)((Instruction & RightRegisterMask) >> RightRegisterShift);

			Destination = Registers.Scalar[DestinationIndex];
			Left = Registers.Scalar[LeftIndex];
			Right = Registers.Scalar[RightIndex];

			Overflow = Registers.Overflow;

			VectorBit = ((Instruction & VectorBitMask) >> VectorBitShift) != Word.False;
		}

		protected override void InternalReset() { }

		protected override void InternalWriteRegisterChanges()
		{
			Registers.Scalar[DestinationIndex] = Destination;
			Registers.Overflow = Overflow;
		}
	}
}
