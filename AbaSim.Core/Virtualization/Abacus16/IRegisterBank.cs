using System;
namespace AbaSim.Core.Virtualization.Abacus16
{
	public interface IRegisterBank<T>
	{
		void Reset();
		T this[RegisterIndex index] { get; set; }
		int Size { get; }
	}
}
