using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace OpenGraphics;

public class GameObject
{
    private VertexData _vertexData;
    private int _vao;

    protected Shader _shader;
    protected Texture _diffuseMap;

    private Matrix4 _position;

    public Matrix4 Position
    {
        get
        {
            return _position;
        }
        set
        {
            _position = value;
            _shader.SetMatrix4("transform", Matrix4.Identity * value);
        }
    }

    public GameObject(VertexData vertexData, Shader shader, Texture diffuse, Matrix4 position)
    {
        _vertexData = vertexData;
        _shader = shader;
        _diffuseMap = diffuse;
        Position = position;

        CreateVAO();
        vertexData.CreateEBO();

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
        var format = _vertexData.GetFormat(key);
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
            count: _vertexData.GetVerticesCount(),
            type: DrawElementsType.UnsignedInt,
            indices: 0
            );
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
