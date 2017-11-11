using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16.Operations
{
	abstract class RegisterOperationUnit : OperationUnit
	{
		private static readonly Word DestinationRegisterMask = (ushort)(Bit.S7 + Bit.S8 + Bit.S9);
		private static readonly byte DestinationRegisterShift = Word.Size - 6 - 3;
		private static readonly Word LeftRegisterMask = (short)(Bit.S4 + Bit.S5 + Bit.S6);
		private static readonly byte LeftRegisterShift = Word.Size - 6 - 3 - 3;
		private static readonly Word RightRegisterMask = (short)(Bit.S1 + Bit.S2 + Bit.S3);
		private static readonly byte RightRegisterShift = Word.Size - 6 - 3 - 3 - 3;
		private static readonly Word VectorBitMask = (short)(Bit.S0);
		private static readonly byte VectorBitShift = Word.Size - 6 - 3 - 3 - 1;

		public RegisterOperationUnit(Word[] register)
		{
			Registers = register;
		}

		protected RegisterIndex DestinationRegister { get; private set; }

		protected RegisterIndex LeftRegister { get; private set; }

		protected RegisterIndex RightRegister { get; private set; }

		protected bool VectorBit { get; private set; }

		protected Word[] Registers { get; private set; }

		public override void Decode(Word instruction)
		{
			DestinationRegister = (RegisterIndex)((instruction & DestinationRegisterMask) >> DestinationRegisterShift);
			LeftRegister = (RegisterIndex)((instruction & LeftRegisterMask) >> LeftRegisterShift);
			RightRegister = (RegisterIndex)((instruction & RightRegisterMask) >> RightRegisterShift);
			VectorBit = ((instruction & VectorBitMask) >> VectorBitShift) != 0;
		}
	}
}
