#version 330

out vec4 outputColor;

in vec2 texCoord;
in vec3 fragPos;
in vec3 normal;

uniform sampler2D texture0;
uniform vec3 lightColor;
uniform vec3 lightPos;

void main()
{
    vec3 ambient = lightColor;

    vec3 norm = normalize(normal);
    vec3 lightDir = normalize(lightPos - fragPos);

    // Diffuse
    float diff = max(dot(norm, lightDir), 0.0);
    vec3 diffuse = diff * lightColor;

    vec3 result = (ambient + diff) * vec3(texture(texture0, texCoord));

    outputColor = vec4(result, 1.0);
}