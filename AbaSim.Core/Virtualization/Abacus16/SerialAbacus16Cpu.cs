using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16
{
	public class SerialAbacus16Cpu : ICpu
	{
		public static readonly int MaximumAddressableMemory = Word.UnsignedMaxValue.UnsignedValue;

		private const int InstructionLength = 6;

		public SerialAbacus16Cpu(IMemoryProvider<Word> programMemory, IMemoryProvider<Word> dataMemory)
		{
			ProgramMemory = programMemory;
			DataMemory = dataMemory;

			//Initialize Operation Units
			OperationRegistry = new Dictionary<byte, Operations.IOperationUnit>();
			//memory access
			OperationRegistry.Add(Operations.LoadOperationUnit.OpCode, new Operations.LoadOperationUnit(DataMemory, _Register));
			OperationRegistry.Add(Operations.LoadIOperationUnit.OpCode, new Operations.LoadIOperationUnit(DataMemory, _Register));
			OperationRegistry.Add(Operations.StoreValueOperationUnit.OpCode, new Operations.StoreValueOperationUnit(_Register));
			OperationRegistry.Add(Operations.StoreValueIOperationUnit.OpCode, new Operations.StoreValueIOperationUnit(_Register));
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

			NotifyInstructionPending();

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

		public event EventHandler<InstructionPendingEventArgs> InstructionPending;

		protected IMemoryProvider<Word> DataMemory;

		protected IMemoryProvider<Word> ProgramMemory;

		public int ProgramCounter { get; protected set; }

		private Operations.IOperationUnit OperationUnit;

		private bool StateChanged = false;

		private ulong LastRegisterStateGeneration = 0;

		/// <summary>
		/// Reads the upcoming instruction based on the current <see cref="ProgramCounter"/>
		/// </summary>
		protected virtual void InstructionFetch()
		{
			if (ProgramCounter >= ProgramMemory.Size)
			{
				throw new ProgramCounterOutOfBoundsException("The address for the next CPU instruction is out of bounds of the program memory.", ProgramCounter);
			}
			try
			{
				CurrentInstruction = ProgramMemory[ProgramCounter];
			}
			catch (MemoryAccessViolationException e)
			{
				throw new ProgramCounterOutOfBoundsException("The address for the next CPU instruction is in an inaccessible area of program memory.", e, ProgramCounter);
			}
		}

		/// <summary>
		/// Parses the upcoming instruction and selects the appropriate OperationUnit from <see cref="OperationRegistry"/>
		/// Note that parameters for the executed instruction are obtained at this point in time.
		/// </summary>
		/// <seealso cref="Operations.IOperationUnit"/>
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

		/// <summary>
		/// Execute the operation as defined by the selected OperationUnit
		/// </summary>
		protected virtual void Execute()
		{
			OperationUnit.Execute();
		}

		/// <summary>
		/// Perform any required data memory access
		/// </summary>
		protected virtual void MemoryAccess()
		{
			//CHECK: accessing the memory to check if a write is needed may cause caches to misbehave (i.e. cause uneeded misses, since we are just writing and not interested in the old value)
			if (OperationUnit.UpdateMemoryAddress != null && DataMemory[OperationUnit.UpdateMemoryAddress.Value] != OperationUnit.UpdateMemoryValue)
			{
				DataMemory[OperationUnit.UpdateMemoryAddress.Value] = OperationUnit.UpdateMemoryValue;
				StateChanged = true;
			}
		}

		/// <summary>
		/// Perform any required writes to registers
		/// </summary>
		protected virtual void WriteBack()
		{
			OperationUnit.WriteRegisterChanges();
			if (!StateChanged)
			{
				StateChanged = LastRegisterStateGeneration != Register.StateGeneration;
			}
			LastRegisterStateGeneration = Register.StateGeneration;
			ProgramCounter += OperationUnit.ProgramCounterChange;
		}

		protected void NotifyInstructionPending()
		{
			if (InstructionPending != null)
			{
				InstructionPending(this, new InstructionPendingEventArgs(OperationUnit, CurrentInstruction, ProgramCounter, this));
			}
		}

		public void Synchronize()
		{
			//BUG: sync cache
			//we treat program memory as read-only => no flush required
			DataMemory.Flush();
		}
	}
}
