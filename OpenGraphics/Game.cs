using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Diagnostics;

namespace OpenGraphics;

public class Game : GameWindow
{
    private IList<GameObject> _objects;

    Camera _camera;

    private float _aspectRatio;
    Stopwatch _stopwatch;

    private uint _framesCount;
    private double _lastCutoff;

    bool _firstMove = true;
    Vector2 _lastCursorPos;

    const float cameraSpeed = 1.5f;
    const float sensitivity = 0.2f;

    float[] _vertices1 = {
        // Position               Normals                Texture 
        -0.5f, -0.5f,  0.5f,    0.0f,  0.0f,  1.0f,    0.0f, 0.0f, // Front face
         0.5f, -0.5f,  0.5f,    0.0f,  0.0f,  1.0f,    1.0f, 0.0f,
         0.5f,  0.5f,  0.5f,    0.0f,  0.0f,  1.0f,    1.0f, 1.0f,
        -0.5f,  0.5f,  0.5f,    0.0f,  0.0f,  1.0f,    0.0f, 1.0f,
                                                       
        // Position               Normals                Texture                               
        -0.5f, -0.5f, -0.5f,    0.0f,  0.0f, -1.0f,    1.0f, 0.0f, // Back face   
        -0.5f,  0.5f, -0.5f,    0.0f,  0.0f, -1.0f,    1.0f, 1.0f,
         0.5f,  0.5f, -0.5f,    0.0f,  0.0f, -1.0f,    0.0f, 1.0f,
         0.5f, -0.5f, -0.5f,    0.0f,  0.0f, -1.0f,    0.0f, 0.0f,
                                                       
        // Position               Normals                Texture                                
        -0.5f,  0.5f, -0.5f,    0.0f,  1.0f,  0.0f,    0.0f, 1.0f, // Top face   
        -0.5f,  0.5f,  0.5f,    0.0f,  1.0f,  0.0f,    0.0f, 0.0f,
         0.5f,  0.5f,  0.5f,    0.0f,  1.0f,  0.0f,    1.0f, 0.0f,
         0.5f,  0.5f, -0.5f,    0.0f,  1.0f,  0.0f,    1.0f, 1.0f,
                                                       
        // Position               Normals                Texture                              
        -0.5f, -0.5f, -0.5f,    0.0f, -1.0f,  0.0f,    0.0f, 0.0f, // Bottom face  
         0.5f, -0.5f, -0.5f,    0.0f, -1.0f,  0.0f,    1.0f, 0.0f,
         0.5f, -0.5f,  0.5f,    0.0f, -1.0f,  0.0f,    1.0f, 1.0f,
        -0.5f, -0.5f,  0.5f,    0.0f, -1.0f,  0.0f,    0.0f, 1.0f,
                                                       
        // Position               Normals                Texture                              
         0.5f, -0.5f, -0.5f,    1.0f,  0.0f,  0.0f,    1.0f, 0.0f, // Right face    
         0.5f,  0.5f, -0.5f,    1.0f,  0.0f,  0.0f,    1.0f, 1.0f,
         0.5f,  0.5f,  0.5f,    1.0f,  0.0f,  0.0f,    0.0f, 1.0f,
         0.5f, -0.5f,  0.5f,    1.0f,  0.0f,  0.0f,    0.0f, 0.0f,
                                                       
        // Position               Normals                Texture                            
        -0.5f, -0.5f, -0.5f,   -1.0f,  0.0f,  0.0f,    0.0f, 0.0f, // Left face       
        -0.5f, -0.5f,  0.5f,   -1.0f,  0.0f,  0.0f,    1.0f, 0.0f,
        -0.5f,  0.5f,  0.5f,   -1.0f,  0.0f,  0.0f,    1.0f, 1.0f,
        -0.5f,  0.5f, -0.5f,   -1.0f,  0.0f,  0.0f,    0.0f, 1.0f
    };

    uint[] _indices1 = {
        0, 1, 2, 0, 2, 3,       // Front face
        4, 5, 6, 4, 6, 7,       // Back face
        8, 9, 10, 8, 10, 11,    // Top face
        12, 13, 14, 12, 14, 15, // Bottom face
        16, 17, 18, 16, 18, 19, // Right face
        20, 21, 22, 20, 22, 23  // Left face
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

        var objectShader = new Shader(@"Shaders\shader.vert", @"Shaders\shader.frag");
        var lightShader = new Shader(@"Shaders\shader.vert", @"Shaders\light.frag");

        var cobblestoneTexture = Texture.LoadFromFile("Resources/cobblestone.png");
        var glowstoneTexture = Texture.LoadFromFile("Resources/glowstone.png");

        _objects = new List<GameObject>()
        {
            new GameObject(objectShader, cobblestoneTexture, _vertices1, _indices1),
            new GameObject(lightShader, glowstoneTexture, _vertices1, _indices1)
        };

        GL.Enable(EnableCap.DepthTest);

        _camera = new Camera(Vector3.UnitZ * 2.0f, _aspectRatio);

        _stopwatch = new Stopwatch();
        _stopwatch.Start();
    }

    private void SetBackgroundColor(float red, float green, float blue, float alpha)
    {
        GL.ClearColor(red, green, blue, alpha);
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

        CountFps();
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

    private void CountFps()
    {
        _framesCount++;
        if (_stopwatch.Elapsed.TotalSeconds >= _lastCutoff + 1)
        {
            _lastCutoff = _stopwatch.Elapsed.TotalSeconds;
            Console.Clear();
            Console.WriteLine($"Fps: {_framesCount}");
            _framesCount = 0;
        }
    }

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



        foreach (var obj in _objects)
        {
            obj.Draw();
        }

        SwapBuffers();
    }

    private void ApplyTransformations()
    {
        var step = _stopwatch.Elapsed.TotalSeconds;

        var cubeTransform = Matrix4.Identity;
        //cubeTransform *= Matrix4.CreateRotationY((float)MathHelper.DegreesToRadians(step * 15));
        //cubeTransform *= Matrix4.CreateRotationZ((float)MathHelper.DegreesToRadians(step * 15));
        _objects[0].Transform(cubeTransform);

        var tetraederTransform = Matrix4.Identity;
        tetraederTransform *= Matrix4.CreateTranslation(2, 0, 0);
        tetraederTransform *= Matrix4.CreateRotationY((float)MathHelper.DegreesToRadians(step * 10));
        _objects[1].Transform(tetraederTransform);

        _objects[0].SetAmbientLight(new Vector3(0.2f, 0.1f, 0.01f));
        _objects[0].SetLightPos(new Vector3(2, 2f, 0.8f));

        foreach (var obj in _objects)
        {
            obj.SetViewMatrix(_camera.GetViewMatrix());
            obj.SetProjectionMatrix(_camera.GetProjectionMatrix());
        }
    }



    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);
        GL.Viewport(0, 0, e.Width, e.Height);
        _aspectRatio = e.Width / (float)e.Height;
        _camera.AspectRatio = _aspectRatio;
    }

    protected override void OnUnload()
    {
        base.OnUnload();
        foreach (var obj in _objects)
        {
            obj.Dispose();
        }
    }
}
