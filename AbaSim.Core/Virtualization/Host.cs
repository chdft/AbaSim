using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization
{
	public class Host
	{
		private const int RunningContinuously = -1;

		public Host(ICpu processor)
		{
			Cpu = processor;
		}

		public Task WorkerTask
		{
			get { return Worker; }
		}

		public bool IsRunning
		{
			get
			{
				return _RemainingCycles > 0 || _RemainingCycles == RunningContinuously;
			}
		}

		/// <summary>
		/// Positive values represent the count of clock cycles which will be scheduled before automatically suspend execution.
		/// Negative values indicate, that no cycle based auto-suspending behavior is currently active.
		/// </summary>
		/// <seealso cref="IsRunning"/>
		public int RemainingCycles
		{
			get { return _RemainingCycles; }
		}
		private volatile int _RemainingCycles;

		public ulong ExecutedClockCycles { get; private set; }

		private object WorkerSynchronization = new object();

		private ICpu Cpu;

		private Task Worker;

		public void Start()
		{
			if (IsRunning) { throw new InvalidOperationException("The host can not be started if it is already running."); }

			_RemainingCycles = RunningContinuously;
			StartBackgroundProcessing();
		}

		public void Step(int cycleCount)
		{
			if (IsRunning) { throw new InvalidOperationException("The host can not be started in stepping mode if it is already running."); }

			_RemainingCycles = cycleCount;
			StartBackgroundProcessing();
		}

		public async Task HardResetAsync()
		{
			SetCycleCounter(0);

			//wait until processing stopped
			await Worker;

			//reset cpu
			Cpu.Reset();
		}

		/// <summary>
		/// Suspends execution.
		/// </summary>
		/// <returns>A Task which completes when the last already schedules cycle has completed.</returns>
		public async Task SuspendAsync()
		{
			SetCycleCounter(0);
			await Worker;
		}

		public event EventHandler<ExecutionCompletedEventArgs> ExecutionCompleted;

		public event EventHandler<ClockCycleScheduledEventArgs> ClockCycleScheduled;

		private void DecrementCycleCounter()
		{
			lock (WorkerSynchronization)
			{
				if (_RemainingCycles > 0)
				{
					_RemainingCycles--;
				}
			}
		}

		private void SetCycleCounter(int remainingCycls)
		{
			lock (WorkerSynchronization)
			{
				_RemainingCycles = remainingCycls;
			}
		}

		private void StartBackgroundProcessing()
		{
			lock (WorkerSynchronization)
			{
				if (Worker == null)
				{
					Worker = Task.Run(() => Run(Cpu));
				}
			}
		}

		private void Run(ICpu cpu)
		{
			//we intentionally use unsynchronized access for performance (this does no cause a race condition)
			while (IsRunning)
			{
				NotifyClockCycleScheduled(cpu);
				try
				{
					cpu.ClockCycle();
					ExecutedClockCycles++;
				}
				catch (CpuException e)
				{
					SetCycleCounter(0);
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

		private void NotifyClockCycleScheduled(ICpu cpu)
		{
			if (ClockCycleScheduled != null)
			{
				ClockCycleScheduled(this, new ClockCycleScheduledEventArgs(cpu));
			}
		}
	}
}
