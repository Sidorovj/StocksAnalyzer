using System;
using System.Windows.Forms;

namespace StocksAnalyzer
{

    static class Program
    {
        public static MainForm MyForm;

        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
	        try
	        {
		        Application.Run(MyForm = new MainForm());
	        }
	        catch (AggregateException ex)
	        {
		        Logger.Log.Fatal(ex.InnerExceptions);
	        }
	        catch (Exception ex)
	        {
				Logger.Log.Fatal(ex);
	        }
        }
    }
}
