#pragma once
#include <glm/glm.hpp>
#include "VertexArray.h"

namespace Engine
{
	class RendererAPI
	{
	public:
		enum class API
		{
			None = 0, OpenGL = 1
		};

		virtual void Init() = 0;
		virtual void Clear(const glm::vec4& color = glm::vec4(0.0f, 0.0f, 0.0f, 1.0f)) = 0;
		inline static API GetAPI() { return sAPI; }
		virtual void DrawIndexed(const Ref<VertexArray>& vertexArray, unsigned int count = 0) = 0;
		virtual void SetViewport(unsigned int x, unsigned int y, unsigned int width, unsigned int height) = 0;
	private:
		static API sAPI;
	};
}