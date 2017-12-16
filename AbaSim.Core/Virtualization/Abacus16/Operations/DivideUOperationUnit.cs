using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16.Operations
{
	[AbaSim.Core.Compiler.Parsing.AssemblyCode("divu", OpCode, Compiler.Parsing.InstructionType.Register)]
	[AbaSim.Core.Compiler.Parsing.AssemblyCode("vdivu", OpCode, Compiler.Parsing.InstructionType.VRegister)]
	class DivideUOperationUnit : RegisterOperationUnit
	{
		public const byte OpCode = Bit.B3 + Bit.B2 + Bit.B1 + Bit.B0;

		public DivideUOperationUnit(IRegisterGroup registers) : base(registers) { }

		protected override void InternalExecute()
		{
			if (VectorBit)
			{
				throw new NotImplementedException();
			}
			else
			{
				//CHECK: where is the overflow written to?
				Destination =  (Word)(Left.UnsignedValue / Right.UnsignedValue);
			}
		}
	}
}
