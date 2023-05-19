using OpenTK.Graphics.OpenGL4;

namespace OpenGraphics;

public class SolidObject : GameObject
{
    private Texture _specularMap;

    public SolidObject(VertexData vertexData, Shader shader, Texture diffuse, Texture specular, Material material) 
        : base(vertexData, shader, diffuse)
    {
        _specularMap = specular;
        SetMaterial(material);

        _specularMap.Use(TextureUnit.Texture1);
    }

    private void SetMaterial(Material material)
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

    public override void Draw()
    {
        _specularMap.Use(TextureUnit.Texture1);
        base.Draw();
    }
}
