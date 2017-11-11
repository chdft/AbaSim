using System;
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
		static SerialAbacus16Cpu cpu;

		static void Main(string[] args)
		{
			string programMemoryFile = args[0];
			string dataMemoryFile = args[1];

			dataMemory = new BufferMemory16(System.IO.File.ReadAllBytes(dataMemoryFile));

			cpu = new SerialAbacus16Cpu(
				new BufferMemory16(System.IO.File.ReadAllBytes(programMemoryFile)),
				dataMemory);

			Host virtualSystem = new Host(cpu);
			virtualSystem.ExecutionCompleted += virtualSystem_ExecutionCompleted;
			virtualSystem.Start();

			Console.WriteLine("Press ENTER to cancel execution");
			Console.ReadLine();
		}

		static void virtualSystem_ExecutionCompleted(object sender, ExecutionCompletedEventArgs e)
		{
			Console.WriteLine("Execution ended due to \"{0}\".", e.Reason.Message);

			Console.WriteLine("Register Dump:");
			Console.WriteLine("PC:    udec{0,5:D} | uhex{0,4:X}", cpu.ProgramCounter);
			Console.WriteLine("ovflw: udec{0,5:D} | uhex{0,4:X}", cpu.Register.Overflow.UnsignedValue);
			Console.WriteLine("ll:    udec{0,5:D} | uhex{0,4:X}", cpu.Register.LoadLink.UnsignedValue);
			for (int i = 0; i < cpu.Register.Scalar.Size; i++)
			{
				Console.WriteLine("S${0:D}: sdec{1,6:D} | shex{1,5:X} | udec{2,5:D} | uhex{2,4:X} | {3}", i, cpu.Register.Scalar[(RegisterIndex)i].SignedValue, cpu.Register.Scalar[(RegisterIndex)i].UnsignedValue, Encoding.ASCII.GetString(cpu.Register.Scalar[(RegisterIndex)i].RawValue));
			}

			Console.WriteLine("Data Memory dump:");
			for (int address = 0; address < dataMemory.Size; address++)
			{
				Console.WriteLine("{0,5:D}: sdec{1,6:D} | shex{1,5:X} | udec{2,5:D} | uhex{2,4:X} | {3}", address, dataMemory[address].SignedValue, dataMemory[address].UnsignedValue, Encoding.ASCII.GetString(dataMemory[address].RawValue));
			}
		}
	}
}
