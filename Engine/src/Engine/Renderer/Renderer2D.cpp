#include "ENPH.h"
#include "Renderer2D.h"

#include "Engine/Renderer/VertexArray.h"
#include "Engine/Renderer/Shader.h"
#include "Engine/Renderer/RenderCommand.h"
#include <glm/gtc/matrix_transform.hpp>
#include "OpenGL/OpenGLTexture.h"

#include <fstream>

namespace Engine
{
	static std::string FontToText(Font font)
	{
		switch (font)
		{
		case Font::Verdana:
			return "Verdana";
		default:
			EN_CORE_ASSERT(false, "Unknown Font Used");
			break;
		}
	}

	struct QuadVertex
	{
		glm::vec4 position;
		glm::vec2 texCoord;
		glm::vec4 color;
		float textIndex;
		float textureScale;
	};

	struct Character
	{
		char value;
		glm::vec2 texCoords[4];
		glm::vec2 offset;
		float advance;
		glm::vec2 size;
	};

	struct CharacterSheet
	{
		Character characters[97];
	};

	struct Renderer2DStorage
	{
		const unsigned int MaxQuads = 10000;
		const unsigned int MaxVerteces = MaxQuads * 4;
		const unsigned int MaxIndeces= MaxQuads * 6;
		static const unsigned int MaxTextureSlots = 32;

		Ref<VertexArray> QuadVA;
		Ref<VertexBuffer> QuadVB;
		Ref<Shader> Shader;

		unsigned int QuadIndexCount = 0;
		QuadVertex* QuadVertexBufferBase = nullptr;
		QuadVertex* QuadVertexBufferPtr = nullptr;
		Texture2D* TextureSlots[MaxTextureSlots];
		unsigned int TextureIndex = 1;//Where 0 is the WhiteTexture;
		
		Texture2D* FontTex[SUPPORTED_FONTS];
		CharacterSheet FontCharacters[MaxTextureSlots];
	};

	static Renderer2DStorage sStorage;

	void Renderer2D::Init()
	{
		sStorage.QuadVA = VertexArray::Create();
		sStorage.QuadVB = VertexBuffer::Create(sStorage.MaxVerteces * sizeof(QuadVertex));
		{
			BufferLayout layout =
			{
				{ "aPosition", ShaderDataType::VecF4},
				{ "aTexCoord", ShaderDataType::VecF2},
				{ "aColor", ShaderDataType::VecF4},
				{"aTexIndex", ShaderDataType::Float},
				{"aTexScale", ShaderDataType::Float}
			};

			sStorage.QuadVB->SetLayout(layout);
		}

		sStorage.QuadVA->AddVertexBuffer(sStorage.QuadVB);

		sStorage.QuadVertexBufferBase = new QuadVertex[sStorage.MaxVerteces];

		unsigned int* quadIndeces = new uint32_t[sStorage.MaxIndeces];

		unsigned int offset = 0;
		for (unsigned int i = 0; i < sStorage.MaxIndeces;)
		{
			quadIndeces[i++] = offset + 0;
			quadIndeces[i++] = offset + 1;
			quadIndeces[i++] = offset + 2;

			quadIndeces[i++] = offset + 2;
			quadIndeces[i++] = offset + 3;
			quadIndeces[i++] = offset + 0;

			offset += 4;
		}

		Ref<IndexBuffer> QuadIndexBuffer = IndexBuffer::Create(sStorage.MaxIndeces, quadIndeces);
		sStorage.QuadVA->SetIndexBuffer(QuadIndexBuffer);
		delete[] quadIndeces;

		sStorage.Shader = Shader::Create("assets/Shaders/Texture.glsl");
		sStorage.Shader->Bind();

		int samplers[sStorage.MaxTextureSlots];
		for (int i = 0; i < sStorage.MaxTextureSlots; i++)
			samplers[i] = i;

		sStorage.Shader->SetIntArray("uTexture", samplers, sStorage.MaxTextureSlots);

		auto whiteTex = new OpenGLTexture2D(1, 1);
		unsigned int data = 0xffffffff;
		whiteTex->SetData(&data);
		sStorage.TextureSlots[0] = whiteTex;

		for (int i = 0; i < SUPPORTED_FONTS; i++)
		{
			Font font = (Font)i;
			sStorage.FontTex[i] = new OpenGLTexture2D("assets/Fonts/" + FontToText(font) + ".png");
			CompileCharSheet(ReadFontFile(font), &sStorage.FontCharacters[i]);
		}
	}

