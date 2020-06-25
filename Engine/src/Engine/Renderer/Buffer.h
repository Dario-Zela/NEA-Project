#pragma once

namespace Engine
{
	enum class ENGINE_API ShaderDataType
	{
		None = 0, VecF2, VecF3, VecF4,
		VecI2, VecI3, VecI4,
		MatF2, MatF3, MatF4,
		MatI2, MatI3, MatI4,
		Bool, Float, Int
	};

	static unsigned int ShaderDataTypeSize(ShaderDataType type)
	{
		switch (type)
		{
		case ShaderDataType::None: EN_CORE_ASSERT(false, "Invalid ShaderDataType"); return 0;
		case ShaderDataType::VecF2: return 8;
		case ShaderDataType::VecF3: return 12;
		case ShaderDataType::VecF4: return 16;
		case ShaderDataType::VecI2 : return 8;
		case ShaderDataType::VecI3: return 12;
		case ShaderDataType::VecI4: return 18;
		case ShaderDataType::MatF2 : return 16;
		case ShaderDataType::MatF3: return 36;
		case ShaderDataType::MatF4: return 64;
		case ShaderDataType::MatI2: return 16;
		case ShaderDataType::MatI3: return 36;
		case ShaderDataType::MatI4: return 64;
		case ShaderDataType::Float: return 4;
		case ShaderDataType::Int: return 4;
		case ShaderDataType::Bool: return 1;
		}

		EN_CORE_ASSERT(false, "Unknown ShaderDataType");
		return 0;
	}

	struct ENGINE_API BufferElement
	{
		std::string Name;
		ShaderDataType Type;
		unsigned int Size;
		size_t Offset;
		bool Normalised;

		BufferElement()
			:BufferElement("Element", ShaderDataType::None) { }

		BufferElement(const std::string name, ShaderDataType type)
			: BufferElement(name, type, false) { }

		BufferElement(const std::string name, ShaderDataType type, bool normalised)
			:Name(name), Type(type), Size(ShaderDataTypeSize(type)), Offset(0), Normalised(normalised) { }


		unsigned int GetElementCount() const
		{
			switch (Type)
			{
			case ShaderDataType::None: EN_CORE_ASSERT(false, "Invalid ShaderDataType"); return 0;
			case ShaderDataType::VecF2: return 2;
			case ShaderDataType::VecF3: return 3;
			case ShaderDataType::VecF4: return 4;
			case ShaderDataType::VecI2: return 2;
			case ShaderDataType::VecI3: return 3;
			case ShaderDataType::VecI4: return 4;
			case ShaderDataType::MatF2: return 4;
			case ShaderDataType::MatF3: return 8;
			case ShaderDataType::MatF4: return 16;
			case ShaderDataType::MatI2: return 4;
			case ShaderDataType::MatI3: return 8;
			case ShaderDataType::MatI4: return 16;
			case ShaderDataType::Int: return 1;
			case ShaderDataType::Float: return 1;
			case ShaderDataType::Bool: return 1;
			}

			EN_CORE_ASSERT(false, "Unknown ShaderDataType");
			return 0;
		}
	};

	class ENGINE_API BufferLayout
	{
	public:
		BufferLayout(const std::initializer_list<BufferElement>& layout)
			:mElements(layout) 
		{
			CalculateOffsetAndStride();
		}
		BufferLayout()
			:BufferLayout({ }) { }

		std::vector<BufferElement>::iterator begin() { return mElements.begin(); }
		std::vector<BufferElement>::iterator end() { return mElements.end(); }
		std::vector<BufferElement>::const_iterator begin() const { return mElements.begin(); }
		std::vector<BufferElement>::const_iterator end() const { return mElements.end(); }

		inline const unsigned int GetStride() const { return mStride; }
		inline const std::vector<BufferElement> GetElements() const { return mElements; }
	private:
		void CalculateOffsetAndStride() 
		{
			size_t offset = 0;
			mStride = 0;
			for (auto& element : mElements)
			{
				element.Offset = offset;
				offset += element.Size;
				mStride += element.Size;
			}
		}

		std::vector<BufferElement> mElements;
		unsigned int mStride = 0;
	};

	class ENGINE_API VertexBuffer
	{
	public:
		virtual ~VertexBuffer() = default;

		virtual void Bind() const = 0;
		virtual void Unbind() const = 0;

		virtual const BufferLayout& GetLayout() const = 0;
		virtual void SetLayout(const BufferLayout& layout) = 0;
		virtual void SetData(const void* data, unsigned int size) = 0;

		static Ref<VertexBuffer> Create(unsigned int size, float* vertecies);
		static Ref<VertexBuffer> Create(unsigned int size);
	};

	class ENGINE_API IndexBuffer
	{
	public:
		virtual ~IndexBuffer() = default;

		virtual void Bind() const = 0;
		virtual void Unbind() const = 0;

		virtual unsigned int GetCount() const = 0;

		static Ref<IndexBuffer> Create(unsigned int count, unsigned int* indices);
	};
}