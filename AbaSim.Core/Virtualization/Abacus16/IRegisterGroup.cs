using System;
namespace AbaSim.Core.Virtualization.Abacus16
{
	public interface IRegisterGroup
	{
		Word LoadLink { get; set; }
		Word Overflow { get; set; }
		void Reset();
		IRegisterBank<Word> Scalar { get; }
		IRegisterBank<Vector> Vector { get; }
		Word VectorLength { get; set; }
		Word VectorMask { get; set; }
	}
}
