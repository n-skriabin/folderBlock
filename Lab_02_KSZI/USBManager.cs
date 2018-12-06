using System;
using System.Windows;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Lab_02_KSZI
{
    public static class USBManager
    {
        public static List<USBDeviceInfo> GetListUsbDevicesId()
        {
            var usbDevices = GetUSBDevices()./*Select(x=>x.DeviceID).*/ToList();

            return usbDevices;
            //foreach (var usbDevice in usbDevices)
            //{
            //    Console.WriteLine("Device ID: {0}, PNP Device ID: {1}, Description: {2}",
            //        usbDevice.DeviceID, usbDevice.PnpDeviceID, usbDevice.Description);
            //}

            //Console.Read();
        }

        public static List<USBDeviceInfo> GetUSBDevices()
        {

            IEnumerable<string> usbDrivesLetters = from drive in new ManagementObjectSearcher("select * from Win32_DiskDrive WHERE InterfaceType='USB'").Get().Cast<ManagementObject>()
                                                   from o in drive.GetRelated("Win32_DiskPartition").Cast<ManagementObject>()
                                                   from i in o.GetRelated("Win32_LogicalDisk").Cast<ManagementObject>()
                                                   select string.Format("{0}\\", i["Name"]);

            var test = from drive in DriveInfo.GetDrives()
                   where drive.DriveType == DriveType.Removable && usbDrivesLetters.Contains(drive.RootDirectory.Name)
                   select drive;

            var devices = new List<USBDeviceInfo>();

            foreach (var item in test)
            {
                var device = new USBDeviceInfo(item.TotalSize+item.VolumeLabel+item.DriveFormat, item.Name);
                devices.Add(device);
            }

            return devices;
        }

        public static void Lock(string folderPath)
        {
            //var folderPath = @"C:\Users\Nick\Desktop\testFolder";
            string adminUserName = Environment.UserName;// getting your adminUserName
            DirectorySecurity ds = Directory.GetAccessControl(folderPath);
            FileSystemAccessRule fsa = new FileSystemAccessRule(adminUserName, FileSystemRights.FullControl, AccessControlType.Deny);

            ds.AddAccessRule(fsa);
            Directory.SetAccessControl(folderPath, ds);
        }

        public static void Unlock(string folderPath)
        {
            //var folderPath = @"C:\Users\Nick\Desktop\testFolder";
            string adminUserName = Environment.UserName;// getting your adminUserName
            DirectorySecurity ds = Directory.GetAccessControl(folderPath);
            FileSystemAccessRule fsa = new FileSystemAccessRule(adminUserName, FileSystemRights.FullControl, AccessControlType.Deny);

            ds.RemoveAccessRule(fsa);
            Directory.SetAccessControl(folderPath, ds);
        }        
    }

    public class USBDeviceInfo
    {
        public USBDeviceInfo(string deviceID, string description)
        {
            this.DeviceID = deviceID;
            this.Description = description;
        }
        public string DeviceID { get; private set; }
        public string Description { get; private set; }
    }
}
