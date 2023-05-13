using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System.Diagnostics;

namespace OpenGraphics;

public class Game : GameWindow
{
    private int _vbo;
    private int _vao;
    private int _ebo;

    Shader _shader;

    Texture _texture;

    private readonly float[] _vertices =
    {
        -0.5f, -0.0f, 0.0f,   1.0f,  1.0f,
         0.5f,  0.0f, 0.0f,   0.0f,  0.0f,
         0.0f,  0.5f, 0.0f,   1.0f,  0.0f,
         0.0f, -0.5f, 0.0f,   0.0f,  1.0f,
    };

    private readonly uint[] _indices =
    {
        0, 1, 2,
        0, 1, 3
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
        _shader.Use();

        CreateVBO();
        CreateVAO();
        CreateEBO();

        
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

        var aPositionLocation = _shader.GetAttribLocation("aPosition");
        GL.EnableVertexAttribArray(aPositionLocation);
        GL.VertexAttribPointer(
            index: aPositionLocation, 
            size: 3, // This is about dimensions
            type: VertexAttribPointerType.Float, 
            normalized: false, 
            stride: 5 * sizeof(float), 
            offset: 0);

        var textureLocation = _shader.GetAttribLocation("aTexCoord");
        GL.EnableVertexAttribArray(textureLocation);
        GL.VertexAttribPointer(
            index: textureLocation,
            size: 2, // This is about dimensions
            type: VertexAttribPointerType.Float,
            normalized: false,
            stride: 5 * sizeof(float),
            offset: 3 * sizeof(float));

        _texture = Texture.LoadFromFile("Resources/Ваня.jpg");
        _texture.Use(TextureUnit.Texture0);
    }

    private void CreateEBO()
    {
        _ebo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);
        GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);
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
        _texture.Use(TextureUnit.Texture0);
        _shader.Use();
        GL.DrawElements(
            mode: PrimitiveType.Triangles,
            count: _indices.Length,
            type: DrawElementsType.UnsignedInt,
            indices: 0
            );
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
