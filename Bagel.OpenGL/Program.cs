using System;

namespace Bagel.OpenGL {
    public static class Program {
        [STAThread]
        private static void Main() {
            using var game = new Bagel();
            game.Run();
        }
    }
}