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

		private State HostState;

		public void Start()
		{
			HostState = State.Running;
			StartBackgroundProcessing();
		}

		public async Task HardResetAsync()
		{
			HostState = State.Stopped;

			//wait until processing stopped
			await Worker;

			//reset cpu
			Cpu.Reset();
		}

		public async Task SuspendAsync()
		{
			HostState = State.Stopped;
			await Worker;
		}

		public void Resume()
		{
			HostState = State.Running;
			StartBackgroundProcessing();
		}

		public event EventHandler<ExecutionCompletedEventArgs> ExecutionCompleted;

		private void StartBackgroundProcessing()
		{
			lock (Worker)
			{
				if (Worker == null)
				{
					Worker = Task.Run((Action)Run);
				}
			}
		}

		private void Run()
		{
			while (HostState == State.Running)
			{
				try
				{
					Cpu.ClockCycle();
				}
				catch (CpuException e)
				{
					HostState = State.Stopped;
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

		public enum State
		{
			Stopped,
			Running
		}
	}
}
