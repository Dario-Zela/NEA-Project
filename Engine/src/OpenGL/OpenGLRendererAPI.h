#pragma once
#include "Engine/Renderer/RendererAPI.h"

namespace Engine
{
	class OpenGLRendererAPI : public RendererAPI
	{
		virtual void Clear(const glm::vec4& color = glm::vec4(0.0f, 0.0f, 0.0f, 1.0f)) override;
		virtual void Init() override;
		virtual void DrawIndexed(const Ref<VertexArray>& vertexArray, unsigned int count = 0) override;
		virtual void SetViewport(unsigned int x, unsigned int y, unsigned int width, unsigned int height) override;
	};
}