#include "ENPH.h"
#include "OpenGLTexture.h"
#include <glad/glad.h>
#include <stb_image.h>

namespace Engine 
{
	OpenGLTexture2D::OpenGLTexture2D(unsigned int width, unsigned int height)
		:mWidth(width),	mHeight(height)
	{
		glCreateTextures(GL_TEXTURE_2D, 1, &mRendererID);
		glTextureStorage2D(mRendererID, 1, GL_RGBA8, mWidth, mHeight);

		glTextureParameteri(mRendererID, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
		glTextureParameteri(mRendererID, GL_TEXTURE_MAG_FILTER, GL_NEAREST);
	}

	OpenGLTexture2D::OpenGLTexture2D(const std::string& path)
		:mPath(path)
	{
		int width, height, channels;
		stbi_set_flip_vertically_on_load(1);
		stbi_uc* data = stbi_load(path.c_str(), &width, &height, &channels, 0);
		EN_CORE_ASSERT(data, "Failed to load texture");
		mWidth = width;
		mHeight = height;

		GLenum internalFormat = 0, dataFormat;
		if (channels == 4)
		{
			internalFormat = GL_RGBA8;
			dataFormat = GL_RGBA;
		}
		if (channels == 3)
		{
			internalFormat = GL_RGB8;
			dataFormat = GL_BGR;
		}

		EN_CORE_ASSERT(internalFormat, "Failed to recognise format");

		glCreateTextures(GL_TEXTURE_2D, 1, &mRendererID);
		glTextureStorage2D(mRendererID, 1, internalFormat, mWidth, mHeight);

		glTextureParameteri(mRendererID, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
		glTextureParameteri(mRendererID, GL_TEXTURE_MAG_FILTER, GL_NEAREST);

		glTextureSubImage2D(mRendererID, 0, 0, 0, mWidth, mHeight, dataFormat, GL_UNSIGNED_BYTE, data);

		stbi_image_free(data);
	}

	OpenGLTexture2D::~OpenGLTexture2D()
	{
		glDeleteTextures(1, &mRendererID);
	}

	void OpenGLTexture2D::SetData(void* data)
	{
		glTextureSubImage2D(mRendererID, 0, 0, 0, mWidth, mHeight, GL_RGBA, GL_UNSIGNED_INT, data);
	}

	void OpenGLTexture2D::Bind(unsigned int textureSlot) const
	{
		glBindTextureUnit(textureSlot, mRendererID);
	}
}