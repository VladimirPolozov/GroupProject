using System;
using System.Windows.Forms;

namespace TextEditor
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Form1 form = new Form1();
            FilePresenter presenter = new FilePresenter(form);
            Application.Run(form);
        }
    }
}
