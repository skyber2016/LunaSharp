using LunaSharp.LunaSharpKernel.Offsets;
using System;
using System.Diagnostics;

namespace LunaSharpKernel.Misc
{

    internal class Signatures
    {
        #region Fields

        internal static IntPtr BaseAddress = new(0x400000);
        internal static IntPtr LocalPlayer = new((int)PlayerOffsets.BaseAddress);
        internal static IntPtr ModuleHandle { get; set; }

        #endregion
        internal static void LoadSignatures()
        {
            ModuleHandle = Process.GetCurrentProcess().Handle;
        }
    }
}
