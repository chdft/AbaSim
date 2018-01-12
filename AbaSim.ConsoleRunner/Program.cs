using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
		static Host virtualSystem;
		static SerialAbacus16Cpu cpu;
		private const string ShowControlFlowFlag = "C";
		private const string StartPausedFlag = "P";
		private const string MonitorProgramFlowFlag = "F";
		private const string BenchmarkFlag = "B";

		static Dictionary<int, FlowInfo> FlowMonitoringMapping;

		static void Main(string[] args)
		{
			string programMemoryFile = args[0];
			string dataMemoryFile = args[1];
			bool showControlFlow = (args.Length < 3 ? false : args[2].Contains(ShowControlFlowFlag));
			bool startPaused = (args.Length < 3 ? false : args[2].Contains(StartPausedFlag));
			bool monitorProgramFlow = (args.Length < 3 ? false : args[2].Contains(MonitorProgramFlowFlag));
			bool benchmark = (args.Length < 3 ? false : args[2].Contains(BenchmarkFlag));

			programMemory = new BufferMemory16(System.IO.File.ReadAllBytes(programMemoryFile));
			dataMemory = new BufferMemory16(System.IO.File.ReadAllBytes(dataMemoryFile));

			cpu = new SerialAbacus16Cpu(
				programMemory,
				dataMemory);

			virtualSystem = new Host(cpu);
			virtualSystem.ExecutionCompleted += virtualSystem_ExecutionCompleted;
			if (showControlFlow)
			{
				cpu.InstructionPending += cpu_InstructionPending;
				//virtualSystem.ClockCycleScheduled += virtualSystem_ClockCycleScheduled;
				Console.WriteLine("Press ESC to cancel execution");
			}
			if (monitorProgramFlow)
			{
				FlowMonitoringMapping = new Dictionary<int, FlowInfo>();
			}
			if (!startPaused)
			{
				virtualSystem.Start();
			}
			if (benchmark)
			{
				System.Threading.Thread.Sleep(1000);
				virtualSystem.SuspendAsync().Wait();
				WriteDumps();
				Console.ReadLine();
				return;
			}

			while (true)
			{
				ConsoleKeyInfo input = Console.ReadKey(true);
				switch (input.Key)
				{
					case ConsoleKey.Escape:
						if (virtualSystem.IsRunning)
						{
							Console.WriteLine("Suspending Execution...");
							virtualSystem.SuspendAsync().Wait();
							WriteDumps();
							Console.WriteLine("Execution suspended. Press ENTER to exit.");
							Console.ReadLine();
						}
						return;
					case ConsoleKey.P:
						Console.WriteLine("Suspending execution...");
						virtualSystem.SuspendAsync().Wait();
						Console.WriteLine("Execution suspended. Press H for help.");
						break;
					case ConsoleKey.R:
						if (!virtualSystem.IsRunning)
						{
							virtualSystem.Start();
						}
						else
						{
							Console.WriteLine("Already running.");
						}
						break;
					case ConsoleKey.S:
						if (!virtualSystem.IsRunning)
						{
							virtualSystem.Step(1);
						}
						else
						{
							Console.WriteLine("Already running.");
						}
						break;
					case ConsoleKey.F:
						WriteFlowDump();
						break;
					case ConsoleKey.D:
						WriteDumps();
						break;
					case ConsoleKey.H:
						Console.WriteLine("ESC: exit | P: pause | S: single step | R: run | D: dump | F: flow info | H: help");
						break;
				}
			}
		}

		static void cpu_InstructionPending(object sender, InstructionPendingEventArgs e)
		{
			if (e.ProgramCounter < programMemory.Size)
			{
				Word instruction = e.Instruction;
				string binRepresentation = instruction.ToString();
				string friendlyOperationName = "███";
				var operationAttribute = e.OperationUnit.GetType().GetCustomAttributes<Core.Compiler.Parsing.AssemblyCodeAttribute>().FirstOrDefault();
				if (operationAttribute != null)
				{
					//TODO: handle fixed constants?
					friendlyOperationName = operationAttribute.FriendlyName;
				}

				if (FlowMonitoringMapping != null)
				{
					if (!FlowMonitoringMapping.ContainsKey(e.ProgramCounter))
					{
						FlowMonitoringMapping.Add(e.ProgramCounter, new FlowInfo()
						{
							CallCount = 1,
							LastCallGeneration = e.Cpu.StateGeneration
						});
					}
					else
					{
						FlowInfo info = FlowMonitoringMapping[e.ProgramCounter];
						if (info.LastCallGeneration == e.Cpu.StateGeneration)
						{
							//reached same instruction, without state change => endless loop
							Console.WriteLine("Endless loop detected at following instruction:");
							//awaiting t inside this event handler would hang
							var t = virtualSystem.SuspendAsync();
						}
						else
						{
							info.LastCallGeneration = e.Cpu.StateGeneration;
							info.CallCount++;
						}
					}
				}

				Console.WriteLine("Executing: {1,4:X} | {2} {3} @ {0,-3} | {4}", e.ProgramCounter, instruction.UnsignedValue, binRepresentation.Substring(0, 8), binRepresentation.Substring(8, 8), friendlyOperationName);
			}
			else
			{
				Console.WriteLine("Executing:  [Out of memory bounds]  @ {0,-3}", e.ProgramCounter);
			}
		}

		static void virtualSystem_ClockCycleScheduled(object sender, ClockCycleScheduledEventArgs e)
		{
			if (e.Cpu.ProgramCounter < programMemory.Size)
			{
				Word instruction = programMemory[e.Cpu.ProgramCounter];
				string binRepresentation = instruction.ToString();

				Console.WriteLine("Executing: {1,4:X} | {2} {3} @ {0,-3}", e.Cpu.ProgramCounter, instruction.UnsignedValue, binRepresentation.Substring(0, 8), binRepresentation.Substring(8, 8));
			}
			else
			{
				Console.WriteLine("Executing:  [Out of memory bounds]  @ {0,-3}", e.Cpu.ProgramCounter);
			}
		}

		static void virtualSystem_ExecutionCompleted(object sender, ExecutionCompletedEventArgs e)
		{
			Console.WriteLine("Execution ended due to \"{0}\".", e.Reason.Message);
			WriteDumps();
			Console.WriteLine("Press any key to exit.");
		}

		static void WriteDumps()
		{
			Console.WriteLine("Executed {0} Instructions.", virtualSystem.ExecutedClockCycles);
			Console.WriteLine("Register Dump:");
			Console.WriteLine("PC:    udec{0,5:D} | uhex{0,4:X}", cpu.ProgramCounter);
			Console.WriteLine("ovflw: udec{0,5:D} | uhex{0,4:X}", cpu.Register.Overflow.UnsignedValue);
			Console.WriteLine("ll:    udec{0,5:D} | uhex{0,4:X}", cpu.Register.LoadLink.UnsignedValue);
			for (int i = 0; i < cpu.Register.Scalar.Size; i++)
			{
				string binRepresentation = cpu.Register.Scalar[(RegisterIndex)i].ToString();

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
				string binRepresentation = memory[address].ToString();
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

		static void WriteFlowDump()
		{
			if (FlowMonitoringMapping != null)
			{
				foreach (var item in FlowMonitoringMapping)
				{
					Console.WriteLine("{0,5:D} executed {1} times", item.Key, item.Value.CallCount);
				}
			}
			else
			{
				Console.WriteLine("Flow monitoring is disabled.");
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

		private class FlowInfo
		{
			public int CallCount;
			public ulong LastCallGeneration;
		}
	}
}
