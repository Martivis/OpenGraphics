
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace OpenGraphics;

public class GameObject : IDisposable
{
    private int _vbo;
    private int _vao;
    private int _ebo;

    private Texture _diffuseMap;
    private Texture _specularMap;
    private Shader _shader;

    private readonly float[] _vertices;

    private readonly uint[] _indices;

    public GameObject(Shader shader, Texture diffuse, Texture specular, float[] vertices, uint[] indices)
    {
        _vertices = vertices;
        _indices = indices;
        _shader = shader;
        _diffuseMap = diffuse;
        _specularMap = specular;

        _diffuseMap.Use(TextureUnit.Texture0);
        _specularMap.Use(TextureUnit.Texture1);
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
            stride: 8 * sizeof(float),
            offset: 0);

        var aNormalLocation = _shader.GetAttribLocation("aNormal");
        GL.EnableVertexAttribArray(aNormalLocation);
        GL.VertexAttribPointer(
            index: aNormalLocation,
            size: 3,
            type: VertexAttribPointerType.Float,
            normalized: false,
            stride: 8 * sizeof(float),
            offset: 3 * sizeof(float)
            );

        var aTextureLocation = _shader.GetAttribLocation("aTexCoord");
        GL.EnableVertexAttribArray(aTextureLocation);
        GL.VertexAttribPointer(
            index: aTextureLocation,
            size: 2, // This is about dimensions
            type: VertexAttribPointerType.Float,
            normalized: false,
            stride: 8 * sizeof(float),
            offset: 6 * sizeof(float));
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

        _diffuseMap.Use(TextureUnit.Texture0);
        _specularMap.Use(TextureUnit.Texture1);
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

    public void SetViewPos(Vector3 position)
    {
        _shader.SetVector3("viewPos", position);
    }

    public void SetMaterial(Material material)
    {
        _shader.SetInt("material.diffuse", 0);
        _shader.SetInt("material.specular", 1);
        _shader.SetVector3("material.specular", material.Specular);
        _shader.SetFloat("material.shininess", material.Shininess);
    }
    public void SetLight(Light light)
    {
        _shader.SetVector3("light.ambient", light.Ambient);
        _shader.SetVector3("light.diffuse", light.Diffuse);
        _shader.SetVector3("light.specular", light.Specular);
        _shader.SetVector3("light.position", light.Position);
    }

    public void Dispose()
    {
        _shader.Dispose();
    }
}
