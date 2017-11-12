using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Compiler.Parsing
{
	public enum InstructionType
	{
		Register,
		VRegister,
		Store,
		Immediate,
		Jump
	}
}
