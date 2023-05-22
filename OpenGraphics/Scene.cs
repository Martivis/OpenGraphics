
using OpenTK.Mathematics;
using System.Diagnostics;

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
                "wooden_planks1",
                new SolidObject(
                    cubeData,
                    new Shader(@"Shaders\shader.vert", @"Shaders\shader.frag"),
                    _textures["oakPlanks"],
                    _textures["oakPlanks"],
                    MaterialsLoader.GetMaterial("WoodenPlanks"),
                    Matrix4.CreateTranslation(-1, -1, 0))
            },
            {
                "wooden_planks2",
                new SolidObject(
                    cubeData,
                    new Shader(@"Shaders\shader.vert", @"Shaders\shader.frag"),
                    _textures["oakPlanks"],
                    _textures["oakPlanks"],
                    MaterialsLoader.GetMaterial("WoodenPlanks"),
                    Matrix4.CreateTranslation(1, -1, 0))
            },
            {
                "wooden_planks3",
                new SolidObject(
                    cubeData,
                    new Shader(@"Shaders\shader.vert", @"Shaders\shader.frag"),
                    _textures["oakPlanks"],
                    _textures["oakPlanks"],
                    MaterialsLoader.GetMaterial("WoodenPlanks"),
                    Matrix4.CreateTranslation(0, -1, 1))
            },
            {
                "wooden_planks4",
                new SolidObject(
                    cubeData,
                    new Shader(@"Shaders\shader.vert", @"Shaders\shader.frag"),
                    _textures["oakPlanks"],
                    _textures["oakPlanks"],
                    MaterialsLoader.GetMaterial("WoodenPlanks"),
                    Matrix4.CreateTranslation(0, -1, -1))
            },
            {
                "wooden_planks5",
                new SolidObject(
                    cubeData,
                    new Shader(@"Shaders\shader.vert", @"Shaders\shader.frag"),
                    _textures["oakPlanks"],
                    _textures["oakPlanks"],
                    MaterialsLoader.GetMaterial("WoodenPlanks"),
                    Matrix4.CreateTranslation(-1, -1, -1))
            },
            {
                "wooden_planks6",
                new SolidObject(
                    cubeData,
                    new Shader(@"Shaders\shader.vert", @"Shaders\shader.frag"),
                    _textures["oakPlanks"],
                    _textures["oakPlanks"],
                    MaterialsLoader.GetMaterial("WoodenPlanks"),
                    Matrix4.CreateTranslation(-1, -1, 1))
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
        glowstoneRotation *= Matrix4.CreateTranslation(2, 1, 0);
        glowstoneRotation *= Matrix4.CreateRotationY((float)MathHelper.DegreesToRadians(step * 10));

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
