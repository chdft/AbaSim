using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16
{
	struct RegisterIndex
	{
		public static readonly RegisterIndex MaxValue = new RegisterIndex(7);

		private RegisterIndex(byte source)
		{
			Value = source;
		}

		private byte Value;

		public override int GetHashCode()
		{
			return Value.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (obj is RegisterIndex)
			{
				return ((RegisterIndex)obj) == this;
			}
			return false;
		}

		public static bool operator <(RegisterIndex a, RegisterIndex b)
		{
			return a.Value < b.Value;
		}
		public static bool operator >(RegisterIndex a, RegisterIndex b)
		{
			return a.Value > b.Value;
		}
		public static bool operator ==(RegisterIndex a, RegisterIndex b)
		{
			return a.Value == b.Value;
		}
		public static bool operator !=(RegisterIndex a, RegisterIndex b)
		{
			return a.Value != b.Value;
		}
		public static explicit operator RegisterIndex(byte source)
		{
			var r = new RegisterIndex();
			r.Value = source;
			return r;
		}
		public static implicit operator byte(RegisterIndex source)
		{
			return source.Value;
		}
	}
}