	void Renderer2D::Shutdown()
	{
		delete sStorage.TextureSlots[0];
		delete[] sStorage.FontTex;
		delete[] sStorage.FontCharacters;
	}

	void Renderer2D::BeginScene(const OrthographicCamera& camera)
	{
		sStorage.Shader->Bind();
		sStorage.Shader->SetMat4("uViewProjection", camera.GetViewProjectionMatrix());

		sStorage.TextureIndex = 1;
		sStorage.QuadIndexCount = 0;
		sStorage.QuadVertexBufferPtr = sStorage.QuadVertexBufferBase;
	}

	void Renderer2D::EndScene()
	{
		unsigned int data = (unsigned char*)sStorage.QuadVertexBufferPtr - (unsigned char*)sStorage.QuadVertexBufferBase;
		sStorage.QuadVB->SetData(sStorage.QuadVertexBufferBase, data);
		Flush();
	}

	void Renderer2D::Flush()
	{
		for (unsigned int i = 0; i < sStorage.TextureIndex; i++)
		{
			sStorage.TextureSlots[i]->Bind(i);
		}
		RenderCommand::DrawIndexed(sStorage.QuadVA, sStorage.QuadIndexCount);
	}

	void Renderer2D::DrawQuad(const glm::vec2& position, const glm::vec2& size, const glm::vec4& color, float rotation)
	{
		DrawQuad({ position.x, position.y, 0.0f }, size, color, rotation);
	}

	void Renderer2D::DrawQuad(const glm::vec3& position, const glm::vec2& size, const glm::vec4& color, float rotation)
	{
		glm::mat4 transform = glm::translate(glm::mat4(1.0f), position);
		if (rotation != 0.0f)
			transform = glm::rotate(transform, glm::radians(rotation), glm::vec3(0, 0, 1));
		transform = glm::scale(transform, { size.x, size.y, 1.0f });

		glm::mat4 positions = glm::mat4(-0.5f, -0.5f, 0.0f, 1.0f,
										 0.5f, -0.5f, 0.0f, 1.0f,
										 0.5f,  0.5f, 0.0f, 1.0f,
										-0.5f,  0.5f, 0.0f, 1.0f);
		
		positions = transform * positions;

		sStorage.QuadVertexBufferPtr->position = positions[0];
		sStorage.QuadVertexBufferPtr->texCoord = { 0.0f, 0.0f };
		sStorage.QuadVertexBufferPtr->color = color;
		sStorage.QuadVertexBufferPtr->textIndex = 0.0f;
		sStorage.QuadVertexBufferPtr->textureScale = 1.0f;
		sStorage.QuadVertexBufferPtr++;

		sStorage.QuadVertexBufferPtr->position = positions[1];
		sStorage.QuadVertexBufferPtr->texCoord = { 1.0f, 0.0f };
		sStorage.QuadVertexBufferPtr->color = color;
		sStorage.QuadVertexBufferPtr->textIndex = 0.0f;
		sStorage.QuadVertexBufferPtr->textureScale = 1.0f;
		sStorage.QuadVertexBufferPtr++;

		sStorage.QuadVertexBufferPtr->position = positions[2];
		sStorage.QuadVertexBufferPtr->texCoord = { 1.0f, 1.0f };
		sStorage.QuadVertexBufferPtr->color = color;
		sStorage.QuadVertexBufferPtr->textIndex = 0.0f;
		sStorage.QuadVertexBufferPtr->textureScale = 1.0f;
		sStorage.QuadVertexBufferPtr++;

		sStorage.QuadVertexBufferPtr->position = positions[3];
		sStorage.QuadVertexBufferPtr->texCoord = { 0.0f, 1.0f };
		sStorage.QuadVertexBufferPtr->color = color;
		sStorage.QuadVertexBufferPtr->textIndex = 0.0f;
		sStorage.QuadVertexBufferPtr->textureScale = 1.0f;
		sStorage.QuadVertexBufferPtr++;

		sStorage.QuadIndexCount += 6;
	}

