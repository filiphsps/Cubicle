using System;

namespace Cubicle {
    class Program {
#if USE_GEARSET
        [STAThread]
#endif
        public static void Main() {
            using (var game = new Cubicle())
                game.Run();

#if USE_GEARSET
            Environment.Exit(0);
#endif
        }
    }
}
