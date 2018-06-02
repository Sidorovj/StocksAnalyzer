using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StocksAnalyzer
{

    static class Program
    {
        static public MainForm myForm;

        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            (new Thread(()=>
            {
                MainClass.Initialize();
            })).Start();            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run((myForm = new MainForm()));
        }
    }
}