	void Renderer2D::DrawQuad(const glm::vec2& position, const glm::vec2& size, Texture2D& texture, const glm::vec4& shade, float textureScale, float rotation)
	{
		DrawQuad({ position.x, position.y, 0.0f }, size, texture, shade, textureScale, rotation);
	}

	void Renderer2D::DrawQuad(const glm::vec3& position, const glm::vec2& size, Texture2D& texture, const glm::vec4& shade, float textureScale, float rotation)
	{
		glm::mat4 transform = glm::translate(glm::mat4(1.0f), position);
		if (rotation != 0.0f)
			transform = glm::rotate(transform, glm::radians(rotation), glm::vec3(0, 0, 1));
		transform = glm::scale(transform, { size.x, size.y, 1.0f });

		glm::mat4 positions = glm::mat4(-0.5f, -0.5f, 0.0f, 1.0f,
										 0.5f, -0.5f, 0.0f, 1.0f,
										 0.5f,  0.5f, 0.0f, 1.0f,
										-0.5f,  0.5f, 0.0f, 1.0f);

		positions = transform * positions;

		float textureIndex = 0.0f;

		for (unsigned int i = 0; i < sStorage.TextureIndex; i++)
		{
			if (*sStorage.TextureSlots[i] == texture)
			{
				textureIndex = (float)i;
				break;
			}
		}

		if (textureIndex == 0.0f)
		{
			textureIndex = (float)sStorage.TextureIndex;
			sStorage.TextureSlots[sStorage.TextureIndex] = &texture;
			sStorage.TextureIndex++;
		}

		sStorage.QuadVertexBufferPtr->position = positions[0];
		sStorage.QuadVertexBufferPtr->texCoord = { 0.0f, 0.0f };
		sStorage.QuadVertexBufferPtr->color = shade;
		sStorage.QuadVertexBufferPtr->textIndex = textureIndex;
		sStorage.QuadVertexBufferPtr->textureScale = textureScale;
		sStorage.QuadVertexBufferPtr++;

		sStorage.QuadVertexBufferPtr->position = positions[1];
		sStorage.QuadVertexBufferPtr->texCoord = { 1.0f, 0.0f };
		sStorage.QuadVertexBufferPtr->color = shade;
		sStorage.QuadVertexBufferPtr->textIndex = textureIndex;
		sStorage.QuadVertexBufferPtr->textureScale = textureScale;
		sStorage.QuadVertexBufferPtr++;

		sStorage.QuadVertexBufferPtr->position = positions[2];
		sStorage.QuadVertexBufferPtr->texCoord = { 1.0f, 1.0f };
		sStorage.QuadVertexBufferPtr->color = shade;
		sStorage.QuadVertexBufferPtr->textIndex = textureIndex;
		sStorage.QuadVertexBufferPtr->textureScale = textureScale;
		sStorage.QuadVertexBufferPtr++;

		sStorage.QuadVertexBufferPtr->position = positions[3];
		sStorage.QuadVertexBufferPtr->texCoord = { 0.0f, 1.0f };
		sStorage.QuadVertexBufferPtr->color = shade;
		sStorage.QuadVertexBufferPtr->textIndex = textureIndex;
		sStorage.QuadVertexBufferPtr->textureScale = textureScale;
		sStorage.QuadVertexBufferPtr++;

		sStorage.QuadIndexCount += 6;
	}

	void Renderer2D::DrawText(const char* text, const glm::vec2& position, const glm::vec2& size, Font font, const glm::vec4& shade, float rotation)
	{
		DrawText(text, { position.x, position.y, 0.0f }, size, font, shade, rotation);
	}

