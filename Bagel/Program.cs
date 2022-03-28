using System;

namespace Bagel
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new Bagel())
                game.Run();
        }
    }
}
