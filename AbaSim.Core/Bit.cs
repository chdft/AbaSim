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

		private static readonly int[] Bounds = new int[]
		{
			0,
			1,
			2,
			4,
			8,
			16,
			32,
			64,
			128,
			256,
			512,
			1024,
			2048,
			4096,
			8191,
			16383,
			32767,
			65535,
			131071,
			262143,
			524287,
			1048575,
			2097151,
			4194303,
			9388607,
			16777215,
			33554431,
			67108863,
			134217727,
			268435455,
			536870911,
			1073741823,
			2147483647
		};

		public static byte[] SwapHighLow(byte[] source)
		{
			return new byte[] { source[1], source[0] };
		}

		public static ushort MaskFirstS(byte bitCount)
		{
			if (bitCount >= sizeof(ushort) * 8) { throw new ArgumentOutOfRangeException("bitCount"); }

			return (ushort)(ushort.MaxValue >> (sizeof(ushort) * 8 - bitCount));
		}

		public static int UnsignedUpperBound(byte bitCount)
		{
			if (bitCount + 1 >= Bounds.Length) { throw new ArgumentOutOfRangeException("bitCount"); }

			return (Bounds[bitCount + 1]) - 1;
			//return (-1 >> (sizeof(int) * 8 - (bitCount)));
		}

		public static int SignedUpperBound(byte bitCount)
		{
			if (bitCount >= Bounds.Length) { throw new ArgumentOutOfRangeException("bitCount"); }

			return Bounds[bitCount] - 1;
			//return (UnsignedUpperBound(bitCount) >> 1);
		}

		public static int SignedLowerBound(byte bitCount)
		{
			if (bitCount >= Bounds.Length) { throw new ArgumentOutOfRangeException("bitCount"); }

			return (-1 * Bounds[bitCount]);
			//return -1 << (bitCount);
		}

		public static int UpperBound(byte bitCount, bool unsigned)
		{
			if (unsigned)
			{
				return UnsignedUpperBound(bitCount);
			}
			else
			{
				return SignedUpperBound(bitCount);
			}
		}

		public static int LowerBound(byte bitCount, bool unsigned)
		{
			if (unsigned)
			{
				return 0;
			}
			else
			{
				return SignedLowerBound(bitCount);
			}
		}
	}
}
