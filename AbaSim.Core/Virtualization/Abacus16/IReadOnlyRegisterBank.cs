using System;
namespace AbaSim.Core.Virtualization.Abacus16
{
	public interface IReadOnlyRegisterBank<T>
	{
		T this[RegisterIndex index] { get; }
	}
}
