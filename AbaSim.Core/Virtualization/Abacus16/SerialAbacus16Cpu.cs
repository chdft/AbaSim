﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16
{
	public class SerialAbacus16Cpu : ICpu
	{
		private const int InstructionLength = 6;

		public SerialAbacus16Cpu(IMemoryProvider<Word> programMemory, IMemoryProvider<Word> dataMemory)
		{
			ProgramMemory = programMemory;
			DataMemory = dataMemory;

			OperationRegistry = new Dictionary<byte, Operations.IOperationUnit>()
			{
				{Operations.LoadOperationUnit.OpCode, new Operations.LoadOperationUnit(DataMemory, Register)}
			};
		}

		public virtual void ClockCycle()
		{
			InstructionFetch();
			InstructionDecode();
			Execute();
			MemoryAccess();
			WriteBack();
		}

		public void Reset()
		{
			ProgramCounter = 0;
		}

		protected Dictionary<byte, Operations.IOperationUnit> OperationRegistry;

		protected Word CurrentInstruction;

		protected Word[] Register = new Word[8];

		protected Vector[] VRegister = new Vector[8];

		protected IMemoryProvider<Word> DataMemory;

		protected IMemoryProvider<Word> ProgramMemory;

		protected int ProgramCounter;

		private Operations.IOperationUnit OperationUnit;

		protected virtual void InstructionFetch()
		{
			CurrentInstruction = ProgramMemory[ProgramCounter];
		}

		protected virtual void InstructionDecode()
		{
			var opCode = (byte)(CurrentInstruction.UnsignedValue >> sizeof(ushort) - InstructionLength);
			Operations.IOperationUnit unit;
			if (OperationRegistry.TryGetValue(opCode, out unit))
			{
				unit.Reset();

				unit.Decode(CurrentInstruction);

				OperationUnit = unit;
			}
			else
			{
				throw new UnknownInstructionException();
			}
		}

		protected virtual void Execute()
		{
			OperationUnit.Execute();
		}

		protected virtual void MemoryAccess()
		{
			if (OperationUnit.UpdateMemoryAddress != null)
			{
				DataMemory[OperationUnit.UpdateMemoryAddress.Value] = OperationUnit.UpdateMemoryValue;
			}
		}

		protected virtual void WriteBack()
		{
			for (int i = 0; i < OperationUnit.UpdatedRegisters.Length; i++)
			{
				if (OperationUnit.UpdatedRegisters[i] != null)
				{
					Register[i] = OperationUnit.UpdatedRegisters[i].Value;
				}
			}
			for (int i = 0; i < OperationUnit.UpdatedVRegisters.Length; i++)
			{
				if (OperationUnit.UpdatedVRegisters[i] != null)
				{
					VRegister[i] = OperationUnit.UpdatedVRegisters[i];
				}
			}
		}
	}
}