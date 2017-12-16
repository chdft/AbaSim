using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16.Operations
{
	[AbaSim.Core.Compiler.Parsing.AssemblyCode("subu", OpCode, Compiler.Parsing.InstructionType.Register)]
	class SubUOperationUnit : RegisterOperationUnit
	{
		public const byte OpCode = Bit.B2 + Bit.B0;

		public SubUOperationUnit(IRegisterGroup registers) : base(registers) { }

		protected override void InternalExecute()
		{
			if (VectorBit)
			{
				throw new NotImplementedException();
			}
			else
			{
				//CHECK: where is the overflow written to?
				Destination =  (Word)(Left.UnsignedValue - Right.UnsignedValue);
			}
		}
	}
}
