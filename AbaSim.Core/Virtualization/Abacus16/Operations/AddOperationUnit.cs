using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16.Operations
{
	[AbaSim.Core.Compiler.Abacus16.AssemblyCode("add", OpCode, Compiler.Abacus16.InstructionType.Register)]
	[AbaSim.Core.Compiler.Abacus16.AssemblyCode("vadd", OpCode, Compiler.Abacus16.InstructionType.VRegister)]
	class AddOperationUnit : RegisterOperationUnit
	{
		public const byte OpCode = 0;

		public AddOperationUnit(IRegisterGroup registers) : base(registers) { }

		protected override void InternalExecute()
		{
			if (VectorBit)
			{
				throw new NotImplementedException();
			}
			else
			{
				//CHECK: where is the overflow written to?
				Destination =  (Word)(Left.SignedValue + Right.SignedValue);
			}
		}
	}
}
