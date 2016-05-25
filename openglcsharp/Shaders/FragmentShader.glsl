#version 130

in vec3 normal;
in vec2 uv;
in vec3 eyeVec;

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

		vec3 N = normalize(normal);
		vec3 L = normalize(light_direction);

		float lambertTerm = dot(N,L);

		vec4 sample = (useTexture ? texture2D(texture, uv) : vec4(1, 1, 1, 1));
		vec3 final_color = vec3(sample.xyz * 0.5);

		if(lambertTerm > 0.0)
		{
			final_color += diffuse * 
						   sample.xyz * 
						   lambertTerm;	
		
			vec3 E = normalize(eyeVec);
			vec3 R = reflect(-L, N);
			float specular = pow( max(dot(R, E), 0.0), 10.7);

			final_color += specular * 0.5;	
		}


		float light = max(0.5, dot(normal, light_direction));
		
		fragment = vec4(final_color, transparency * sample.a);
		//fragment = vec4(light * 1.5 * diffuse * sample.xyz, transparency * sample.a);
	}
	else
	{
		vec4 sample = (useTexture ? texture2D(texture, uv) : vec4(1, 1, 1, 1));
		fragment = vec4(1.0 * diffuse * sample.xyz, transparency * sample.a);
	}
}