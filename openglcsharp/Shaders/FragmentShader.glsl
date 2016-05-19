#version 130

in vec3 normal;
in vec2 uv;

out vec4 fragment;

uniform vec3 diffuse;
uniform sampler2D texture;
uniform float transparency;
uniform bool useTexture;
uniform bool uselighting;

void main(void)
{
	if(uselighting == true)
	{
		vec3 light_direction = normalize(vec3(0, 1, 2));
		float light = max(0.5, dot(normal, light_direction));
		vec4 sample = (useTexture ? texture2D(texture, uv) : vec4(1, 1, 1, 1));
		fragment = vec4(light * diffuse * sample.xyz, transparency * sample.a);
	}
	else
	{
		vec4 sample = (useTexture ? texture2D(texture, uv) : vec4(1, 1, 1, 1));
		fragment = vec4(1.0 * diffuse * sample.xyz, transparency * sample.a);
	}
}