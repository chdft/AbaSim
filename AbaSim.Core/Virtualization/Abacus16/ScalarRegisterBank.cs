using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16
{
	class ScalarRegisterBank : IRegisterBank<Word>, IReadOnlyRegisterBank<Word>
	{
		public ScalarRegisterBank(Action changeHandler)
		{
			ChangeHandler = changeHandler;
			Reset();
		}

		public Word this[RegisterIndex index]
		{
			get
			{
				return Store[index];
			}
			set
			{
				if (value != Store[index])
				{
					Store[index] = value;
					NotifyChange();
				}
			}
		}

		private Word[] Store;

		private Action ChangeHandler;

		public int Size
		{
			get { return RegisterIndex.MaxValue + 1; }
		}

		public void Reset()
		{
			Store = new Word[Size];
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
