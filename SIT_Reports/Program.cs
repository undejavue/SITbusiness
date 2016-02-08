using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SIT_Reports
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}