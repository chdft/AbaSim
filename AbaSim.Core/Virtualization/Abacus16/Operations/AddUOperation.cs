using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16.Operations
{
	[AbaSim.Core.Compiler.Abacus16.AssemblyCode("addu", OpCode, Compiler.Abacus16.InstructionType.Register)]
	[AbaSim.Core.Compiler.Abacus16.AssemblyCode("vaddu", OpCode, Compiler.Abacus16.InstructionType.VRegister)]
	class AddUOperationUnit : RegisterOperationUnit
	{
		public const byte OpCode = Bit.B0;

		public AddUOperationUnit(IRegisterGroup registers) : base(registers) { }

		protected override void InternalExecute()
		{
			if (VectorBit)
			{
				throw new NotImplementedException();
			}
			else
			{
				//CHECK: where is the overflow written to?
				Destination =  (Word)(Left.UnsignedValue + Right.UnsignedValue);
			}
		}
	}
}
