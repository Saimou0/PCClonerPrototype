using System.Runtime.InteropServices;

namespace PCClonerPrototype
{
    internal static class Program
    {
        //[DllImport("kernel32.dll", SetLastError = true)]
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool AllocConsole();

        public static Dictionary<string, bool> fileSelection = [];
        public static string selectedFolderPath = "";
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            AllocConsole();
            Application.Run(new MainForm());
        }
    }
}