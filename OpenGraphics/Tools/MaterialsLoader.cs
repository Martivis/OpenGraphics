
using OpenTK.Mathematics;

namespace OpenGraphics;

public static class MaterialsLoader
{
    private static Dictionary<string, Material> _materials;

    static MaterialsLoader()
    {
        _materials = new Dictionary<string, Material>
        {
            {
                "Cobblestone",
                new Material
                {
                    Specular = new Vector3(0.5f, 0.5f, 0.5f),
                    Shininess = 16f
                }
            }
        };
    }
    public static Material GetMaterial(string name)
    {
        return _materials[name];
    }
}
