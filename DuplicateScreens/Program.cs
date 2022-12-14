using System;
using System.Linq;
using System.Threading;
using System.Runtime.InteropServices;
using System.Management;

namespace DuplicateScreens
{
    internal class Program
    {
        // https://stackoverflow.com/questions/16326932/how-to-set-primary-monitor-for-windows-7-in-c-sharp
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern long SetDisplayConfig(uint numPathArrayElements,
        IntPtr pathArray, uint numModeArrayElements, IntPtr modeArray, uint flags);


        static UInt32 SDC_TOPOLOGY_INTERNAL = 0x00000001;
        static UInt32 SDC_TOPOLOGY_CLONE = 0x00000002;
        static UInt32 SDC_TOPOLOGY_EXTEND = 0x00000004;
        static UInt32 SDC_TOPOLOGY_EXTERNAL = 0x00000008;
        static UInt32 SDC_APPLY = 0x00000080;

        static void CloneDisplays()
        {
            SetDisplayConfig(0, IntPtr.Zero, 0, IntPtr.Zero, (SDC_APPLY | SDC_TOPOLOGY_CLONE));
        }

        static void ExtendDisplays()
        {
            SetDisplayConfig(0, IntPtr.Zero, 0, IntPtr.Zero, (SDC_APPLY | SDC_TOPOLOGY_EXTEND));
        }

        static void ExternalDisplay()
        {
            SetDisplayConfig(0, IntPtr.Zero, 0, IntPtr.Zero, (SDC_APPLY | SDC_TOPOLOGY_EXTERNAL));
        }

        static void InternalDisplay()
        {
            SetDisplayConfig(0, IntPtr.Zero, 0, IntPtr.Zero, (SDC_APPLY | SDC_TOPOLOGY_INTERNAL));
        }






        [STAThread]
        static void Main()
        {
            CloneDisplays();
            
            for(int i=0; i<300; i++)
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_PnPEntity where service=\"monitor\"");
                int numberOfMonitors = searcher.Get().Count;
                Console.WriteLine(numberOfMonitors);

                if (i == 0 && numberOfMonitors == 2)
                    break;

                if(numberOfMonitors == 2)
                {
                    CloneDisplays();
                    return;
                }

                Thread.Sleep(1 * 1000);
            }
        }

    }
}
