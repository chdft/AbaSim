using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16.Operations
{
	public interface IOperationUnit
	{
		void Decode(Word instruction);

		void Execute();

		Word?[] UpdatedRegisters { get; }

		Vector[] UpadtedVRegisters { get; }

		int? UpdateMemoryAddress { get; }

		Word UpdateMemoryValue { get; }
	}
}
