using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Compiler.Parsing
{
	[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
	public sealed class AssemblyCodeAttribute : Attribute
	{
		public AssemblyCodeAttribute(string friendlyName, byte opCode, InstructionType type)
		{
			FriendlyName = friendlyName;
			OpCode = opCode;
			Type = type;
		}

		public string FriendlyName { get; private set; }

		public InstructionType Type { get; private set; }

		public byte OpCode { get; private set; }
	}
}
