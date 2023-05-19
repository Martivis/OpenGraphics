using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;

namespace OpenGraphics;

public class VertexData
{
    private int _vbo;
    private int _ebo;

    private readonly float[] _vertices;
    private readonly uint[] _indices;

    private readonly IDictionary<string, VBODataFormat> _format;

    public VertexData(float[] vertices, uint[] indices, IDictionary<string, VBODataFormat> format)
    {
        _vertices = vertices;
        _indices = indices;
        _format = format;

        //CreateVBO();
        //CreateEBO();
    }

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

    public VBODataFormat GetFormat(string key) => _format[key];

    public int GetVerticesCount() => _indices.Length;

    public float[] GetVertices() => _vertices;
    public uint[] GetIndices() => _indices;
    public IDictionary<string, VBODataFormat> GetDictionary() => _format;
}
