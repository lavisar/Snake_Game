using System;
using System.Windows.Forms;

namespace Snake
{
    static class Program
    {
        /*
         *  Main Class
         *  If the snake run to fast, set speed < 10 in -- Setting Class
         */
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            
        }
    }
}
