#pragma once
#include "Engine/Renderer/OrthographicCamera.h"
#include "Engine/Renderer/Texture.h"

#undef DrawText

namespace Engine
{
	struct CharacterSheet;

	enum class ENGINE_API Font
	{
		Verdana
	};

	const int SUPPORTED_FONTS = 1;

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

		static void DrawText(const char* text, const glm::vec2& position, const glm::vec2& size, Font font, const glm::vec4& shade = glm::vec4(1.0f), float rotation = 0.0f);
		static void DrawText(const char* text, const glm::vec3& position, const glm::vec2& size, Font font, const glm::vec4& shade = glm::vec4(1.0f), float rotation = 0.0f);

	private:
		static glm::mat4 DrawChar(char c, glm::mat4& positions, const glm::mat4& tranform, const glm::vec2& unitVector, const glm::vec4& shade, float textureIndex, float fontIndex);
		static std::string ReadFontFile(Font font);
		static void CompileCharSheet(const std::string& stringData, CharacterSheet* sheet);
	};
}