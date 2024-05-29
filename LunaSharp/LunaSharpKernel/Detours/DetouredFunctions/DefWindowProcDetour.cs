using ImGuiNET;
using RenderSpy.Inputs;
using System;
using System.Diagnostics;

namespace LunaSharp.LunaSharpKernel.Detours.DetouredFunctions
{
    public static class DefWindowProcDetour
    {
        public static DefWindowProc DefWindowProcW_Hook = new DefWindowProc();
        public static void CreateHook()
        {
            
            DefWindowProcW_Hook.WindowHandle = Process.GetCurrentProcess().MainWindowHandle;
            DefWindowProcW_Hook.Install();
            DefWindowProcW_Hook.WindowProc += (IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam) =>
            {

                try
                {
                    if (ImGuiRender.Instance.IsInitialized)
                    {
                        ImGuiIOPtr IO = ImGuiNET.ImGui.GetIO();
                        IO.MouseDrawCursor = true;

                        ImplWin32.WndProcHandler(hWnd, msg, (long)wParam, (uint)lParam);

                    }
                }
                catch (Exception ex)
                {
                    //LogConsole(ex.Message); //Fix error : The arithmetic operation has caused an overflow.
                }
                return IntPtr.Zero;
            };
        }
    }
}
