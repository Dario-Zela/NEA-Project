#include "ENPH.h"
#include "VertexArray.h"
#include "OpenGL/OpenGLVertexArray.h"
#include "Engine/Renderer/Render.h"

namespace Engine
{
	Ref<VertexArray> VertexArray::Create()
	{
		switch (Renderer::GetCurrenAPI())
		{
		case RendererAPI::API::None: EN_CORE_ASSERT(false, "No Renderer API has been selected"); return nullptr;
		case RendererAPI::API::OpenGL: return CreateRef<OpenGLVertexArray>();
		default:
			break;
		}
		EN_CORE_ASSERT(false, "Unknown RenderAPI");
		return nullptr;
	}
}