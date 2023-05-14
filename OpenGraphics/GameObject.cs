
using OpenTK.Graphics.OpenGL4;

namespace OpenGraphics;

public class GameObject
{
    private int _vbo;
    private int _vao;
    private int _ebo;

    private Texture _texture;
    private readonly Shader _shader;

    private readonly float[] _vertices =
    {
        -0.5f, -0.0f, 0.0f, 1.0f, 1.0f,
         0.5f,  0.0f, 0.0f, 0.0f, 0.0f,
         0.0f,  0.5f, 0.0f, 1.0f, 0.0f,
         0.0f, -0.5f, 0.0f, 0.0f, 1.0f,

        -0.5f, -0.0f, -0.2f, 1.0f, 0.0f,
         0.5f,  0.0f, -0.2f, 0.0f, 1.0f,
         0.0f,  0.5f, -0.2f, 1.0f, 1.0f,
         0.0f, -0.5f, -0.2f, 0.0f, 0.0f,
    };

    private readonly uint[] _indices =
    {
        0, 1, 2,
        0, 1, 3,

        0, 6, 4,
        0, 6, 2,

        0, 7, 3,
        0, 7, 4,

        5, 3, 7,
        5, 3, 1,

        5, 2, 1,
        5, 2, 6,

        5, 4, 7,
        5, 4, 6,
    };

    public GameObject(Shader shader, float[] vertices, uint[] indices)
    {
        _vertices = vertices;
        _indices = indices;
        _shader = shader;

        _texture = Texture.LoadFromFile("Resources/Ваня.jpg");
        _texture.Use(TextureUnit.Texture0);


        CreateVBO();
        CreateVAO();
        CreateEBO();

    }

    private void CreateVBO()
    {
        _vbo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);
    }

    private void CreateVAO()
    {
        _vao = GL.GenVertexArray();
        GL.BindVertexArray(_vao);

        var aPositionLocation = _shader.GetAttribLocation("aPosition");
        GL.EnableVertexAttribArray(aPositionLocation);
        GL.VertexAttribPointer(
            index: aPositionLocation,
            size: 3, // This is about dimensions
            type: VertexAttribPointerType.Float,
            normalized: false,
            stride: 5 * sizeof(float),
            offset: 0);

        var textureLocation = _shader.GetAttribLocation("aTexCoord");
        GL.EnableVertexAttribArray(textureLocation);
        GL.VertexAttribPointer(
            index: textureLocation,
            size: 2, // This is about dimensions
            type: VertexAttribPointerType.Float,
            normalized: false,
            stride: 5 * sizeof(float),
            offset: 3 * sizeof(float));
    }

    private void CreateEBO()
    {
        _ebo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);
        GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);
    }

    public void Draw()
    {
        GL.BindVertexArray(_vao);

        _texture.Use(TextureUnit.Texture0);
        _shader.Use();

        GL.DrawElements(
            mode: PrimitiveType.Triangles,
            count: _indices.Length,
            type: DrawElementsType.UnsignedInt,
            indices: 0
            );
    }
}
