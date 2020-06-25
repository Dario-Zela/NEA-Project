#pragma once
#include "Engine/Renderer/VertexArray.h"

namespace Engine
{
	class OpenGLVertexArray : public VertexArray
	{
	public:
		OpenGLVertexArray();
		virtual ~OpenGLVertexArray();

		virtual void Bind() const override;
		virtual void Unbind() const override;

		virtual void AddVertexBuffer(const Ref<VertexBuffer> vertexBuffer) override;
		virtual void SetIndexBuffer(const Ref<IndexBuffer> indexBuffer) override;

		virtual inline const std::vector<Ref<VertexBuffer>>& GetVertexBuffers() const override {return mVertexBuffers;}
		virtual inline const Ref<IndexBuffer>& GetIndexBuffer() const override { return mIndexBuffer; }

	private:
		std::vector<Ref<VertexBuffer>> mVertexBuffers;
		Ref<IndexBuffer> mIndexBuffer;

		unsigned int mBufferIndex = 0;
		unsigned int mRendererID;
	};
}