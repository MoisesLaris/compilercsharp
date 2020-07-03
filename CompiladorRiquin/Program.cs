using System;
using Gtk;

namespace CompiladorRiquin
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                ejecutarLexico e = new ejecutarLexico();
                e.ejecutarTerminal(args[0]);
                Console.WriteLine(e.getListaTokens());
            }
            else
            {
                Application.Init();
                MainWindow win = new MainWindow();
                win.Show();
                Application.Run();
            }
        }
    }
}
