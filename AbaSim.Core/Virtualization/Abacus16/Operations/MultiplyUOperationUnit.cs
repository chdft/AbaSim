using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16.Operations
{
	[AbaSim.Core.Compiler.Abacus16.AssemblyCode("mulu", OpCode, Compiler.Abacus16.InstructionType.Register)]
	[AbaSim.Core.Compiler.Abacus16.AssemblyCode("vmulu", OpCode, Compiler.Abacus16.InstructionType.VRegister)]
	class MultiplyUOperationUnit : RegisterOperationUnit
	{
		public const byte OpCode = Bit.B3 + Bit.B0;

		public MultiplyUOperationUnit(IRegisterGroup registers) : base(registers) { }

		protected override void InternalExecute()
		{
			if (VectorBit)
			{
				throw new NotImplementedException();
			}
			else
			{
				Destination = (Word)(Left.UnsignedValue * Right.UnsignedValue);
				Overflow = (Word)((Left.UnsignedValue * Right.UnsignedValue) - ushort.MaxValue);
			}
		}
	}
}
