using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Diagnostics;

namespace OpenGraphics;

public class Game : GameWindow
{
    private int _vbo;
    private int _vao;
    private int _ebo;

    Shader _shader;
    Texture _texture;

    Camera _camera;

    private float _aspectRatio;
    Stopwatch _stopwatch;

    bool _firstMove = true;
    Vector2 _lastCursorPos;

    const float cameraSpeed = 1.5f;
    const float sensitivity = 0.2f;

    private readonly float[] _vertices =
    {
        -0.5f, -0.0f, 0.0f, 1.0f, 1.0f,
         0.5f,  0.0f, 0.0f, 0.0f, 0.0f,
         0.0f,  0.5f, 0.0f, 1.0f, 0.0f,
         0.0f, -0.5f, 0.0f, 0.0f, 1.0f,
        
        -0.5f, -0.0f, -0.2f, 1.0f, 0.0f,
         0.5f,  0.0f, -0.2f, 0.0f, 1.0f,
         0.0f,  0.5f, -0.2f, 1.0f, 1.0f,
         0.0f, -0.5f, -0.2f, 0.0f, 0.0f,
    };

    private readonly uint[] _indices =
    {
        0, 1, 2,
        0, 1, 3,
        
        0, 6, 4,
        0, 6, 2,
        
        0, 7, 3,
        0, 7, 4,
        
        5, 3, 7,
        5, 3, 1,
        
        5, 2, 1,
        5, 2, 6,
        
        5, 4, 7,
        5, 4, 6,
    };

    public Game(int width, int height, string title) 
        : base(GameWindowSettings.Default, 
            new NativeWindowSettings() { Size = (width, height), Title = title }) 
    {
        _aspectRatio = width / height;
    }


    protected override void OnLoad()
    {
        base.OnLoad();

        SetBackgroundColor(0, 0, 0, 1);

        _shader = new Shader(@"Shaders\shader.vert", @"Shaders\shader.frag");
        _shader.Use();

        _texture = Texture.LoadFromFile("Resources/Ваня.jpg");
        _texture.Use(TextureUnit.Texture0);

        CreateVBO();
        CreateVAO();
        CreateEBO();

        GL.Enable(EnableCap.DepthTest);

        _camera = new Camera(Vector3.UnitZ * 0.8f, _aspectRatio);

        _stopwatch = new Stopwatch();
        _stopwatch.Start();
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
    }

    private void CreateEBO()
    {
        _ebo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);
        GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);
    }

    protected override void OnUpdateFrame(FrameEventArgs e)
    {
        base.OnUpdateFrame(e);

        if (!IsFocused)
        {
            return;
        }

        var input = KeyboardState;

        if (input.IsKeyDown(Keys.Escape))
        {
            Close();
        }

        

        if (input.IsKeyDown(Keys.W))
        {
            _camera.Position += _camera.Front * cameraSpeed * (float)e.Time; // Forward
        }

        if (input.IsKeyDown(Keys.S))
        {
            _camera.Position -= _camera.Front * cameraSpeed * (float)e.Time; // Backwards
        }
        if (input.IsKeyDown(Keys.A))
        {
            _camera.Position -= _camera.Right * cameraSpeed * (float)e.Time; // Left
        }
        if (input.IsKeyDown(Keys.D))
        {
            _camera.Position += _camera.Right * cameraSpeed * (float)e.Time; // Right
        }
        if (input.IsKeyDown(Keys.Space))
        {
            _camera.Position += _camera.Up * cameraSpeed * (float)e.Time; // Up
        }
        if (input.IsKeyDown(Keys.LeftShift))
        {
            _camera.Position -= _camera.Up * cameraSpeed * (float)e.Time; // Down
        }

        if (MouseState.IsButtonDown(MouseButton.Left))
        {
            CursorState = CursorState.Grabbed;
            RotateCamara();
        }
        else
        {
            _firstMove = true;
            CursorState = CursorState.Normal;
        }
    }

    private void RotateCamara()
    {
        var mouse = MouseState;
        if (_firstMove)
        {
            _lastCursorPos = new Vector2(mouse.X, mouse.Y);
            _firstMove = false;
        }
        else
        {
            // Calculate the offset of the mouse position
            var deltaX = mouse.X - _lastCursorPos.X;
            var deltaY = mouse.Y - _lastCursorPos.Y;
            _lastCursorPos = new Vector2(mouse.X, mouse.Y);

            // Apply the camera pitch and yaw (we clamp the pitch in the camera class)
            _camera.Yaw += deltaX * sensitivity;
            _camera.Pitch -= deltaY * sensitivity; // Reversed since y-coordinates range from bottom to top
        }
    }

    // In the mouse wheel function, we manage all the zooming of the camera.
    // This is simply done by changing the FOV of the camera.
    protected override void OnMouseWheel(MouseWheelEventArgs e)
    {
        base.OnMouseWheel(e);

        _camera.Fov -= e.OffsetY;
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
        base.OnRenderFrame(e);

        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        ApplyTransformations();

        DrawObject();

        SwapBuffers();
    }

    private void ApplyTransformations()
    {
        var transform = Matrix4.Identity;

        var step = _stopwatch.Elapsed.TotalSeconds;

        transform = transform * Matrix4.CreateRotationY((float)MathHelper.DegreesToRadians(step * 15));
        transform = transform * Matrix4.CreateRotationZ((float)MathHelper.DegreesToRadians(step * 15));


        _shader.SetMatrix4("transform", transform);
        _shader.SetMatrix4("view", _camera.GetViewMatrix());
        _shader.SetMatrix4("projection", _camera.GetProjectionMatrix());
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
        _aspectRatio = e.Width / e.Height;
        _camera.AspectRatio = _aspectRatio;
    }

    protected override void OnUnload()
    {
        base.OnUnload();
        _shader.Dispose();
    }
}
