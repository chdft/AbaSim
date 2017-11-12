using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16.Operations
{
	abstract class OperationUnit : IOperationUnit
	{
		protected const int OpCodeSize = 6;

		public OperationUnit()
		{
			Reset();
		}

		public virtual void Decode(Word instruction)
		{
			Instruction = instruction;
		}

		public virtual void Reset()
		{
			_UpdatedRegisters = new Word?[8];
			_UpdatedVRegisters = new Vector[8];
			_UpdateMemoryAddress = null;
			ProgramCounterChange = 1;
		}

		public void Execute()
		{
			InternalExecute();
		}

		public Word?[] UpdatedRegisters
		{
			get { return _UpdatedRegisters; }
		}
		private Word?[] _UpdatedRegisters;

		public Vector[] UpdatedVRegisters
		{
			get { return _UpdatedVRegisters; }
		}
		private Vector[] _UpdatedVRegisters;

		public int? UpdateMemoryAddress
		{
			get { return _UpdateMemoryAddress; }
		}
		private int? _UpdateMemoryAddress;

		public Word UpdateMemoryValue
		{
			get { return _UpdateMemoryValue; }
		}
		private Word _UpdateMemoryValue;

		protected Word Instruction { get; private set; }

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

		public int ProgramCounterChange
		{
			get;
			protected set;
		}
	}
}
