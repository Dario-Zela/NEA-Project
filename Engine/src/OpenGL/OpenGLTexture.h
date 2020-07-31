#pragma once
#include "Engine/Renderer/Texture.h"

namespace Engine
{
	class ENGINE_API OpenGLTexture2D : public Texture2D
	{
	public:
		OpenGLTexture2D(unsigned int width, unsigned int height);
		OpenGLTexture2D(const std::string& path);
		virtual ~OpenGLTexture2D();

		inline virtual unsigned int GetWidth() const override { return mWidth; }
		inline virtual unsigned int GetHeight() const override { return mHeight; }
		virtual void SetData(void* data) override;
		virtual bool operator==(const Texture& other) const override { return mRendererID == ((OpenGLTexture2D&)other).mRendererID; }

		virtual void Bind(unsigned int textureSlot = 0) const override;
	private:
		unsigned int mWidth;
		unsigned int mHeight;
		unsigned int mRendererID;
		std::string mPath;
	};
}