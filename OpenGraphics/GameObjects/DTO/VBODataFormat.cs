
using OpenTK.Graphics.OpenGL4;

namespace OpenGraphics;

public record VBODataFormat
{
    public int Dimention { get; set; }
    public VertexAttribPointerType Type { get; set; }
    public bool Normalized { get; set; }
    public int Stride { get; set; }
    public int Offset { get; set; }
}
