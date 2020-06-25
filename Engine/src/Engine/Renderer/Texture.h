#pragma once
#include "Engine/Core/Core.h"
#include <string>

namespace Engine
{
	class ENGINE_API Texture
	{
	public:
		~Texture() = default;
		virtual unsigned int GetWidth() const = 0;
		virtual unsigned int GetHeight() const = 0;
		virtual void SetData(void* data) = 0;
		virtual bool operator==(const Texture& other) const = 0;

		virtual void Bind(unsigned int textureSlot = 0) const = 0;
	};

	class ENGINE_API Texture2D : public Texture
	{
	public:
		static Ref<Texture2D> Create(const std::string& path);
		static Ref<Texture2D> Create(unsigned int width, unsigned int height);
	};
}