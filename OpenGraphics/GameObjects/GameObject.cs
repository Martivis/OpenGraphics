using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace OpenGraphics;

public class GameObject
{
    private int _vbo;
    private int _ebo;

    private readonly float[] _vertices;
    private readonly uint[] _indices;

    private readonly IDictionary<string, VBODataFormat> _format;

    private void CreateVBO()
    {
        _vbo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);
    }

    private void CreateEBO()
    {
        _ebo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);
        GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);
    }
    private int _vao;

    protected Shader _shader;
    protected Texture _diffuseMap;
    

    public GameObject(float[] vertices, uint[] indices, IDictionary<string, VBODataFormat> format, Shader shader, Texture diffuse)
    {
        _vertices = vertices;
        _indices = indices;
        _shader = shader;
        _format = format;
        _diffuseMap = diffuse;

        CreateVBO();
        CreateVAO();
        CreateEBO();

        _diffuseMap.Use(TextureUnit.Texture0);
        _shader.Use();
    }


    private void CreateVAO()
    {
        _vao = GL.GenVertexArray();
        GL.BindVertexArray(_vao);

        AddAttribute("aPosition");
        AddAttribute("aNormal");
        AddAttribute("aTexCoord");
    }

    private void AddAttribute(string key)
    {
        var aLocation = _shader.GetAttribLocation(key);
        var format = _format[key];
        GL.EnableVertexAttribArray(aLocation);
        GL.VertexAttribPointer(
            index: aLocation,
            format.Dimention,
            format.Type,
            format.Normalized,
            format.Stride,
            format.Offset);
    }

    public virtual void Draw()
    {
        GL.BindVertexArray(_vao);

        _diffuseMap.Use(TextureUnit.Texture0);
        
        _shader.Use();

        GL.DrawElements(
            mode: PrimitiveType.Triangles,
            count: _vertices.Length,
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

    public void SetViewPos(Vector3 position)
    {
        _shader.SetVector3("viewPos", position);
    }


    public void Dispose()
    {
        _shader.Dispose();
    }
}
