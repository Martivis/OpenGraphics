
using OpenTK.Mathematics;
using System.Diagnostics;

namespace OpenGraphics;

public class Scene : IDisposable
{
    private IList<SolidObject> _solidObjects;
    private IList<GameObject> _gameObjects;
    private Stopwatch _stopwatch;
    public Scene()
    {
        var solidObjectShader = new Shader(@"Shaders\shader.vert", @"Shaders\shader.frag");
        var lightShader = new Shader(@"Shaders\shader.vert", @"Shaders\light.frag");

        var cobblestoneTexture = Texture.LoadFromFile("Resources/cobblestone.png");
        var cobblestoneSpecularTexture = Texture.LoadFromFile("Resources/cobblestone_specular2.png");
        var glowstoneTexture = Texture.LoadFromFile("Resources/glowstone.png");

        var cubeData = ObjectLoader.GetObject("stub");

        _solidObjects = new List<SolidObject>()
        {
            new SolidObject(
                cubeData,
                solidObjectShader,
                cobblestoneTexture,
                cobblestoneSpecularTexture,
                MaterialsLoader.GetMaterial("Cobblestone"))
        };

        _gameObjects = new List<GameObject>()
        {
            new GameObject(
                cubeData,
                lightShader,
                glowstoneTexture)
        };

        _stopwatch = new Stopwatch();
        _stopwatch.Start();
    }

    public void Draw(Camera camera)
    {
        ApplyTransformations(camera);

        foreach (var obj in _solidObjects)
        {
            obj.Draw();
        }
        foreach (var obj in _gameObjects)
        {
            obj.Draw();
        }
    }

    private void ApplyTransformations(Camera camera)
    {
        var step = _stopwatch.Elapsed.TotalSeconds;

        var cubeTransform = Matrix4.Identity;
        //cubeTransform *= Matrix4.CreateRotationY((float)MathHelper.DegreesToRadians(step * 15));
        //cubeTransform *= Matrix4.CreateRotationZ((float)MathHelper.DegreesToRadians(step * 15));
        _solidObjects[0].Transform(cubeTransform);


        var tetraederTransform = Matrix4.Identity;
        tetraederTransform *= Matrix4.CreateTranslation(2, 1, 0);
        tetraederTransform *= Matrix4.CreateRotationY((float)MathHelper.DegreesToRadians(step * 10));

        var light = new Light()
        {
            Ambient = new Vector3(0.1f, 0.1f, 0.1f),
            Diffuse = new Vector3(1, 0.9f, 0.81f),
            Specular = new Vector3(1, 0.9f, 0.81f),
            Position = tetraederTransform.ExtractTranslation()
        };

        _solidObjects[0].SetLight(light);
        _gameObjects[0].Transform(tetraederTransform);

        foreach (var obj in _solidObjects)
        {
            obj.SetViewPos(camera.Position);
            obj.SetViewMatrix(camera.GetViewMatrix());
            obj.SetProjectionMatrix(camera.GetProjectionMatrix());
        }

        foreach (var obj in _gameObjects)
        {
            obj.SetViewMatrix(camera.GetViewMatrix());
            obj.SetProjectionMatrix(camera.GetProjectionMatrix());
        }
    }

    public void Dispose()
    {
        foreach (var obj in _solidObjects)
        {
            obj.Dispose();
        }
        foreach (var obj in _gameObjects)
        {
            obj.Dispose();
        }
    }
}
