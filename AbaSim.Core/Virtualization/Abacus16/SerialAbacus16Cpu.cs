using System;
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
				//memory access
				{Operations.LoadOperationUnit.OpCode, new Operations.LoadOperationUnit(DataMemory, _Register)},
				{Operations.LoadIOperationUnit.OpCode, new Operations.LoadIOperationUnit(DataMemory, _Register)},
				{Operations.StoreValueOperationUnit.OpCode, new Operations.StoreValueOperationUnit(DataMemory, _Register)},
				{Operations.StoreValueIOperationUnit.OpCode, new Operations.StoreValueIOperationUnit(DataMemory, _Register)},
				//register move
				{Operations.MoveOperationUnit.OpCode, new Operations.MoveOperationUnit(_Register)},
				{Operations.SpecialMemoryOperationUnit.OpCode, new Operations.SpecialMemoryOperationUnit(this, _Register)},
				//scalar arithmetic
				{Operations.AddOperationUnit.OpCode, new Operations.AddOperationUnit(_Register)},
				{Operations.AddIOperationUnit.OpCode, new Operations.AddIOperationUnit(_Register)},
				{Operations.AddUOperationUnit.OpCode, new Operations.AddUOperationUnit(_Register)},
				{Operations.AddIUOperationUnit.OpCode, new Operations.AddIUOperationUnit(_Register)},
				{Operations.SubOperationUnit.OpCode, new Operations.SubOperationUnit(_Register)},
				{Operations.SubIOperationUnit.OpCode, new Operations.SubIOperationUnit(_Register)},
				{Operations.SubUOperationUnit.OpCode, new Operations.SubUOperationUnit(_Register)},
				{Operations.SubIUOperationUnit.OpCode, new Operations.SubIUOperationUnit(_Register)},
				//Jump
				{Operations.SimpleJumpOperationUnit.OpCode, new Operations.SimpleJumpOperationUnit()}
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

		public IRegisterGroup Register
		{
			get { return _Register; }
		}
		private RegisterGroup _Register = new RegisterGroup();

		protected IMemoryProvider<Word> DataMemory;

		protected IMemoryProvider<Word> ProgramMemory;

		public int ProgramCounter { get; protected set; }

		private Operations.IOperationUnit OperationUnit;

		protected virtual void InstructionFetch()
		{
			if (ProgramCounter >= ProgramMemory.Size)
			{
				throw new ProgramCounterOutOfBoundsException("The address for the next CPU instruction is out of bounds of the program memory.", ProgramCounter);
			}
			CurrentInstruction = ProgramMemory[ProgramCounter];
		}

		protected virtual void InstructionDecode()
		{
			var opCode = (byte)(CurrentInstruction.UnsignedValue >> Word.Size - InstructionLength);
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
					Register.Scalar[(RegisterIndex)i] = OperationUnit.UpdatedRegisters[i].Value;
				}
			}
			for (int i = 0; i < OperationUnit.UpdatedVRegisters.Length; i++)
			{
				if (OperationUnit.UpdatedVRegisters[i] != null)
				{
					Register.Vector[(RegisterIndex)i] = OperationUnit.UpdatedVRegisters[i];
				}
			}
			ProgramCounter += OperationUnit.ProgramCounterChange;
		}


		public void Synchronize()
		{
			DataMemory.Flush();
		}
	}
}
