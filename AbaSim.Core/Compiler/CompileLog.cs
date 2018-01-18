using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Compiler
{
	public class CompileLog : IReadOnlyList<CompileLogItem>
	{
		private List<CompileLogItem> Items = new List<CompileLogItem>();

		public bool ErrorOccured { get; private set; }

		public void Error(string location, string message, string description)
		{
			ErrorOccured = true;
			Log(new CompileLogItem()
			{
				Severity = CompileLogItemSeverity.Error,
				Message = message,
				Description = description,
				Location = location
			});
		}

		public void Warning(string location, string message, string description)
		{
			Log(new CompileLogItem()
			{
				Severity = CompileLogItemSeverity.Warning,
				Message = message,
				Description = description,
				Location = location
			});
		}

		public void Information(string location, string message, string description)
		{
			Log(new CompileLogItem()
			{
				Severity = CompileLogItemSeverity.Information,
				Message = message,
				Description = description,
				Location = location
			});
		}

		public void Debug(string location, string message, string description)
		{
			Log(new CompileLogItem()
			{
				Severity = CompileLogItemSeverity.Debug,
				Message = message,
				Description = description,
				Location = location
			});
		}

		protected void Log(CompileLogItem item)
		{
			Items.Add(item);
		}

		public CompileLogItem this[int index]
		{
			get { return Items[index]; }
		}

		public int Count
		{
			get { return Items.Count; }
		}

		public IEnumerator<CompileLogItem> GetEnumerator()
		{
			return Items.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return Items.GetEnumerator();
		}
	}
}
