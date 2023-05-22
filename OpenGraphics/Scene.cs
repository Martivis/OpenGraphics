
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace OpenGraphics;

public class Scene : IDisposable
{
    private Dictionary<string, GameObject> _gameObjects;
    private Dictionary<string, Texture> _textures;
    private Stopwatch _stopwatch;
    public Scene()
    {
        _textures = new Dictionary<string, Texture>()
        {
            { "cobblestone", Texture.LoadFromFile("Resources/cobblestone.png") },
            { "cobblestoneSpecular", Texture.LoadFromFile("Resources/cobblestone_specular2.png") },
            { "glowstone", Texture.LoadFromFile("Resources/glowstone.png") },
            { "oakPlanks", Texture.LoadFromFile("Resources/planks_oak.png") },
            { "goldBlock", Texture.LoadFromFile("Resources/gold_block.png") },
        };
        
        var cubeData = ObjectLoader.GetObject("stub");
        _gameObjects = new Dictionary<string, GameObject>()
        {
            {
                "cobblestone1",
                new SolidObject(
                    cubeData,
                    new Shader(@"Shaders\shader.vert", @"Shaders\shader.frag"),
                    _textures["cobblestone"],
                    _textures["cobblestoneSpecular"],
                    MaterialsLoader.GetMaterial("Cobblestone"),
                    Matrix4.CreateTranslation(0, 0, 0))
            },
            {
                "gold_block1",
                new SolidObject(
                    cubeData,
                    new Shader(@"Shaders\shader.vert", @"Shaders\shader.frag"),
                    _textures["goldBlock"],
                    _textures["goldBlock"],
                    MaterialsLoader.GetMaterial("GoldBlock"),
                    Matrix4.CreateTranslation(1, -1, 1))
            },
            {
                "rotating_glowstone",
                new GameObject(
                    cubeData,
                    new Shader(@"Shaders\shader.vert", @"Shaders\light.frag"),
                    _textures["glowstone"],
                    Matrix4.CreateTranslation(0, 0, 0))
            },
        };

        var roomCorner1 = new Vector3(-2, -1, -2);
        var roomCorner2 = new Vector3(1, 1, 1);

        for (int x = (int)roomCorner1.X; x <= roomCorner2.X; x++)
        {
            for (int z = (int)roomCorner1.Z; z <= roomCorner2.Z; z++)
            {
                if (x == roomCorner1.X && z == roomCorner1.Z) // Skip place for gold block
                { 
                    continue; 
                }
                var yLimit = (x == roomCorner1.X || z == roomCorner1.Z) ? roomCorner2.Y : roomCorner1.Y;
                for (int y = (int)roomCorner1.Y; y <= yLimit; y++)
                {
                    _gameObjects.Add($"wooden_planks{x}{y}{z}",
                    new SolidObject(
                    cubeData,
                    new Shader(@"Shaders\shader.vert", @"Shaders\shader.frag"),
                    _textures["oakPlanks"],
                    _textures["oakPlanks"],
                    MaterialsLoader.GetMaterial("WoodenPlanks"),
                    Matrix4.CreateTranslation(new Vector3(x, y, z))));
                }
            }
        }

        _stopwatch = new Stopwatch();
        _stopwatch.Start();
    }

    public void Draw(Camera camera)
    {
        ApplyTransformations(camera);

        foreach (var obj in _gameObjects)
        {
            obj.Value.Draw();
        }
    }

    private void ApplyTransformations(Camera camera)
    {
        var step = _stopwatch.Elapsed.TotalSeconds;

        var glowstoneRotation = Matrix4.Identity;
        glowstoneRotation *= Matrix4.CreateTranslation(5, 1, 0);
        glowstoneRotation *= Matrix4.CreateRotationY((float)MathHelper.DegreesToRadians(step * 5));

        var light = new Light()
        {
            Ambient = new Vector3(0.1f, 0.1f, 0.1f),
            Diffuse = new Vector3(1, 0.9f, 0.81f),
            Specular = new Vector3(1, 0.9f, 0.81f),
            Position = glowstoneRotation.ExtractTranslation()
        };

        _gameObjects["rotating_glowstone"].Position = glowstoneRotation;

        foreach (var obj in _gameObjects)
        {
            (obj.Value as SolidObject)?.SetLight(light);

            if (obj.Value is SolidObject solid)
            {
                solid.SetViewPos(camera.Position);
            }
            obj.Value.SetViewMatrix(camera.GetViewMatrix());
            obj.Value.SetProjectionMatrix(camera.GetProjectionMatrix());
        }
    }

    public void Dispose()
    {
        foreach (var obj in _gameObjects)
        {
            obj.Value.Dispose();
        }
    }
}
