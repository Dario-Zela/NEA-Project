#pragma once

#include "RendererAPI.h"

namespace Engine
{
	class ENGINE_API RenderCommand
	{
	public:
		inline static void Init() { sRendererAPI->Init(); }
		inline static void SetViewport(unsigned int x, unsigned int y, unsigned int width, unsigned int height) { sRendererAPI->SetViewport(x, y, width, height); };
 		inline static void DrawIndexed(const Ref<VertexArray>& vertexArray, unsigned int count = 0) { sRendererAPI->DrawIndexed(vertexArray, count); }
		inline static void Clear(const glm::vec4& color = glm::vec4(0.0f, 0.0f, 0.0f, 1.0f)) { sRendererAPI->Clear(color); }
	private:
		static Scope<RendererAPI> sRendererAPI;
	};
}