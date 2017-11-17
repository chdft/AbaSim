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
			get
			{
				Task worker = Worker;
				if (worker == null)
				{
					worker = Task.FromResult(0);
				}
				return worker;
			}
		}

		public bool IsRunning
		{
			get
			{
				return Worker != null;
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
			if (Worker != null)
			{
				await Worker;
			}

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
			if (Worker != null)
			{
				await Worker;
			}
		}

		public event EventHandler<ExecutionCompletedEventArgs> ExecutionCompleted;

		public event EventHandler<ClockCycleScheduledEventArgs> ClockCycleScheduled;

		protected bool GetSouldScheduleNewCycles()
		{
			lock (WorkerSynchronization)
			{
				return _RemainingCycles > 0 || _RemainingCycles == RunningContinuously;
			}
		}

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
					Worker = Task.Run(() =>
					{
						Run(Cpu);
						Worker = null;
					});
				}
			}
		}

		private void Run(ICpu cpu)
		{
			//we intentionally use unsynchronized access for performance (this does no cause a race condition)
			while (GetSouldScheduleNewCycles())
			{
				NotifyClockCycleScheduled(cpu);
				try
				{
					cpu.ClockCycle();
					ExecutedClockCycles++;
					DecrementCycleCounter();
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
