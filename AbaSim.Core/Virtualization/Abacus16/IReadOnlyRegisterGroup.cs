using System;
namespace AbaSim.Core.Virtualization.Abacus16
{
	public interface IReadOnlyRegisterGroup
	{
		Word LoadLink { get; }
		Word Overflow { get; }
		IReadOnlyRegisterBank<Word> Scalar { get; }
		IReadOnlyRegisterBank<Vector> Vector { get; }
		Word VectorLength { get; }
		Word VectorMask { get; }
		ulong StateGeneration { get; }
	}
}
