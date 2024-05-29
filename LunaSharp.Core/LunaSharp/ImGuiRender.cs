using ImGuiNET;
using LunaSharp.Core.LunaSharpKernel.Detours.DetouredFunctions;
using LunaSharp.Memory;
using LunaSharp.Utils;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

public class ImGuiRender : IDisposable
{
    public bool IsInitialized { get; set; }
    private static ImGuiRender _instance { get; set; }
    public static ImGuiRender Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = new ImGuiRender();
            }
            return _instance;
        }
    }

    public void Initialize(IntPtr device)
    {
        if (IsInitialized) return;
        var cimguiPath = Path.Combine(ApplicationInfo.PathRoot, "Libs", "cimgui.dll");
        if (File.Exists(cimguiPath))
        {
            var libPtr = WinImports.LoadLibrary(cimguiPath);
            Logging.Write()($"cimgui.dll load at {libPtr:X}");
        }
        var ctx = ImGui.CreateContext();
        var io = ImGui.GetIO();
        io.ConfigFlags |= ImGuiConfigFlags.NavEnableKeyboard;
        io.ConfigFlags |= ImGuiConfigFlags.NavEnableGamepad;
        // Setup Dear ImGui style
        ImGui.StyleColorsDark();
        //ImGui::StyleColorsLight();

        // Setup Platform/Renderer backends
        var hwnd = Process.GetCurrentProcess().MainWindowHandle;
        ImplWin32.Init(hwnd);
        ImplDX9.Init(device);
        DInputDetour.Instance.GetDeviceState += Instance_GetDeviceState;

        IsInitialized = true;

    }

    private int Instance_GetDeviceState(IntPtr hDevice, int cbData, IntPtr lpvData)
    {
        if (this.IsInitialized)
        {
            try
            {

                int Result = DInputDetour.Instance.CallOrgin(hDevice, cbData, lpvData);

                if (Result == 0)
                {
                    ImGuiIOPtr IO = ImGui.GetIO();

                    if (cbData == 16 || cbData == 20)
                    {
                        DInputDetour.LPDIMOUSESTATE MouseData = DInputDetour.Instance.GetMouseData(lpvData);
                        IO.MouseDown[0] = (MouseData.rgbButtons[0] != 0);
                        IO.MouseDown[1] = (MouseData.rgbButtons[1] != 0);

                        IO.MouseWheel += (float)(MouseData.lZ / (float)SystemInformation.MouseWheelScrollDelta);

                    }

                }
                return Result;
            }
            catch (Exception ex)
            {
                //LogConsole(ex.Message); //Fix error : The arithmetic operation has caused an overflow.
            }
        }
        return DInputDetour.Instance.CallOrgin(hDevice, cbData, lpvData);
    }

    public void Render()
    {
        ImplDX9.NewFrame();
        ImplWin32.NewFrame();
        ImGui.NewFrame();
        ImGui.Begin("Another window from c#");
        ImGui.Text("Hello worlds");
        ImGui.Button("Click me");
    }

    public void Endscene()
    {
        ImGui.EndFrame();
        ImGui.Render();
        var data = ImGui.GetDrawData();
        ImplDX9.RenderDrawData(data);
    }

    public void Dispose()
    {
        ImplDX9.Shutdown();
        ImplWin32.Shutdown();
        ImGui.DestroyContext();
    }
}