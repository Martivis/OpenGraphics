using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Diagnostics;

namespace OpenGraphics;

public class Game : GameWindow
{
    Scene _scene;
    Camera _camera;

    private float _aspectRatio;

    private FPSCounter _fpsCounter;

    bool _firstMove = true;
    Vector2 _lastCursorPos;

    const float cameraSpeed = 1.5f;
    const float sensitivity = 0.2f;

    

    Vector3 _lightPos = new Vector3(0, 0, 0);

    public Game(int width, int height, string title)
        : base(GameWindowSettings.Default,
            new NativeWindowSettings() { Size = (width, height), Title = title })
    {
        _aspectRatio = width / height;
        _fpsCounter = new FPSCounter();
    }


    protected override void OnLoad()
    {
        base.OnLoad();

        SetBackgroundColor(0, 0, 0, 1);

        GL.Enable(EnableCap.DepthTest);

        _camera = new Camera(Vector3.UnitZ * 2.0f, _aspectRatio);
        _scene = new Scene();
        
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

        _fpsCounter.CountFps();
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

    protected override void OnMouseWheel(MouseWheelEventArgs e)
    {
        base.OnMouseWheel(e);

        _camera.Fov -= e.OffsetY;
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
        base.OnRenderFrame(e);

        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        _scene.Draw(_camera);

        SwapBuffers();
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
        _scene.Dispose();
    }
}
