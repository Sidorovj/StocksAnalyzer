using System.Windows.Forms;
using StocksAnalyzer.Core.Interfaces;

namespace StocksAnalyzer.WinForms
{
	class FormsTextReporter : IReportText
	{
		private readonly Control m_label;
		public FormsTextReporter(Control pb)
		{
			m_label = pb;
		}

		public string Text
		{
			get => m_label.Text;
			set
			{
				if (m_label.InvokeRequired)
					m_label.BeginInvoke((MethodInvoker)delegate { m_label.Text = value; });
				else
				{
					m_label.Text = value;
				}
			}
		}

	}
}
