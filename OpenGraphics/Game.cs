
using OpenGraphics.Shaders;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace OpenGraphics;

public class Game : GameWindow
{
    private int _vbo;
    private int _vao;

    Shader _shader;

    private float[] _vertices =
    {
        -0.5f, -0.5f, 0.0f,
         0.5f, -0.5f, 0.0f,
         0.5f,  0.5f, 0.0f,
         1.0f, 1.0f, 0.0f
    };

    public Game(int width, int height, string title) 
        : base(GameWindowSettings.Default, 
            new NativeWindowSettings() { Size = (width, height), Title = title }) 
    {
    }

    protected override void OnKeyDown(KeyboardKeyEventArgs e)
    {
        base.OnKeyDown(e);
        switch (e.Key)
        {
            case OpenTK.Windowing.GraphicsLibraryFramework.Keys.A:
                break;
            case OpenTK.Windowing.GraphicsLibraryFramework.Keys.D:
                break;
            case OpenTK.Windowing.GraphicsLibraryFramework.Keys.S:
                break;
            case OpenTK.Windowing.GraphicsLibraryFramework.Keys.W:
                break;
            case OpenTK.Windowing.GraphicsLibraryFramework.Keys.Escape:
                Close();
                break;
            case OpenTK.Windowing.GraphicsLibraryFramework.Keys.Enter:
                break;
            default:
                break;
        }
    }

    protected override void OnLoad()
    {
        base.OnLoad();

        SetBackgroundColor(0, 0, 0, 1);

        _shader = new Shader(@"Shaders\shader.vert", @"Shaders\shader.frag");
        CreateVBO();
        CreateVAO();

    }

    private void SetBackgroundColor(float red, float green, float blue, float alpha)
    {
        GL.ClearColor(red, green, blue, alpha);
    }

    private void CreateVBO()
    {
        _vbo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);
    }

    private void CreateVAO()
    {
        _vao = GL.GenVertexArray();
        GL.BindVertexArray(_vao);

        var attribLocation = _shader.GetAttribLocation("aPosition");

        GL.VertexAttribPointer(
            index: 0, 
            size: 3, 
            type: VertexAttribPointerType.Float, 
            normalized: false, 
            stride: 3 * sizeof(float), 
            offset: attribLocation);

        GL.EnableVertexAttribArray(attribLocation);
    }

    

    protected override void OnRenderFrame(FrameEventArgs e)
    {
        base.OnRenderFrame(e);

        GL.Clear(ClearBufferMask.ColorBufferBit);

        DrawObject();

        SwapBuffers();
    }

    private void DrawObject()
    {
        GL.BindVertexArray(_vao);
        GL.DrawArrays(
            mode: PrimitiveType.Triangles,
            first: 0,
            count: _vertices.Length / 3);
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);

        GL.Viewport(0, 0, e.Width, e.Height);
    }

    protected override void OnUnload()
    {
        base.OnUnload();
        _shader.Dispose();
    }
}
