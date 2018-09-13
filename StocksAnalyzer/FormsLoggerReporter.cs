using System;
using System.Threading;
using System.Windows.Forms;
using StocksAnalyzer.Core.Interfaces;

namespace StocksAnalyzer.WinForms
{
	class FormsLoggerReporter : IReportText
	{
		private RichTextBox m_viewLogger;

		public string Text
		{
			get => m_viewLogger.Text;
			set
			{
				if (m_viewLogger == null)
				{
					while (Program.MyForm == null)
						Thread.Sleep(20);
					m_viewLogger = Program.MyForm.richTextBoxLog;
				}

				if (m_viewLogger.InvokeRequired)
					m_viewLogger.BeginInvoke(
						(MethodInvoker)delegate
						{
							m_viewLogger.Text = value + Environment.NewLine + m_viewLogger.Text;
						});
				else
				{
					m_viewLogger.Text = value + Environment.NewLine + m_viewLogger.Text;
				}

			}
		}

	}
}
