#include "ENPH.h"
#include "Texture.h"

#include "OpenGL/OpenGLTexture.h"
#include "Render.h"

namespace Engine
{
	Ref<Texture2D> Texture2D::Create(const std::string& path)
	{
		switch (Renderer::GetCurrenAPI())
		{
		case RendererAPI::API::None: EN_CORE_ASSERT(false, "No Renderer API has been selected"); return nullptr;
		case RendererAPI::API::OpenGL: return CreateRef<OpenGLTexture2D>(path);
		default:
			break;
		}
		EN_CORE_ASSERT(false, "Unknown RenderAPI");
		return nullptr;
	}

	Ref<Texture2D> Texture2D::Create(unsigned int width, unsigned int height)
	{
		switch (Renderer::GetCurrenAPI())
		{
		case RendererAPI::API::None: EN_CORE_ASSERT(false, "No Renderer API has been selected"); return nullptr;
		case RendererAPI::API::OpenGL: return CreateRef<OpenGLTexture2D>(width, height);
		default:
			break;
		}
		EN_CORE_ASSERT(false, "Unknown RenderAPI");
		return nullptr;
	}
}