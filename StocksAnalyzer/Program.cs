using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StocksAnalyzer
{

    static class Program
    {
        static public Form1 myForm;
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            MainDel del = () =>
            {
                MainClass.Initialize();
            };            
            del.BeginInvoke(null, null);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run((myForm = new Form1()));
        }
    }
}
