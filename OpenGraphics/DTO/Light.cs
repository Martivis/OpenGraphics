using OpenTK.Mathematics;

namespace OpenGraphics;

public class Light
{
    public Vector3 Position { get; set; }

    public Vector3 Ambient { get; set; }
    public Vector3 Diffuse { get; set; }
    public Vector3 Specular { get; set; }
}