	void Renderer2D::DrawText(const char* text, const glm::vec3& position, const glm::vec2& size, Font font, const glm::vec4& shade, float rotation)
	{
		glm::vec2 scaledSize = size / 87.0f;
		glm::mat4 transform = glm::translate(glm::mat4(1.0f), position);
		if (rotation != 0.0f)
			transform = glm::rotate(transform, glm::radians(rotation), glm::vec3(0, 0, 1));
		transform = glm::scale(transform, { scaledSize.x, scaledSize.y, 1.0f });

		glm::vec2 unitVector(1.0f);
		unitVector.x = scaledSize.x * glm::cos(glm::radians(rotation)) - scaledSize.y * glm::sin(glm::radians(rotation));
		unitVector.y = scaledSize.x * glm::sin(glm::radians(rotation)) + scaledSize.y * glm::cos(glm::radians(rotation));

		glm::mat4 positions = glm::mat4(-0.5f, -0.5f, 0.0f, 1.0f,
										 0.5f, -0.5f, 0.0f, 1.0f,
										 0.5f,  0.5f, 0.0f, 1.0f,
										-0.5f,  0.5f, 0.0f, 1.0f);

		//positions = transform * positions;

		float textureIndex = 0.0f;
		float fontIndex = (float)(int)font;

		for (unsigned int i = 0; i < sStorage.TextureIndex; i++)
		{
			if (*sStorage.TextureSlots[i] == *sStorage.FontTex[(int)fontIndex])
			{
				textureIndex = (float)i;
				break;
			}
		}

		if (textureIndex == 0.0f)
		{
			textureIndex = (float)sStorage.TextureIndex;
			sStorage.TextureSlots[sStorage.TextureIndex] = sStorage.FontTex[(int)fontIndex];
			sStorage.TextureIndex++;
		}


		for (int i = 0; i < std::string(text).length(); i++)
		{
			positions = DrawChar(text[i], positions, transform, unitVector, shade, textureIndex, fontIndex);
		}
	}

	glm::mat4 Renderer2D::DrawChar(char c, glm::mat4& positions, const glm::mat4& transform, const glm::vec2& unitVector, const glm::vec4& shade, float textureIndex, float fontIndex)
	{
		Character data = Character();
		auto fontData = sStorage.FontCharacters[(int)fontIndex];

		for (auto font : fontData.characters)
		{
			if (c == font.value)
			{
				data = font;
				break;
			}
		}

		if (data.value == Character().value)
		{
			return positions;
		}

		glm::mat4 positionCopy(positions);
		glm::mat4 transformation = glm::scale(transform, glm::vec3(data.size, 0.0f));
		positionCopy = transformation * positions;

		glm::vec4 offset = transformation * glm::vec4(data.offset, 0.0f, 0.0f);

		sStorage.QuadVertexBufferPtr->position = positionCopy[0] + offset;
		sStorage.QuadVertexBufferPtr->texCoord = data.texCoords[0];
		sStorage.QuadVertexBufferPtr->color = shade;
		sStorage.QuadVertexBufferPtr->textIndex = textureIndex;
		sStorage.QuadVertexBufferPtr->textureScale = 1.0f;
		sStorage.QuadVertexBufferPtr++;

		sStorage.QuadVertexBufferPtr->position = positionCopy[1] + offset;
		sStorage.QuadVertexBufferPtr->texCoord = data.texCoords[1];
		sStorage.QuadVertexBufferPtr->color = shade;
		sStorage.QuadVertexBufferPtr->textIndex = textureIndex;
		sStorage.QuadVertexBufferPtr->textureScale = 1.0f;
		sStorage.QuadVertexBufferPtr++;

		sStorage.QuadVertexBufferPtr->position = positionCopy[2] + offset;
		sStorage.QuadVertexBufferPtr->texCoord = data.texCoords[2];
		sStorage.QuadVertexBufferPtr->color = shade;
		sStorage.QuadVertexBufferPtr->textIndex = textureIndex;
		sStorage.QuadVertexBufferPtr->textureScale = 1.0f;
		sStorage.QuadVertexBufferPtr++;

		sStorage.QuadVertexBufferPtr->position = positionCopy[3] + offset;
		sStorage.QuadVertexBufferPtr->texCoord = data.texCoords[3];
		sStorage.QuadVertexBufferPtr->color = shade;
		sStorage.QuadVertexBufferPtr->textIndex = textureIndex;
		sStorage.QuadVertexBufferPtr->textureScale = 1.0f;
		sStorage.QuadVertexBufferPtr++;

		sStorage.QuadIndexCount += 6;

		glm::vec4 translation = transformation * glm::vec4(data.advance, 0.0f, 0.0f, 0.0f);
		auto test = glm::translate(positions, glm::vec3(translation.x, translation.y, translation.z));
		if(positions == test)
			return positions;
		return test;
	}

