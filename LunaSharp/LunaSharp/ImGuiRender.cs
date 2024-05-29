using ImGuiNET;
using LunaSharp.Utils;
using RenderSpy.Globals;
using System;
using System.Diagnostics;
using System.IO;

public class ImGuiRender : IDisposable
{
    public bool IsInitialized { get; set; }
    private static ImGuiRender _instance { get; set; }
    public static ImGuiRender Instance => _instance ??= new ImGuiRender();

    public void Initialize(IntPtr device)
    {
        if (IsInitialized) return;
        var cimguiPath = Path.Combine(ApplicationInfo.PathRoot, "Libs", "cimgui.dll");
        if (File.Exists(cimguiPath))
        {
            var libPtr = WinApi.LoadLibrary(cimguiPath);
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
        IsInitialized = true;

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