using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16
{
	public class Vector
	{
		public Vector()
		{
			Value = new Word[16];
		}

		public Word this[uint index]
		{
			get
			{
				return Value[index];
			}
			set
			{
				Value[index] = value;
			}
		}

		private Word[] Value;
	}
}
