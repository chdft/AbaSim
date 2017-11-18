using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16
{
	public class InstructionPendingEventArgs : EventArgs
	{
		public InstructionPendingEventArgs(Operations.IOperationUnit operationUnit, Word instruction, int programCounter, SerialAbacus16Cpu cpu)
		{
			OperationUnit = operationUnit;
			Instruction = instruction;
			ProgramCounter = programCounter;
			Cpu = cpu;
		}

		public Operations.IOperationUnit OperationUnit { get; private set; }

		public Word Instruction { get; private set; }

		public int ProgramCounter { get; private set; }

		public SerialAbacus16Cpu Cpu { get; private set; }
	}
}
