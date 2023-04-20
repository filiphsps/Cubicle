#if _WINDOWS && DEBUG
using System.Runtime.InteropServices;
#endif

namespace Cubicle {
    class Program {

        public static void Main() {
#if _WINDOWS && DEBUG
            [DllImport("kernel32.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            static extern bool AllocConsole();

            AllocConsole();
#endif

            using (Cubicle game = new Cubicle()) {
                game.Run();
            }
        }
    }
}
