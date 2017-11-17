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

			OperationRegistry = new Dictionary<byte, Operations.IOperationUnit>();
			//memory access
			OperationRegistry.Add(Operations.LoadOperationUnit.OpCode, new Operations.LoadOperationUnit(DataMemory, _Register));
			OperationRegistry.Add(Operations.LoadIOperationUnit.OpCode, new Operations.LoadIOperationUnit(DataMemory, _Register));
			OperationRegistry.Add(Operations.StoreValueOperationUnit.OpCode, new Operations.StoreValueOperationUnit(DataMemory, _Register));
			OperationRegistry.Add(Operations.StoreValueIOperationUnit.OpCode, new Operations.StoreValueIOperationUnit(DataMemory, _Register));
			//register move
			OperationRegistry.Add(Operations.MoveOperationUnit.OpCode, new Operations.MoveOperationUnit(_Register));
			//synchronization
			OperationRegistry.Add(Operations.SpecialMemoryOperationUnit.OpCode, new Operations.SpecialMemoryOperationUnit(this, _Register));
			//scalar arithmetic
			OperationRegistry.Add(Operations.AddOperationUnit.OpCode, new Operations.AddOperationUnit(_Register));
			OperationRegistry.Add(Operations.AddIOperationUnit.OpCode, new Operations.AddIOperationUnit(_Register));
			OperationRegistry.Add(Operations.AddUOperationUnit.OpCode, new Operations.AddUOperationUnit(_Register));
			OperationRegistry.Add(Operations.AddIUOperationUnit.OpCode, new Operations.AddIUOperationUnit(_Register));
			OperationRegistry.Add(Operations.SubOperationUnit.OpCode, new Operations.SubOperationUnit(_Register));
			OperationRegistry.Add(Operations.SubIOperationUnit.OpCode, new Operations.SubIOperationUnit(_Register));
			OperationRegistry.Add(Operations.SubUOperationUnit.OpCode, new Operations.SubUOperationUnit(_Register));
			OperationRegistry.Add(Operations.SubIUOperationUnit.OpCode, new Operations.SubIUOperationUnit(_Register));
			OperationRegistry.Add(Operations.MultiplyOperationUnit.OpCode, new Operations.MultiplyOperationUnit(_Register));
			OperationRegistry.Add(Operations.MultiplyIOperationUnit.OpCode, new Operations.MultiplyIOperationUnit(_Register));
			OperationRegistry.Add(Operations.MultiplyUOperationUnit.OpCode, new Operations.MultiplyUOperationUnit(_Register));
			OperationRegistry.Add(Operations.MultiplyIUOperationUnit.OpCode, new Operations.MultiplyIUOperationUnit(_Register));
			OperationRegistry.Add(Operations.DivideOperationUnit.OpCode, new Operations.DivideOperationUnit(_Register));
			OperationRegistry.Add(Operations.DivideIOperationUnit.OpCode, new Operations.DivideIOperationUnit(_Register));
			OperationRegistry.Add(Operations.DivideUOperationUnit.OpCode, new Operations.DivideUOperationUnit(_Register));
			OperationRegistry.Add(Operations.DivideIUOperationUnit.OpCode, new Operations.DivideIUOperationUnit(_Register));
			//comparison
			OperationRegistry.Add(Operations.SetLessThanOperationUnit.OpCode, new Operations.SetLessThanOperationUnit(_Register));
			OperationRegistry.Add(Operations.SetLessThanUOperationUnit.OpCode, new Operations.SetLessThanUOperationUnit(_Register));
			OperationRegistry.Add(Operations.SetLessThanEqualOperationUnit.OpCode, new Operations.SetLessThanEqualOperationUnit(_Register));
			OperationRegistry.Add(Operations.SetLessThanEqualUOperationUnit.OpCode, new Operations.SetLessThanEqualUOperationUnit(_Register));
			OperationRegistry.Add(Operations.SetEqualOperationUnit.OpCode, new Operations.SetEqualOperationUnit(_Register));
			OperationRegistry.Add(Operations.SetNotEqualOperationUnit.OpCode, new Operations.SetNotEqualOperationUnit(_Register));
			//bitwise logic
			OperationRegistry.Add(Operations.LeftShiftOperationUnit.OpCode, new Operations.LeftShiftOperationUnit(_Register));
			OperationRegistry.Add(Operations.BitwiseAndOperationUnit.OpCode, new Operations.BitwiseAndOperationUnit(_Register));
			OperationRegistry.Add(Operations.BitwiseOrOperationUnit.OpCode, new Operations.BitwiseOrOperationUnit(_Register));
			OperationRegistry.Add(Operations.BitwiseXorOperationUnit.OpCode, new Operations.BitwiseXorOperationUnit(_Register));
			OperationRegistry.Add(Operations.BitwiseNotOperationUnit.OpCode, new Operations.BitwiseNotOperationUnit(_Register));
			//Jump
			OperationRegistry.Add(Operations.SimpleJumpOperationUnit.OpCode, new Operations.SimpleJumpOperationUnit());
			OperationRegistry.Add(Operations.BranchNotZeroOperationUnit.OpCode, new Operations.BranchNotZeroOperationUnit(_Register));
			OperationRegistry.Add(Operations.BranchZeroOperationUnit.OpCode, new Operations.BranchZeroOperationUnit(_Register));
			OperationRegistry.Add(Operations.AdvancedJumpOperationUnit.OpCode, new Operations.AdvancedJumpOperationUnit(_Register));

		}


		/// <summary>
		/// Number representing the state generation. This value will change during a call to <see cref="ClockCycle()"/> if and only if changes to state (ignoring the <see cref="ProgramCounter"/>) occurred.
		/// </summary>
		/// <remarks>
		/// This value may be used by debuggers to identify infinite loops.
		/// Note that only state changes initiated by this CPU are considered, externally managed memory changes and other CPUs within the same Host are not considered.
		/// </remarks>
		public ulong StateGeneration { get; private set; }

		public virtual void ClockCycle()
		{
			StateChanged = false;

			InstructionFetch();
			InstructionDecode();
			Execute();
			MemoryAccess();
			WriteBack();

			if (StateChanged)
			{
				StateGeneration++;
			}
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

		private bool StateChanged = false;

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
				StateChanged = true;
			}
		}

		protected virtual void WriteBack()
		{
			for (int i = 0; i < OperationUnit.UpdatedRegisters.Length; i++)
			{
				if (OperationUnit.UpdatedRegisters[i] != null)
				{
					Register.Scalar[(RegisterIndex)i] = OperationUnit.UpdatedRegisters[i].Value;
					StateChanged = true;
				}
			}
			for (int i = 0; i < OperationUnit.UpdatedVRegisters.Length; i++)
			{
				if (OperationUnit.UpdatedVRegisters[i] != null)
				{
					Register.Vector[(RegisterIndex)i] = OperationUnit.UpdatedVRegisters[i];
					StateChanged = true;
				}
			}
			ProgramCounter += OperationUnit.ProgramCounterChange;
		}


		public void Synchronize()
		{
			//we treat program memory as read-only => no flush required
			DataMemory.Flush();
		}
	}
}
