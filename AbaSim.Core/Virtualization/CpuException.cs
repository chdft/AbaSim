using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization
{
	[Serializable]
	public class CpuException : Exception
	{
		public CpuException() { }
		public CpuException(string message) : base(message) { }
		public CpuException(string message, Exception inner) : base(message, inner) { }
		protected CpuException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }
	}
}
