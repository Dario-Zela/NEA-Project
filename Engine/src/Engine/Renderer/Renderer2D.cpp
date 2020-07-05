#include "ENPH.h"
#include "Renderer2D.h"

#include "Engine/Renderer/VertexArray.h"
#include "Engine/Renderer/Shader.h"
#include "Engine/Renderer/RenderCommand.h"
#include <glm/gtc/matrix_transform.hpp>

namespace Engine
{
	struct QuadVertex
	{
		glm::vec4 position;
		glm::vec2 texCoord;
		glm::vec4 color;
		float textIndex;
		float textureScale;
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
		Ref<Texture2D> TextureSlots[MaxTextureSlots];
		unsigned int TextureIndex = 1;//Where 0 is the WhiteTexture;
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

		Ref<Texture2D> whiteTex = Texture2D::Create(1, 1);
		unsigned int data = 0xffffffff;
		whiteTex->SetData(&data);
		sStorage.TextureSlots[0] = whiteTex;
	}

	void Renderer2D::Shutdown()
	{
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
		glm::mat4 tranform = glm::translate(glm::mat4(1.0f), position);
		if (rotation != 0.0f)
			tranform = glm::rotate(tranform, glm::radians(rotation), glm::vec3(0, 0, 1));
		tranform = glm::scale(tranform, { size.x, size.y, 1.0f });

		glm::mat4 positions = glm::mat4(-0.5f, -0.5f, 0.0f, 1.0f,
										 0.5f, -0.5f, 0.0f, 1.0f,
										 0.5f,  0.5f, 0.0f, 1.0f,
										-0.5f,  0.5f, 0.0f, 1.0f);
		
		positions = tranform * positions;

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

	void Renderer2D::DrawQuad(const glm::vec2& position, const glm::vec2& size, const Ref<Texture2D>& texture, const glm::vec4& shade, float textureScale, float rotation)
	{
		DrawQuad({ position.x, position.y, 0.0f }, size, texture, shade, textureScale, rotation);
	}

	void Renderer2D::DrawQuad(const glm::vec3& position, const glm::vec2& size, const Ref<Texture2D>& texture, const glm::vec4& shade, float textureScale, float rotation)
	{
		glm::mat4 tranform = glm::translate(glm::mat4(1.0f), position);
		if (rotation != 0.0f)
			tranform = glm::rotate(tranform, glm::radians(rotation), glm::vec3(0, 0, 1));
		tranform = glm::scale(tranform, { size.x, size.y, 1.0f });

		glm::mat4 positions = glm::mat4(-0.5f, -0.5f, 0.0f, 1.0f,
										 0.5f, -0.5f, 0.0f, 1.0f,
										 0.5f,  0.5f, 0.0f, 1.0f,
										-0.5f,  0.5f, 0.0f, 1.0f);

		positions = tranform * positions;

		float textureIndex = 0.0f;

		for (unsigned int i = 0; i < sStorage.TextureIndex; i++)
		{
			if (*sStorage.TextureSlots[i].get() == *texture.get())
			{
				textureIndex = (float)i;
				break;
			}
		}

		if (textureIndex == 0.0f)
		{
			textureIndex = (float)sStorage.TextureIndex;
			sStorage.TextureSlots[sStorage.TextureIndex] = texture;
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

}