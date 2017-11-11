using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16.Operations
{
	class SimpleJumOperationUnit:JumpOperationUnit
	{
		protected override void InternalExecute()
		{
			ProgramCounterChange = SignedConstant;
		}
	}
}
