using LunaSharp.LunaSharpKernel.Offsets;
using System;
using System.Diagnostics;

namespace LunaSharpKernel.Misc
{

    internal class Signatures
    {
        #region Fields

        internal static IntPtr ModuleHandle { get; set; }

        #endregion
        internal static void LoadSignatures()
        {
            ModuleHandle = Process.GetCurrentProcess().Handle;
        }
    }
}
