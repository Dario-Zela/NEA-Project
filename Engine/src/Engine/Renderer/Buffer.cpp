#include "ENPH.h"
#include "Buffer.h"

#include "OpenGL/OpenGLBuffer.h"
#include "Engine/Renderer/Render.h"

namespace Engine
{
	Ref<VertexBuffer> VertexBuffer::Create(unsigned int size)
	{
		switch (Renderer::GetCurrenAPI())
		{
		case RendererAPI::API::None: EN_CORE_ASSERT(false, "No Renderer API has been selected"); return nullptr;
		case RendererAPI::API::OpenGL: return CreateRef<OpenGLVertexBuffer>(size);
		default:
			break;
		}
		EN_CORE_ASSERT(false, "Unknown RenderAPI");
		return nullptr;
	}

	Ref<VertexBuffer> VertexBuffer::Create(unsigned int size, float* vertecies)
	{
		switch (Renderer::GetCurrenAPI())
		{
		case RendererAPI::API::None: EN_CORE_ASSERT(false, "No Renderer API has been selected"); return nullptr;
		case RendererAPI::API::OpenGL: return CreateRef<OpenGLVertexBuffer>(size, vertecies);
		default:
			break;
		}
		EN_CORE_ASSERT(false, "Unknown RenderAPI");
		return nullptr;
	}

	Ref<IndexBuffer> IndexBuffer::Create(unsigned int count, unsigned int* indices)
	{
		switch (Renderer::GetCurrenAPI())
		{
		case RendererAPI::API::None: EN_CORE_ASSERT(false, "No Renderer API has been selected"); return nullptr;
		case RendererAPI::API::OpenGL: return CreateRef<OpenGLIndexBuffer>(count, indices);
		default:
			break;
	}
		EN_CORE_ASSERT(false, "Unknown RenderAPI");
		return nullptr;
	}

}