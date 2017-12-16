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

		public void Decode(Word instruction)
		{
			Instruction = instruction;
			InternalDecode();
		}

		public void Reset()
		{
			_UpdateMemoryAddress = null;
			ProgramCounterChange = 1;
			InternalReset();
		}

		public void Execute()
		{
			InternalExecute();
		}

		public void WriteRegisterChanges()
		{
			InternalWriteRegisterChanges();
		}

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

		[Obsolete("Write values directly when WriteRegisterChanges() is called", true)]
		protected void UpdateRegister(RegisterIndex index, Word newValue)
		{
		}

		[Obsolete("Write values directly when WriteRegisterChanges() is called", true)]
		protected void UpdateVRegister(RegisterIndex index, Vector newValue)
		{
		}

		protected void ScheduleMemoryChange(int address, Word newValue)
		{
			_UpdateMemoryAddress = address;
			_UpdateMemoryValue = newValue;
		}

		protected abstract void InternalExecute();

		protected abstract void InternalDecode();

		protected abstract void InternalReset();

		protected abstract void InternalWriteRegisterChanges();

		public int ProgramCounterChange
		{
			get;
			protected set;
		}
	}
}
