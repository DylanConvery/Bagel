using System;

namespace Bagel
{
    public static class Program
    {
        [STAThread]
        private static void Main()
        {
            using var game = new Bagel();
            game.Run();
        }
    }
}