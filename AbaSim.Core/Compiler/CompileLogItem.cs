using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Compiler
{
	public struct CompileLogItem
	{
		public CompileLogItemSeverity Severity;
		public string Message;
		public string Description;
		public string Location;
	}
}
