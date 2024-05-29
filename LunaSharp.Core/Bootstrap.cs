using LunaSharp.Memory;
using LunaSharp.Utils;
using LunaSharpKernel.Detours;
using LunaSharpKernel.Misc;
using System;
using System.Diagnostics;
namespace LunaSharp
{
    internal static class Bootstrap
    {
        #region Fields

        private static bool m_initialized;

        #endregion
        #region Static Methods

        [STAThread]
        private static int EntryPoint(string args)
        {
            try
            {
                if (m_initialized) return 0;
                m_initialized = true;
                Logging.LogAllExceptions();
                WinImports.AllocConsole();
                Logging.Write()("Process (" + Process.GetCurrentProcess().ProcessName + ") [" + Process.GetCurrentProcess().Id + "] Found");

                //Get All Signatures
                Signatures.LoadSignatures();
                //Initialize Hooking System
                DetoursMgr.Initialize();
                Game.OnEventGameRender += Game_OnEventGameRender;
            }
            catch (Exception ex)
            {
            }

            return 0;
        }

        private static void Game_OnEventGameRender(IntPtr device)
        {
            ImGuiRender.Instance.Initialize(device);
            ImGuiRender.Instance.Render();
            ImGuiRender.Instance.Endscene();
        }



        #endregion


    }
}
