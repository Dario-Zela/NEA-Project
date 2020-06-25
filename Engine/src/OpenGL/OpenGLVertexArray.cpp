#include "ENPH.h"
#include "OpenGLVertexArray.h"

#include <glad/glad.h>

namespace Engine
{
	static GLenum ShaderDataTypeToGLBaseType(ShaderDataType type)
	{
		switch (type)
		{
		case ShaderDataType::None: EN_CORE_ASSERT(false, "Invalid ShaderDataType"); return GL_NONE;
		case ShaderDataType::VecF2: return GL_FLOAT;
		case ShaderDataType::VecF3: return GL_FLOAT;
		case ShaderDataType::VecF4: return GL_FLOAT;
		case ShaderDataType::VecI2: return GL_INT;
		case ShaderDataType::VecI3: return GL_INT;
		case ShaderDataType::VecI4: return GL_INT;
		case ShaderDataType::MatF2: return GL_FLOAT;
		case ShaderDataType::MatF3: return GL_FLOAT;
		case ShaderDataType::MatF4: return GL_FLOAT;
		case ShaderDataType::MatI2: return GL_INT;
		case ShaderDataType::MatI3: return GL_INT;
		case ShaderDataType::MatI4: return GL_INT;
		case ShaderDataType::Float: return GL_FLOAT;
		case ShaderDataType::Int: return GL_INT;
		case ShaderDataType::Bool: return GL_BOOL;
		}

		EN_CORE_ASSERT(false, "Unknown ShaderDataType");
		return 0;
	}

	OpenGLVertexArray::~OpenGLVertexArray()
	{
		glDeleteVertexArrays(1, &mRendererID);
	}

	OpenGLVertexArray::OpenGLVertexArray()
	{
		glCreateVertexArrays(1, &mRendererID);
	}

	void OpenGLVertexArray::Bind() const
	{
		glBindVertexArray(mRendererID);
	}

	void OpenGLVertexArray::Unbind() const
	{
		glBindVertexArray(0);
	}

	void OpenGLVertexArray::AddVertexBuffer(const Ref<VertexBuffer> vertexBuffer)
	{
		EN_CORE_ASSERT(vertexBuffer->GetLayout().GetElements().size(), "Vertex Buffer has no layout");

		glBindVertexArray(mRendererID);
		vertexBuffer->Bind();

		const auto& layout = vertexBuffer->GetLayout();
		for (const auto& element : layout)
		{
			glEnableVertexAttribArray(mBufferIndex);
			glVertexAttribPointer(mBufferIndex, element.GetElementCount(), ShaderDataTypeToGLBaseType(element.Type),
				element.Normalised ? GL_TRUE : GL_FALSE, layout.GetStride(),
				(const void*)element.Offset);
			mBufferIndex++;
		}

		mVertexBuffers.push_back(vertexBuffer);
	}

	void OpenGLVertexArray::SetIndexBuffer(const Ref<IndexBuffer> indexBuffer)
	{
		glBindVertexArray(mRendererID);
		indexBuffer->Bind();
		mIndexBuffer = indexBuffer;
	}
}