	std::string Renderer2D::ReadFontFile(Font font)
	{
		std::string result;
		std::ifstream in("assets/Fonts/" + FontToText(font) + ".fnt", std::ios::in | std::ios::binary);
		if (in)
		{
			in.seekg(0, std::ios::end);
			if (in.tellg() != -1)
			{
				result.resize(in.tellg());
				in.seekg(0, std::ios::beg);
				in.read(&result[0], result.size());
				in.close();
			}
			else
			{
				EN_CORE_ERROR("Couldn't read the font {0}", FontToText(font));
			}
		}
		else
		{
			EN_CORE_ERROR("Couldn't open the font file at {0}", "assets/Fonts/" + FontToText(font) + ".fnt");
		}
		return result;
	}

	void Renderer2D::CompileCharSheet(const std::string& stringData, CharacterSheet* sheet)
	{
		size_t position = 0;
		int chars = 0;
		while (position != std::string::npos && chars != 97)
		{
			size_t AttributePosition = stringData.find_first_of(",", position);
			sheet->characters[chars].value = std::stoi(stringData.substr(position, AttributePosition));
			position = AttributePosition + 1;

			AttributePosition = stringData.find_first_of(",", position);
			float x = (float)std::stoi(stringData.substr(position, AttributePosition)) / 512.0f;
			position = AttributePosition + 1;

			AttributePosition = stringData.find_first_of(",", position);
			float y = (float)std::stoi(stringData.substr(position, AttributePosition)) / 512.0f;
			position = AttributePosition + 1;

			AttributePosition = stringData.find_first_of(",", position);
			float width = (float)std::stoi(stringData.substr(position, AttributePosition)) / 512.0f;
			position = AttributePosition + 1;

			AttributePosition = stringData.find_first_of(",", position);
			float height = (float)std::stoi(stringData.substr(position, AttributePosition)) / 512.0f;
			position = AttributePosition + 1;

			sheet->characters[chars].texCoords[0] = glm::vec2(x, 1-( y + height));
			sheet->characters[chars].texCoords[1] = glm::vec2(x + width, 1 - (y + height));
			sheet->characters[chars].texCoords[2] = glm::vec2(x + width, 1 - y);
			sheet->characters[chars].texCoords[3] = glm::vec2(x, 1 - y);

			sheet->characters[chars].size = glm::vec2(width, height);

			AttributePosition = stringData.find_first_of(",", position);
			float xoffset = (float)std::stoi(stringData.substr(position, AttributePosition));
			xoffset /= 512.0f;
			position = AttributePosition + 1;

			AttributePosition = stringData.find_first_of(",", position);
			float yoffset = (float)std::stoi(stringData.substr(position, AttributePosition));
			yoffset /= 512.0f;
			position = AttributePosition + 1;

			sheet->characters[chars].offset = glm::vec2(xoffset, yoffset);

			AttributePosition = stringData.find_first_of(",", position);
			sheet->characters[chars].advance = std::stoi(stringData.substr(position, AttributePosition));
			sheet->characters[chars].advance /= 512.0f;
			position = AttributePosition + 1;

			chars++;
		}
	}

}