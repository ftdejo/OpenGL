#version 330 core
out vec4 color;

struct Material
{
    sampler2D diffuse;
    sampler2D specular;
    float shininess;
};

struct Light{
	vec3 ambient;
	vec3 diffuse;
	vec3 specular;
	
    float constant;
    float linear;
    float quadratic;

	float outer;
	float inner;

	vec3 position;
	vec3 direction;
};

uniform Material material;
uniform Light light;
uniform vec3 viewPos;

in vec3 Normal;
in vec3 FragPos;
in vec2 TexCoords;

void main(){
	// 环境光
	vec3 ambient = light.ambient * texture(material.diffuse,TexCoords).xyz;

	//漫反射
	vec3 normal = normalize(Normal);
	vec3 lightDir =  normalize(light.position - FragPos);

	float intensity = clamp((dot(light.direction,-lightDir) - light.outer)/(light.inner - light.outer),0.0f,1.0f);

	float diff = max(dot(normal,lightDir),0);
	vec3 diffuse = light.diffuse * diff * texture(material.diffuse,TexCoords).xyz;
	diffuse *= intensity;
	//镜面反射
	vec3 viewDir = normalize(viewPos - FragPos);
	vec3 reflectDir = normalize(reflect(-lightDir,normal));
	float spec = pow(max(dot(viewDir,reflectDir),0.0),material.shininess);
	//反转vec3 specular = light.specular * spec * (vec3(1.0f)- texture(material.specular,TexCoords).xyz);
	vec3 specular = light.specular * spec * texture(material.specular,TexCoords).xyz;
	specular *= intensity;

	// attenuation
    float distance    = length(light.position - FragPos);
    float attenuation = 1.0 / (light.constant + light.linear * distance + light.quadratic * (distance * distance));    
	ambient  *= attenuation; 
    diffuse   *= attenuation;
    specular *= attenuation; 

	vec3 result = ambient + diffuse + specular;
	color = vec4(result , 1.0f);
}