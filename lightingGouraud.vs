#version 330 core
layout (location = 0) in vec3 aPos;
layout (location = 1) in vec3 normal;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

uniform vec3 objectColor;
uniform vec3 lightColor;
uniform vec3 lightPos;  
uniform vec3 viewPos;


out vec3 resultColor;

void main()
{
	gl_Position = projection * view * model * vec4(aPos, 1.0);
	vec3 FragPos = vec3(model * vec4(aPos, 1.0f));
	vec3 Normal = normalize(mat3(transpose(inverse(model))) * normal);

	//specular
	float ambientStrength = 0.1f;
    vec3 ambient = ambientStrength * lightColor;

	//diffuse
	vec3 lightDir = normalize(lightPos - FragPos);
	float diff = max(dot(lightDir,Normal),0.0);
	vec3 diffuse = diff * lightColor;

	//specular
	float specularStrength = 0.5f;
	vec3 viewDir = normalize(viewPos - FragPos);
	vec3 reflecDir = reflect(-lightDir,Normal);
	float spec = pow(max(dot(viewDir,reflecDir),0.0),32);
	vec3 specular = specularStrength*spec * lightColor;

	resultColor = (ambient+diffuse+specular)*objectColor;
}