using System;
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
        public static List<string> GetListUsbDevicesId()
        {
            var usbDevices = GetUSBDevices().Select(x=>x.DeviceID).ToList();

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
            List<USBDeviceInfo> devices = new List<USBDeviceInfo>();

            ManagementObjectCollection collection;
            using (var searcher = new ManagementObjectSearcher(@"Select * From Win32_USBHub"))
                collection = searcher.Get();

            foreach (var device in collection)
            {
                devices.Add(new USBDeviceInfo(
                (string)device.GetPropertyValue("DeviceID"),
                (string)device.GetPropertyValue("PNPDeviceID"),
                (string)device.GetPropertyValue("Description")
                ));
            }

            collection.Dispose();
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
        public USBDeviceInfo(string deviceID, string pnpDeviceID, string description)
        {
            this.DeviceID = deviceID;
            this.PnpDeviceID = pnpDeviceID;
            this.Description = description;
        }
        public string DeviceID { get; private set; }
        public string PnpDeviceID { get; private set; }
        public string Description { get; private set; }
    }
}
