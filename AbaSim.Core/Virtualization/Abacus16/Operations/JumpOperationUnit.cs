using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16.Operations
{
	abstract class JumpOperationUnit : OperationUnit
	{
		private static readonly Word ConstantMask = (short)(Bit.S0 + Bit.S1 + Bit.S2 + Bit.S3 + Bit.S4 + Bit.S5 + Bit.S6 + Bit.S7 + Bit.S8 + Bit.S9);
		private const int ConstantSize = 10;
		private const byte ConstantShift = Word.Size - OpCodeSize - ConstantSize;

		public JumpOperationUnit() { }

		protected short SignedConstant { get; private set; }

		protected ushort UnsignedConstant { get; private set; }

		public override void Decode(Word instruction)
		{
			Word constant = ((instruction & ConstantMask) >> ConstantShift);
			SignedConstant = constant.SignExtend(ConstantSize).SignedValue;
			UnsignedConstant = constant.UnsignedValue;
		}
	}
}
