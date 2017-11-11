using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16
{
	class RegisterGroup : IRegisterGroup, IReadOnlyRegisterGroup
	{
		public IRegisterBank<Word> Scalar
		{
			get { return _Scalar; }
		}
		IReadOnlyRegisterBank<Word> IReadOnlyRegisterGroup.Scalar
		{
			get { return _Scalar; }
		}
		private RegisterBank<Word> _Scalar = new RegisterBank<Word>();

		public IRegisterBank<Vector> Vector
		{
			get { return _Vector; }
		}
		IReadOnlyRegisterBank<Vector> IReadOnlyRegisterGroup.Vector
		{
			get { return _Vector; }
		}
		private RegisterBank<Vector> _Vector = new RegisterBank<Vector>();

		public Word Overflow { get; set; }

		public Word LoadLink { get; set; }

		public Word VectorLength { get; set; }

		public Word VectorMask { get; set; }

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
