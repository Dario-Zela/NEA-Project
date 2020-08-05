#pragma once
#include "Engine/Renderer/OrthographicCamera.h"
#include "Engine/Renderer/Texture.h"

#undef DrawText

namespace Engine
{
	struct CharacterSheet;

	enum class ENGINE_API Font
	{
		Verdana, TimesNewRoman, FixedDsys
	};

	const int SUPPORTED_FONTS = 3;

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

		static void DrawLine(const glm::vec2& position1, const glm::vec2& position2, float thickness, const glm::vec4& color);
		static void DrawRect(const glm::vec2& position1, const glm::vec2& position2, float thickness, const glm::vec4& color);
	private:
		static void DrawChar(char c, glm::mat4& transform, const glm::vec3& position, const glm::vec2& size, const glm::vec4& shade, float textureIndex, float fontIndex);
		static std::string ReadFontFile(Font font);
		static void CompileCharSheet(const std::string& stringData, CharacterSheet* sheet);
	};
}