using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization
{
	public interface IWord
	{
		byte[] RawValue { get; set; }
	}
}
