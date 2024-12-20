using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Sender
{
    internal class SenderWallpaper
    {
        private const int SPI_GETDESKWALLPAPER = 0x0073;
        private const int MAX_PATH = 260;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SystemParametersInfo(int uAction, int uParam, StringBuilder lpvParam, int fuWinIni);

        // Gets the exact path to the current wallpaper
        public static string GetCurrentDesktopBackground()
        {
            StringBuilder wallpaper = new StringBuilder(MAX_PATH);
            SystemParametersInfo(SPI_GETDESKWALLPAPER, wallpaper.Capacity, wallpaper, 0);
            return wallpaper.ToString();
        }
    }
}
