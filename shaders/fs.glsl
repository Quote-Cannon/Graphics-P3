#version 330
 
// shader input
in vec2 uv;						// interpolated texture coordinates
in vec3 normal;					// interpolated normal
in vec4 worldPos;
uniform sampler2D pixels;		// texture sampler

// shader output
out vec4 outputColor;

// fragment shader
uniform vec3 lightPos;
void main()
{
	vec3 L = lightPos - worldPos.xyz;
	float dist = L.length();
	L = normalize( L );
	vec3 lightColor = vec3(10, 10, 10);
	vec3 materialColor = texture( pixels, uv).xyz;
	float attenuation = 1.0f / (dist*dist);
    outputColor =  vec4( materialColor * max( 0.0f, dot( L, normal ) ) *
attenuation * lightColor, 1 );

}