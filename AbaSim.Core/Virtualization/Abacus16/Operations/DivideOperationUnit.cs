using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16.Operations
{
	[AbaSim.Core.Compiler.Parsing.AssemblyCode("div", OpCode, Compiler.Parsing.InstructionType.Register)]
	[AbaSim.Core.Compiler.Parsing.AssemblyCode("vdiv", OpCode, Compiler.Parsing.InstructionType.VRegister)]
	class DivideOperationUnit : RegisterOperationUnit
	{
		public const byte OpCode = Bit.B3 + Bit.B2;

		public DivideOperationUnit(IRegisterGroup registers) : base(registers) { }

		protected override void InternalExecute()
		{
			if (VectorBit)
			{
				throw new NotImplementedException();
			}
			else
			{
				Destination =  (Word)(Left.SignedValue / Right.SignedValue);
				Overflow = (Word)(Left.SignedValue % Right.SignedValue);
			}
		}
	}
}
