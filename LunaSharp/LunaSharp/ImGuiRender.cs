using ImGuiNET;
using System.Numerics;

public class ImGuiRender
{
    private float f = 0.0f;
    private bool show_test_window = false;
    private bool show_another_window = false;
    private Vector3 clear_color = new Vector3(114f / 255f, 144f / 255f, 154f / 255f);
    private byte[] _textBuffer = new byte[100];
    private static ImGuiRender _instance { get; set; }
    public static ImGuiRender Instance => _instance ??= new ImGuiRender();

    public void Initialize()
    {
        ImGui.Text("Hello, world!");
        ImGui.SliderFloat("float", ref f, 0.0f, 1.0f, string.Empty);
        ImGui.ColorEdit3("clear color", ref clear_color);
        if (ImGui.Button("Test Window")) show_test_window = !show_test_window;
        if (ImGui.Button("Another Window")) show_another_window = !show_another_window;
        ImGui.Text(string.Format("Application average {0:F3} ms/frame ({1:F1} FPS)", 1000f / ImGui.GetIO().Framerate, ImGui.GetIO().Framerate));

        ImGui.InputText("Text input", _textBuffer, 100);
        ImGui.Text("Texture sample");
    }
}