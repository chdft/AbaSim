using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16.Operations
{
	abstract class RegisterOperationUnit : IOperationUnit
	{
		private static readonly Word DestinationRegisterMask = (short)(2 ^ 7 + 2 ^ 8 + 2 ^ 9);
		private static readonly byte DestinationRegisterShift = Word.Size - 6 - 3;
		private static readonly Word LeftRegisterMask = (short)(2 ^ 4 + 2 ^ 5 + 2 ^ 6);
		private static readonly byte LeftRegisterShift = Word.Size - 6 - 3 - 3;
		private static readonly Word RightRegisterMask = (short)(2 ^ 1 + 2 ^ 2 + 2 ^ 3);
		private static readonly byte RightRegisterShift = Word.Size - 6 - 3 - 3 - 3;
		private static readonly Word VectorBitMask = (short)(2 ^ 0);
		private static readonly byte VectorBitShift = Word.Size - 6 - 3 - 3 - 1;

		protected RegisterIndex DestinationRegister { get; private set; }

		protected RegisterIndex LeftRegister { get; private set; }

		protected RegisterIndex RightRegister { get; private set; }

		protected bool VectorBit { get; private set; }

		public void Decode(Word instruction)
		{
			DestinationRegister = (RegisterIndex)((instruction & DestinationRegisterMask) >> DestinationRegisterShift);
			LeftRegister = (RegisterIndex)((instruction & LeftRegisterMask) >> LeftRegisterShift);
			RightRegister = (RegisterIndex)((instruction & RightRegisterMask) >> RightRegisterShift);
			VectorBit = ((instruction & VectorBitMask) >> VectorBitShift) != 0;
		}

		public void Execute()
		{
			InternalExecute();
		}

		public Word?[] UpdatedRegisters
		{
			get { return _UpdatedRegisters; }
		}
		private Word?[] _UpdatedRegisters = new Word?[8];

		public Vector[] UpadtedVRegisters
		{
			get { return _UpdatedVRegisters; }
		}
		private Vector[] _UpdatedVRegisters = new Vector[8];
		
		public int? UpdateMemoryAddress
		{
			get { return _UpdateMemoryAddress; }
		}	
		private int? _UpdateMemoryAddress = null;

		public Word UpdateMemoryValue
		{
			get { return _UpdateMemoryValue; }
		}
		private Word _UpdateMemoryValue;

		protected void UpdateRegister(RegisterIndex index, Word newValue)
		{
			_UpdatedRegisters[index] = newValue;
		}

		protected void UpdateVRegister(RegisterIndex index, Vector newValue)
		{
			_UpdatedVRegisters[index] = newValue;
		}

		protected void UpdateMemory(int address, Word newValue)
		{
			_UpdateMemoryAddress = address;
			_UpdateMemoryValue = newValue;
		}

		protected abstract void InternalExecute();
	}
}
