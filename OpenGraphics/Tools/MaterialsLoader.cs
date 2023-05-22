
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
            },
            {
                "GoldBlock",
                new Material
                {
                    Specular = new Vector3(0.3f, 0.3f, 0.3f),
                    Shininess = 64f
                }
            },
            {
                "WoodenPlanks",
                new Material
                {
                    Specular = new Vector3(0.3f, 0.3f, 0.3f),
                    Shininess = 64f
                }
            }
        };
    }
    public static Material GetMaterial(string name)
    {
        return _materials[name];
    }
}
