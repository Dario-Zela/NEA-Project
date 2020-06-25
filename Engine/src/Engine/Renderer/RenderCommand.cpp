#include "ENPH.h"
#include "RenderCommand.h"

#include "OpenGL/OpenGLRendererAPI.h"

namespace Engine
{
	Scope<RendererAPI> RenderCommand::sRendererAPI = CreateScope<OpenGLRendererAPI>();
}