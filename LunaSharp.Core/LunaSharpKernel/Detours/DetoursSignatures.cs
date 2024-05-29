using LunaSharp.Memory;
using System;

namespace LunaSharp.LunaSharpKernel.Detours
{
    internal static class DetoursSignatures
    {
        #region Static Methods

        internal static IntPtr GetWglSwapBufferPtr()
        {
            var handle = WinImports.GetModuleHandle("opengl32.dll");
            if(handle == IntPtr.Zero)
            {
                throw new LunaException("Could not found opengl32.dll");
            }
            var processHandle = WinImports.GetProcAddress(handle, "wglSwapBuffers");
            if (handle == IntPtr.Zero)
            {
                throw new LunaException("Could not found wglSwapBuffer");
            }
            return processHandle;
        }

        #endregion
    }
}
