using System.Windows.Forms;
using StocksAnalyzer.Adapters;

namespace StocksAnalyzer.WinForms
{
	class FormsProgressReporter : IReportProgress
	{
		private readonly ProgressBar m_progressBar;
		public FormsProgressReporter(ProgressBar pb)
		{
			m_progressBar = pb;
		}

		public int Value
		{
			get => m_progressBar.Value;
			set
			{
				if (m_progressBar.InvokeRequired)
					m_progressBar.BeginInvoke((MethodInvoker)delegate { m_progressBar.Value = value; });
				else
				{
					m_progressBar.Value = value;
				}
			}
		}
	}
}
