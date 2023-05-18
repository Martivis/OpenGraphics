#version 330

struct Material
{
    sampler2D diffuse;
    sampler2D specular;
    float shininess;
};

struct Light
{
    vec3 position;

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};

out vec4 outputColor;

in vec2 texCoord;
in vec3 fragPos;
in vec3 normal;

uniform Light light;
uniform Material material;
uniform vec3 viewPos;

void main()
{
    vec3 ambient = light.ambient * vec3(texture(material.diffuse, texCoord));

    vec3 norm = normalize(normal);
    vec3 lightDir = normalize(light.position - fragPos);
    float diff = max(dot(norm, lightDir), 0.0);
    vec3 diffuse = light.diffuse * diff * vec3(texture(material.diffuse, texCoord));

    vec3 viewDir = normalize(viewPos - fragPos);
    vec3 reflectDir = reflect(-lightDir, norm);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
    vec3 specular = light.specular * spec * vec3(texture(material.specular, texCoord));

    vec3 result = ambient + diffuse + specular;
    outputColor = vec4(result, 1.0);
}