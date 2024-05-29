using LunaSharp.Utils;
using LunaSharpKernel.Detours;
using System;
using System.Runtime.InteropServices;

namespace LunaSharp.LunaSharpKernel.Detours.DetouredFunctions
{
    internal static class OpenglState
    {
        #region Static Methods

        private static string FunctionName => "OpenglState::wglSwapBuffer";

        internal static void CreateHook()
        {
            var swapBufferPtr = DetoursSignatures.GetWglSwapBufferPtr();
            if(swapBufferPtr == IntPtr.Zero)
            {
                Logging.Write($"OpenglState")("wglSwapBuffer() not Found");
                return;
            }
            NativeWglSwapBuffer originalFunction = Marshal.GetDelegateForFunctionPointer<NativeWglSwapBuffer>(swapBufferPtr);
            NativeWglSwapBuffer newFunction = Hooked_WglSwapBuffer;
            DetoursMgr.GetDetours.Create(originalFunction, newFunction, FunctionName);

        }

        private static bool Hooked_WglSwapBuffer(IntPtr hdc)
        {
            Game.OnGameRenderNative(hdc);
            return (bool)DetoursMgr.GetDetours[FunctionName].CallOriginal(hdc);
        }

        #endregion

        #region Nested Types, Enums, Delegates

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate bool NativeWglSwapBuffer(IntPtr hdc);

        #endregion
    }
}
