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

		static void Main(string[] args)
		{
			string programMemoryFile = args[1];
			string dataMemoryFile = args[2];

			SerialAbacus16Cpu cpu = new SerialAbacus16Cpu(
				new BufferMemory16(System.IO.File.ReadAllBytes(programMemoryFile)),
				new BufferMemory16(System.IO.File.ReadAllBytes(dataMemoryFile)));

			Host virtualSystem = new Host(cpu);
			virtualSystem.ExecutionCompleted += virtualSystem_ExecutionCompleted;
			virtualSystem.Start();

			Console.WriteLine("Press any key to cancel execution");
		}

		static void virtualSystem_ExecutionCompleted(object sender, ExecutionCompletedEventArgs e)
		{
			Console.WriteLine("Execution ended due to {0}.", e);
			Console.WriteLine("Memory dump:");
			for (int address = 0; address < dataMemory.Size; address++)
			{
				Console.WriteLine("{0,5:D}: sdec{1,7:D} | shex{1,5:X} | udec{2,7:D} | uhex{2,7:X}", address, dataMemory[address].SignedValue, dataMemory[address].UnsignedValue);
			}
		}
	}
}
