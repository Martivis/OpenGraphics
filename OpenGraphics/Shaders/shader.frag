#version 330

out vec4 outputColor;

in vec2 texCoord;

uniform sampler2D texture0;
uniform vec3 lightColor;
void main()
{
    vec3 ambient = lightColor * vec3(texture(texture0, texCoord));

    outputColor = vec4(ambient, 1.0);
}