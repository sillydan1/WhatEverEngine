#version 130

in vec3 vertexPosition;
in vec3 vertexNormal;
in vec2 vertexUV;

out vec3 normal;
out vec2 uv;
out vec3 eyeVec;

uniform mat4 projection_matrix;
uniform mat4 view_matrix;
uniform mat4 model_matrix;

void main(void)
{
    normal = (length(vertexNormal) == 0 ? vec3(0, 0, 0) : normalize((model_matrix * vec4(vertexNormal, 0)).xyz));
    uv = vertexUV;
	vec3 vvertex = vec3((view_matrix * model_matrix) * gl_Vertex);
	eyeVec = -vvertex;
    gl_Position = projection_matrix * view_matrix * model_matrix * vec4(vertexPosition, 1);
}