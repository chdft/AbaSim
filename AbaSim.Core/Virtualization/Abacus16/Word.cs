using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16
{
	[StructLayout(LayoutKind.Explicit)]
	public struct Word : IWord
	{
		[FieldOffset(0)]
		public short SignedValue;

		[FieldOffset(0)]
		public ushort UnsignedValue;

		public static implicit operator Word(ushort source)
		{
			var result = new Word();
			result.UnsignedValue = source;
			return result;
		}

		public static implicit operator Word(short source)
		{
			var result = new Word();
			result.SignedValue = source;
			return result;
		}

		public byte[] RawValue
		{
			get
			{
				return BitConverter.GetBytes(UnsignedValue);
			}
			set
			{
				UnsignedValue = BitConverter.ToUInt16(value, 0);
			}
		}
	}
}
