using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16.Operations
{
	abstract class JumpOperationUnit : IOperationUnit
	{
		public void Decode(Word instruction)
		{
			throw new NotImplementedException();
		}

		public void Execute()
		{
			throw new NotImplementedException();
		}

		public Word?[] UpdatedRegisters
		{
			get { throw new NotImplementedException(); }
		}

		public Vector[] UpadtedVRegisters
		{
			get { throw new NotImplementedException(); }
		}

		public uint? UpdateMemoryAddress
		{
			get { throw new NotImplementedException(); }
		}

		public Word UpdateMemoryValue
		{
			get { throw new NotImplementedException(); }
		}
	}
}
