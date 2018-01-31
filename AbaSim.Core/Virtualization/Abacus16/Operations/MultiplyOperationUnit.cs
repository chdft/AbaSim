using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16.Operations
{
	[AbaSim.Core.Compiler.Abacus16.AssemblyCode("mul", OpCode, Compiler.Abacus16.InstructionType.Register)]
	[AbaSim.Core.Compiler.Abacus16.AssemblyCode("vmul", OpCode, Compiler.Abacus16.InstructionType.VRegister)]
	class MultiplyOperationUnit : RegisterOperationUnit
	{
		public const byte OpCode = Bit.B3;

		public MultiplyOperationUnit(IRegisterGroup registers) : base(registers) { }

		protected override void InternalExecute()
		{
			if (VectorBit)
			{
				throw new NotImplementedException();
			}
			else
			{
				Destination = (Word)(Left.SignedValue * Right.SignedValue);
				Overflow = (Word)((Left.SignedValue * Right.SignedValue) - short.MaxValue);
			}
		}
	}
}
