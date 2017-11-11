using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16
{
	public struct RegisterIndex
	{
		public static readonly RegisterIndex MaxValue = new RegisterIndex(7);

		private RegisterIndex(byte source)
		{
			Value = (byte)(source % 8);
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
			return new RegisterIndex(source);
		}

		public static implicit operator byte(RegisterIndex source)
		{
			return source.Value;
		}

		public static explicit operator RegisterIndex(ushort source)
		{
			return new RegisterIndex((byte)source);
		}

		public static implicit operator ushort(RegisterIndex source)
		{
			return source.Value;
		}

		public static explicit operator RegisterIndex(Word source)
		{
			return new RegisterIndex((byte)source.UnsignedValue);
		}

		public static implicit operator Word(RegisterIndex source)
		{
			return (Word)(ushort)source.Value;
		}
	}
}
