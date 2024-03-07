using LunaSharp.Hooking;
using LunaSharp.LunaSharpKernel.Detours.DetouredFunctions;

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
            OpenglState.CreateHook();
            Detours.ApplyAll();
        }

        #endregion
    }
}
