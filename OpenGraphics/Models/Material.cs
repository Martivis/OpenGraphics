using OpenTK.Mathematics;

namespace OpenGraphics;

public class Material
{
    public Vector3 Ambient { get; set; }
    public Vector3 Diffuse { get; set; }
    public Vector3 Specular { get; set; }
    public float Shininess { get; set; }
}
