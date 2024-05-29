using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace LunaSharp.Core.Utils
{
    public static class Helper
    {
        public static IntPtr[] GetVTblAddresses(IntPtr pointer, int numberOfMethods)
        {
            return GetVTblAddresses(pointer, 0, numberOfMethods);
        }

        public static IntPtr[] GetVTblAddresses(IntPtr pointer, int startIndex, int numberOfMethods)
        {
            List<IntPtr> vtblAddresses = new List<IntPtr>();

            IntPtr vTable = Marshal.ReadIntPtr(pointer);
            for (int i = startIndex; i < startIndex + numberOfMethods; i++)
                vtblAddresses.Add(Marshal.ReadIntPtr(vTable, i * IntPtr.Size));

            return vtblAddresses.ToArray();
        }
    }
}
