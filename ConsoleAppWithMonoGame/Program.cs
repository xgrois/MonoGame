using System;

namespace ConsoleAppWithMonoGame
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            using (DummyGame game = new DummyGame())
            {
                game.Run();
            }
        }
    }
}
