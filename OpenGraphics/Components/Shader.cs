using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Reflection.Metadata;

namespace OpenGraphics;

public class Shader : IDisposable
{
    int _handle;
    private readonly Dictionary<string, int> _uniformLocations = new Dictionary<string, int>();

    public Shader(string vertexPath, string fragmentPath)
    {
        string vertexShaderSource = File.ReadAllText(vertexPath);
        string fragmentShaderSource = File.ReadAllText(fragmentPath);

        var vertexShader = CreateShader(vertexShaderSource, ShaderType.VertexShader);
        CompileShader(vertexShader);

        var fragmentShader = CreateShader(fragmentShaderSource, ShaderType.FragmentShader);
        CompileShader(fragmentShader);

        _handle = GL.CreateProgram();

        GL.AttachShader(_handle, vertexShader);
        GL.AttachShader(_handle, fragmentShader);
        GL.LinkProgram(_handle);

        LogProgramErrors(_handle);

        CleanShader(vertexShader);
        CleanShader(fragmentShader);

        GetUniformsLocation();
    }

    private int CreateShader(string shaderSource, ShaderType type)
    {
        int shader = GL.CreateShader(type);
        GL.ShaderSource(shader, shaderSource);
        return shader;
    }

    private void CompileShader(int shader)
    {
        GL.CompileShader(shader);
        LogShaderErrors(shader);
    }

    private void LogShaderErrors(int shader)
    {
        GL.GetShader(shader, ShaderParameter.CompileStatus, out int success);
        if (success == 0)
        {
            string log = GL.GetShaderInfoLog(shader);
            Console.WriteLine(log);
        }
    }

    private void LogProgramErrors(int program)
    {
        GL.GetProgram(program, GetProgramParameterName.LinkStatus, out int success);
        if (success == 0)
        {
            string infoLog = GL.GetProgramInfoLog(program);
            Console.WriteLine(infoLog);
        }
    }

    private void CleanShader(int shader)
    {
        GL.DetachShader(_handle, shader);
        GL.DeleteShader(shader);
    }

    private void GetUniformsLocation()
    {
        GL.GetProgram(_handle, GetProgramParameterName.ActiveUniforms, out var numberOfUniforms);

        for (var i = 0; i < numberOfUniforms; i++)
        {
            // get the name of this uniform,
            var key = GL.GetActiveUniform(_handle, i, out _, out _);

            // get the location,
            var location = GL.GetUniformLocation(_handle, key);

            // and then add it to the dictionary.
            _uniformLocations.Add(key, location);
        }
    }

    public int GetAttribLocation(string attribName)
    {
        return GL.GetAttribLocation(_handle, attribName);
    }

    public void Use()
    {
        GL.UseProgram(_handle);
    }

    public void Dispose()
    {
        GL.DeleteProgram(_handle);
    }

    /// <summary>
    /// Set a uniform int on this shader.
    /// </summary>
    /// <param name="name">The name of the uniform</param>
    /// <param name="data">The data to set</param>
    public void SetInt(string name, int data)
    {
        GL.UseProgram(_handle);
        GL.Uniform1(_uniformLocations[name], data);
    }

    /// <summary>
    /// Set a uniform float on this shader.
    /// </summary>
    /// <param name="name">The name of the uniform</param>
    /// <param name="data">The data to set</param>
    public void SetFloat(string name, float data)
    {
        GL.UseProgram(_handle);
        GL.Uniform1(_uniformLocations[name], data);
    }

    /// <summary>
    /// Set a uniform Matrix4 on this shader
    /// </summary>
    /// <param name="name">The name of the uniform</param>
    /// <param name="data">The data to set</param>
    /// <remarks>
    ///   <para>
    ///   The matrix is transposed before being sent to the shader.
    ///   </para>
    /// </remarks>
    public void SetMatrix4(string name, Matrix4 data)
    {
        GL.UseProgram(_handle);
        GL.UniformMatrix4(_uniformLocations[name], true, ref data);
    }

    /// <summary>
    /// Set a uniform Vector3 on this shader.
    /// </summary>
    /// <param name="name">The name of the uniform</param>
    /// <param name="data">The data to set</param>
    public void SetVector3(string name, Vector3 data)
    {
        GL.UseProgram(_handle);
        GL.Uniform3(_uniformLocations[name], data);
    }
}
