using System;
using System.Threading;
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
            new Thread(MainClass.Initialize).Start();            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(MyForm = new MainForm());
        }
    }
}
