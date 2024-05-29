using LunaSharp.Core.LunaSharpKernel.Detours.DetouredFunctions;
using LunaSharp.Hooking;
using LunaSharp.LunaSharpKernel.Detours.DetouredFunctions;
using LunaSharp.Memory;
using System;

namespace LunaSharpKernel.Detours
{
    internal class DetoursMgr
    {
        #region Fields

        private static bool m_initialized;
        private static readonly DetourManager Detours = new DetourManager();

        #endregion

        #region Properties

        internal static DetourManager GetDetours => Detours;
        #endregion

        #region Static Methods

        internal static void Initialize()
        {
            if (m_initialized) return;
            m_initialized = true;
            var dinputHandle = WinImports.GetModuleHandle("dinput8.dll");
            if (dinputHandle != IntPtr.Zero)
            {
                DInputDetour.Instance.CreateHook();
            }
            PresentDetour.Instance.CreateHook();
            Detours.ApplyAll();
        }

        #endregion
    }
}
