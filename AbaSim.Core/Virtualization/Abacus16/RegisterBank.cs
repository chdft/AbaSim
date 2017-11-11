using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16
{
	class RegisterBank<T> : IRegisterBank<T>, IReadOnlyRegisterBank<T>
	{
		public RegisterBank()
		{
			Reset();
		}

		public T this[RegisterIndex index]
		{
			get
			{
				return Store[index];
			}
			set
			{
				Store[index] = value;
			}
		}

		private T[] Store;

		public void Reset()
		{
			Store = new T[RegisterIndex.MaxValue + 1];
		}
	}
}
