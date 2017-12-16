using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16
{
	class RegisterGroup : IRegisterGroup, IReadOnlyRegisterGroup
	{
		public RegisterGroup()
		{
			_Scalar = new ScalarRegisterBank(() => StateGeneration++);
			_Vector = new VectorRegisterBank(() => StateGeneration++);
		}

		public IRegisterBank<Word> Scalar
		{
			get { return _Scalar; }
		}
		IReadOnlyRegisterBank<Word> IReadOnlyRegisterGroup.Scalar
		{
			get { return _Scalar; }
		}
		private ScalarRegisterBank _Scalar;

		public IRegisterBank<Vector> Vector
		{
			get { return _Vector; }
		}
		IReadOnlyRegisterBank<Vector> IReadOnlyRegisterGroup.Vector
		{
			get { return _Vector; }
		}
		private VectorRegisterBank _Vector;

		public Word Overflow { get; set; }

		public Word LoadLink { get; set; }

		public Word VectorLength { get; set; }

		public Word VectorMask { get; set; }

		public ulong StateGeneration
		{
			get;
			private set;
		}

		public void Reset()
		{
			Scalar.Reset();
			Vector.Reset();
			Overflow = Word.Empty;
			LoadLink = Word.Empty;
			VectorLength = (byte)16;
			VectorMask = ~Word.Empty;
		}
	}
}
