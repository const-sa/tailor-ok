using System.Management;

namespace SewingSystem.Classes
{
    class ClsHardDriveSerial
    {
        public static string HDDSerieal()
        {
            short i = 0, j = 0;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");

            foreach (ManagementObject wmi_HD in searcher.Get())
                if (wmi_HD["Name"].ToString() == @"\\.\PHYSICALDRIVE0") break;
                else i++;
            searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMedia");
            foreach (ManagementObject wmi_HD in searcher.Get())
                if (j == i) return wmi_HD["SerialNumber"].ToString();
                else j++;
            searcher.Dispose();
            return null;
        }
    }
}
