using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization
{
	public class Host
	{
		public Host(ICpu processor)
		{
			Cpu = processor;
		}

		private ICpu Cpu;

		private Task Worker;

		public bool IsRunning { get; private set; }

		public ulong ExecutedClockCycles { get; private set; }

		private object WorkerSynchronization = new object();

		public void Start()
		{
			IsRunning = true;
			StartBackgroundProcessing();
		}

		public async Task HardResetAsync()
		{
			IsRunning = false;

			//wait until processing stopped
			await Worker;

			//reset cpu
			Cpu.Reset();
		}

		public async Task SuspendAsync()
		{
			IsRunning = false;
			await Worker;
		}

		public void Resume()
		{
			IsRunning = true;
			StartBackgroundProcessing();
		}

		public event EventHandler<ExecutionCompletedEventArgs> ExecutionCompleted;

		public event EventHandler<ClockCycleScheduledEventArgs> ClockCycleScheduled;

		private void StartBackgroundProcessing()
		{
			lock (WorkerSynchronization)
			{
				if (Worker == null)
				{
					Worker = Task.Run((Action)Run);
				}
			}
		}

		private void Run()
		{
			while (IsRunning)
			{
				NotifyClockCycleScheduled();
				try
				{
					Cpu.ClockCycle();
					ExecutedClockCycles++;
				}
				catch (CpuException e)
				{
					IsRunning = false;
					NotifyExecutionCompleted(e);
					break;
				}
			}
		}

		private void NotifyExecutionCompleted(Exception reason)
		{
			if (ExecutionCompleted != null)
			{
				ExecutionCompleted(this, new ExecutionCompletedEventArgs(reason));
			}
		}

		private void NotifyClockCycleScheduled()
		{
			if (ClockCycleScheduled != null)
			{
				ClockCycleScheduled(this, new ClockCycleScheduledEventArgs(Cpu.ProgramCounter));
			}
		}
	}
}
