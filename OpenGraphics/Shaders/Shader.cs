using OpenTK.Graphics.OpenGL4;

namespace OpenGraphics.Shaders;

public class Shader : IDisposable
{
    int _handle;

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
}
