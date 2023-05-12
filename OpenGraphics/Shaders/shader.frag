#version 330

out vec4 outputColor;
in vec3 frColor;

void main()
{
    outputColor = vec4(frColor, 1.0);
}