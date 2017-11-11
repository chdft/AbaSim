using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core
{
	static class Bit
	{
		public const byte B0 = 0x1;
		public const byte B1 = 0x2;
		public const byte B2 = 0x4;
		public const byte B3 = 0x8;
		public const byte B4 = 0x10;
		public const byte B5 = 0x20;
		public const byte B6 = 0x40;
		public const byte B7 = 0x80;

		public const ushort S0 = 0x1;
		public const ushort S1 = 0x2;
		public const ushort S2 = 0x4;
		public const ushort S3 = 0x8;
		public const ushort S4 = 0x10;
		public const ushort S5 = 0x20;
		public const ushort S6 = 0x40;
		public const ushort S7 = 0x80;
		public const ushort S8 = 0x100;
		public const ushort S9 = 0x200;
		public const ushort S10 = 0x400;
		public const ushort S11 = 0x800;
		public const ushort S12 = 0x1000;
		public const ushort S13 = 0x2000;
		public const ushort S14 = 0x4000;
		public const ushort S15 = 0x8000;

		public static byte[] SwapHighLow(byte[] source)
		{
			return new byte[] { source[1], source[0] };
		}
	}
}
