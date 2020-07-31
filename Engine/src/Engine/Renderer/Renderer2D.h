#pragma once
#include "Engine/Renderer/OrthographicCamera.h"
#include "Engine/Renderer/Texture.h"

namespace Engine
{
	class ENGINE_API Renderer2D
	{
	public:
		static void Init();
		static void Shutdown();

		static void BeginScene(const OrthographicCamera& camera);
		static void EndScene();
		static void Flush();

		static void DrawQuad(const glm::vec2& position, const glm::vec2& size, const glm::vec4& color, float rotation = 0.0f);
		static void DrawQuad(const glm::vec3& position, const glm::vec2& size, const glm::vec4& color, float rotation = 0.0f);
		static void DrawQuad(const glm::vec2& position, const glm::vec2& size, Texture2D& texture, const glm::vec4& shade = glm::vec4(1.0f), float textureScale = 1.0f, float rotation = 0.0f);
		static void DrawQuad(const glm::vec3& position, const glm::vec2& size, Texture2D& texture, const glm::vec4& shade = glm::vec4(1.0f), float textureScale = 1.0f, float rotation = 0.0f);
	};
}