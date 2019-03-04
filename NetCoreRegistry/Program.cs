using System;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace NetCoreRegistry
{
    class Program
    {
        static void Main(string[] args)
        {
            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                try
                {
                    using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\edi_wang"))
                    {
                        key.SetValue("Title", $".NET Core Rocks! {DateTime.UtcNow:MM/dd/yyyy-HHmmss}");
                    }

                    using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\edi_wang"))
                    {
                        if (key != null)
                        {
                            Object o = key.GetValue("Title");
                            Console.WriteLine(o.ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
