#pragma once
#include "Engine/Renderer/Buffer.h"

namespace Engine
{
	//VERTEX BUFFER//////////////////////////////////////////////////////////////////////////////

	class ENGINE_API OpenGLVertexBuffer : public VertexBuffer
	{
	public:
		OpenGLVertexBuffer(unsigned int size);
		OpenGLVertexBuffer(unsigned int size, float* vertecies);
		virtual ~OpenGLVertexBuffer();

		virtual void Bind() const override;
		virtual void Unbind() const override;
		virtual inline void SetLayout(const BufferLayout& layout) override { mLayout = layout; };
		virtual void SetData(const void* data, unsigned int size) override;

		virtual inline const BufferLayout& GetLayout() const override { return mLayout; };

	private:
		unsigned int mRenderID;
		BufferLayout mLayout;
	};

	//INDEX BUFFER///////////////////////////////////////////////////////////////////////////////

	class ENGINE_API OpenGLIndexBuffer : public IndexBuffer
	{
	public:
		OpenGLIndexBuffer(unsigned int count, unsigned int* indices);
		virtual ~OpenGLIndexBuffer();

		virtual inline unsigned int GetCount() const override { return mCount; }

		virtual void Bind() const override;
		virtual void Unbind() const override;
	private:
		unsigned int mRenderID;
		unsigned int mCount;
	};

}