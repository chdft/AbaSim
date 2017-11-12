﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AbaSim.Core.Virtualization;
using AbaSim.Core.Virtualization.Abacus16;

namespace AbaSim.ConsoleRunner
{
	class Program
	{
		static IMemoryProvider<Word> dataMemory;
		static IMemoryProvider<Word> programMemory;
		static SerialAbacus16Cpu cpu;

		static void Main(string[] args)
		{
			string programMemoryFile = args[0];
			string dataMemoryFile = args[1];

			programMemory = new BufferMemory16(System.IO.File.ReadAllBytes(programMemoryFile));
			dataMemory = new BufferMemory16(System.IO.File.ReadAllBytes(dataMemoryFile));

			cpu = new SerialAbacus16Cpu(
				programMemory,
				dataMemory);

			Host virtualSystem = new Host(cpu);
			virtualSystem.ExecutionCompleted += virtualSystem_ExecutionCompleted;
			virtualSystem.Start();

			Console.WriteLine("Press ENTER to cancel execution");
			Console.ReadLine();
			if (virtualSystem.IsRunning)
			{
				virtualSystem.SuspendAsync().Wait();
				WriteDumps();
			}
		}

		static void virtualSystem_ExecutionCompleted(object sender, ExecutionCompletedEventArgs e)
		{
			Console.WriteLine("Execution ended due to \"{0}\".", e.Reason.Message);
			WriteDumps();
		}

		static void WriteDumps()
		{
			Console.WriteLine("Register Dump:");
			Console.WriteLine("PC:    udec{0,5:D} | uhex{0,4:X}", cpu.ProgramCounter);
			Console.WriteLine("ovflw: udec{0,5:D} | uhex{0,4:X}", cpu.Register.Overflow.UnsignedValue);
			Console.WriteLine("ll:    udec{0,5:D} | uhex{0,4:X}", cpu.Register.LoadLink.UnsignedValue);
			for (int i = 0; i < cpu.Register.Scalar.Size; i++)
			{
				string binRepresentation = Convert.ToString(cpu.Register.Scalar[(RegisterIndex)i].SignedValue, 2).PadLeft(16, '0');

				Console.WriteLine("S${0:D}: sdec{1,6:D} | shex{1,5:X} | udec{2,5:D} | uhex{2,4:X} | {3}{4} | {5} {6}",
					i,
					cpu.Register.Scalar[(RegisterIndex)i].SignedValue,
					cpu.Register.Scalar[(RegisterIndex)i].UnsignedValue,
					GetTextValue(cpu.Register.Scalar[(RegisterIndex)i].RawValue, 0),
					GetTextValue(cpu.Register.Scalar[(RegisterIndex)i].RawValue, 1),
					binRepresentation.Substring(0, 8),
					binRepresentation.Substring(8, 8));
			}

			Console.WriteLine("Data Memory dump:");
			WriteMemoryDump(dataMemory);

			Console.WriteLine("Program Memory dump:");
			WriteMemoryDump(programMemory);

		}

		static void WriteMemoryDump(IMemoryProvider<Word> memory)
		{
			for (int address = 0; address < memory.Size; address++)
			{
				string binRepresentation = Convert.ToString(memory[address].SignedValue, 2).PadLeft(16, '0');
				Console.WriteLine("{0,5:D}: sdec{1,6:D} | shex{1,5:X} | udec{2,5:D} | uhex{2,4:X} | {3}{4} | {5} {6}",
					address,
					memory[address].SignedValue,
					memory[address].UnsignedValue,
					GetTextValue(memory[address].RawValue, 0),
					GetTextValue(memory[address].RawValue, 1),
					binRepresentation.Substring(0, 8),
					binRepresentation.Substring(8, 8));
			}
		}

		static string GetTextValue(byte[] source, int index)
		{
			if (source[index] >= 32 && source[index] < 127)
			{
				return Encoding.ASCII.GetString(source, index, 1);
			}
			else
			{
				return "░";
			}
		}
	}
}