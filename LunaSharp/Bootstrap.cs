using LunaSharp.LunaSharp;
using LunaSharp.Utils;
using LunaSharpKernel.Detours;
using LunaSharpKernel.Misc;
using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Windows.Forms;
using static LunaSharp.OpenGLNative;
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
                Logging.Write()("Process (" + Process.GetCurrentProcess().ProcessName + ") [" + Process.GetCurrentProcess().Id + "] Found");
                Logging.LogAllExceptions();

                //Get All Signatures
                Signatures.LoadSignatures();
                //Initialize Hooking System
                DetoursMgr.Initialize();
                Game.OnEventGameRender += Game_OnEventGameRender;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetBaseException().Message);
            }

            return 0;
        }

        private static void Game_OnEventGameRender(IntPtr hdc)
        {
            var player = ObjectMgr.LocalPlayer;
            var health = player.Health();
            var pos = player.NetworkOrigin;

            glColor3f(0.0f, 0.0f, 1.0f);
            glLineWidth(30);
            glBegin(BeginMode.GL_LINE_LOOP);
            glVertex2i(50, 90);
            glVertex2i(100, 90);
            glVertex2i(100, 150);
            glVertex2i(50, 150);
            glEnd();
            glFlush();
        }

        #endregion


    }
}
