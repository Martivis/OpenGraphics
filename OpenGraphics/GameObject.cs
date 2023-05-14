
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace OpenGraphics;

public class GameObject : IDisposable
{
    private int _vbo;
    private int _vao;
    private int _ebo;

    private Texture _texture;
    private Shader _shader;

    private readonly float[] _vertices;

    private readonly uint[] _indices;

    public GameObject(Shader shader, Texture texture, float[] vertices, uint[] indices)
    {
        _vertices = vertices;
        _indices = indices;
        _shader = shader;
        _texture = texture;

        _texture.Use(TextureUnit.Texture0);
        _shader.Use();


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

    public void Transform(Matrix4 matrix)
    {
        _shader.SetMatrix4("transform", Matrix4.Identity * matrix);
    }

    public void SetViewMatrix(Matrix4 matrix)
    {
        _shader.SetMatrix4("view", matrix);
    }

    public void SetProjectionMatrix(Matrix4 matrix)
    {
        _shader.SetMatrix4("projection", matrix);
    }

    public void Dispose()
    {
        _shader.Dispose();
    }
}
