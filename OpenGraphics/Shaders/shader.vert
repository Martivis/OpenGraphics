#version 330 core

layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec3 aNormal;
layout (location = 2) in vec2 aTexCoord;

out vec2 texCoord;
out vec3 fragPos;
out vec3 normal;

uniform mat4 transform;
uniform mat4 view;
uniform mat4 projection;


void main()
{
    fragPos = vec3(vec4(aPosition, 1.0) * transform);
    normal = aNormal * mat3(transpose(inverse(transform)));
    texCoord = aTexCoord;

    gl_Position = vec4(aPosition, 1.0) * transform * view * projection;
}