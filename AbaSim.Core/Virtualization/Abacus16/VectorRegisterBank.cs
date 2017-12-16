using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16
{
	class VectorRegisterBank : IRegisterBank<Vector>, IReadOnlyRegisterBank<Vector>
	{
		public VectorRegisterBank(Action changeHandler)
		{
			ChangeHandler = changeHandler;
			Reset();
		}

		public Vector this[RegisterIndex index]
		{
			get
			{
				return Store[index];
			}
			set
			{
				Store[index] = value;
				NotifyChange();
			}
		}

		private Vector[] Store;

		private Action ChangeHandler;

		public int Size
		{
			get { return RegisterIndex.MaxValue + 1; }
		}

		public void Reset()
		{
			Store = new Vector[Size];
		}

		private void NotifyChange()
		{
			if (ChangeHandler != null)
			{
				ChangeHandler();
			}
		}
	}
}
