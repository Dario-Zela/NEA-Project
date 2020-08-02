#type vertex
#version 330 core
		
layout(location = 0) in vec3 aPosition;
		
uniform mat4 uTransform;
uniform mat4 uViewProjection;

out vec4 vColor;

void main()
{
	gl_Position = uViewProjection * uTransform * vec4(aPosition, 1.0);
	vColor = vec4(aPosition,1.0f);
}

#type fragment
#version 330 core
		
layout(location = 0) out vec4 color;

in vec4 vColor;

void main()
{
	color = vColor;
}