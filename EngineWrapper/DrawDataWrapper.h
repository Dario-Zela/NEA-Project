#pragma once

namespace Wrapper
{
	public enum ShaderDataType
	{
		None = 0, VecF2, VecF3, VecF4,
		VecI2, VecI3, VecI4,
		MatF2, MatF3, MatF4,
		MatI2, MatI3, MatI4,
		Bool, Float, Int
	};

	public struct ShaderData
	{
		static unsigned int Size(ShaderDataType type) { return Engine::ShaderDataTypeSize((Engine::ShaderDataType)(int)type); }
	};

	public ref struct BufferElement : public ManagedObject<Engine::BufferElement>
	{
		BufferElement(Engine::BufferElement* element) : ManagedObject(element) {}
		BufferElement() : ManagedObject(new Engine::BufferElement()) {}
		BufferElement(String^ name, ShaderDataType type) : ManagedObject(new Engine::BufferElement(stringsToCStrings(name), (Engine::ShaderDataType)(int)type)) {}
		BufferElement(String^ name, ShaderDataType type, bool normalised) : ManagedObject(new Engine::BufferElement(stringsToCStrings(name), (Engine::ShaderDataType)(int)type, normalised)) {}

		unsigned int GetElementCount() { return mInstance->GetElementCount(); }

		String^ GetName() { return gcnew String(mInstance->Name.c_str()); }
		ShaderDataType GetType() { return (ShaderDataType)(int)mInstance->Type; }
		unsigned int GetSize() { return mInstance->Size; }
		unsigned int GetOffset() { return mInstance->Offset; }
		bool GetIsNormalised() { return mInstance->Normalised; }
	};

	public ref class BufferLayout : ManagedObject<Engine::BufferLayout>
	{
	public:
		BufferLayout(System::Collections::Generic::List<BufferElement^> layout) : ManagedObject(nullptr)
		{
			std::vector<Engine::BufferElement> layoutCpp;
			for each (auto element in layout)
			{
				layoutCpp.push_back(*element->GetInstance());
			}
			mInstance = new Engine::BufferLayout(layoutCpp);
		}
		BufferLayout() : ManagedObject(new Engine::BufferLayout()) {}
		BufferLayout(Engine::BufferLayout* layout) : ManagedObject(layout) {}

		System::Collections::Generic::List<BufferElement^>^ GetElements()
		{
			System::Collections::Generic::List<BufferElement^> elements;
			auto elementsCpp = mInstance->GetElements();

			for (auto element : elementsCpp)
			{
				elements.Add(gcnew BufferElement(&element));
			}

			return elements.GetRange(0, elements.Count);
		}
		unsigned int GetStride() { return mInstance->GetStride(); }
	};

	public ref class VertexBuffer : ManagedObject<Engine::VertexBuffer>
	{
	public:
		VertexBuffer(Engine::VertexBuffer* buffer) : ManagedObject(buffer) {}
		VertexBuffer(unsigned int size, float* vertecies) : ManagedObject(Engine::VertexBuffer::Create(size, vertecies).get()) {}
		VertexBuffer(unsigned int size) : ManagedObject(Engine::VertexBuffer::Create(size).get()) {}

		void Bind() { mInstance->Bind(); }
		void Unbind() { mInstance->Unbind(); }

		BufferLayout^ GetLayout() { Engine::BufferLayout layout = mInstance->GetLayout(); return gcnew BufferLayout(&layout); }
		void SetLayout(BufferLayout layout) { return mInstance->SetLayout(*layout.GetInstance()); }
		void SetData(const void* data, unsigned int size) { return mInstance->SetData(data, size); }
	};

	public ref class IndexBuffer : ManagedObject<Engine::IndexBuffer>
	{
	public:
		IndexBuffer(unsigned int count, unsigned int* indicies) : ManagedObject(Engine::IndexBuffer::Create(count, indicies).get()) {}
		IndexBuffer(Engine::IndexBuffer* buffer) : ManagedObject(buffer) {}

		void Bind() { mInstance->Bind(); }
		void Unbind() { mInstance->Unbind(); }

		unsigned int GetCount() { return mInstance->GetCount(); }
	};

	public ref class VertexArray : ManagedObject<Engine::VertexArray>
	{
	public:
		VertexArray() : ManagedObject(Engine::VertexArray::Create().get()) {}

		void Bind() { mInstance->Bind(); }
		void Unbind() { mInstance->Unbind(); }

		void AddVertexBuffer(VertexBuffer vertexBuffer) { mInstance->AddVertexBuffer(std::shared_ptr<Engine::VertexBuffer>(vertexBuffer.GetInstance())); }
		void SetIndexBuffer(IndexBuffer indexBuffer) { mInstance->SetIndexBuffer(std::shared_ptr<Engine::IndexBuffer>(indexBuffer.GetInstance())); }

		System::Collections::Generic::List<VertexBuffer^>^ GetVertexBuffers() 
		{
			auto buffersCpp = mInstance->GetVertexBuffers();
			System::Collections::Generic::List<VertexBuffer^> buffers;

			for (auto buffer : buffersCpp)
			{
				buffers.Add(gcnew VertexBuffer(buffer.get()));
			}

			return buffers.GetRange(0, buffers.Count);
		}
		
		IndexBuffer^ GetIndexBuffer() 
		{ 
			Engine::IndexBuffer* buffer = mInstance->GetIndexBuffer().get();
			return gcnew IndexBuffer(buffer);
		}
	};

	public ref struct Vec3 : ManagedObject<glm::vec3>
	{
		Vec3(glm::vec3* vec) : ManagedObject(vec) {}
		Vec3() : ManagedObject(new glm::vec3()) {}
		Vec3(float value) : ManagedObject(new glm::vec3(value)) {}
		Vec3(float x, float y, float z) : ManagedObject(new glm::vec3(x, y, z)) {}

		static Vec3^ operator +(Vec3 right, Vec3 left) { return gcnew Vec3(&(*right.GetInstance() + *left.GetInstance())); }
		static Vec3^ operator -(Vec3 right, Vec3 left) { return gcnew Vec3(&(*right.GetInstance() - *left.GetInstance())); }
		static Vec3^ operator *(Vec3 right, Vec3 left) { return gcnew Vec3(&(*right.GetInstance() * *left.GetInstance())); }
		static Vec3^ operator /(Vec3 right, Vec3 left) { return gcnew Vec3(&(*right.GetInstance() / *left.GetInstance())); }
	};

	public ref struct Vec4 : ManagedObject<glm::vec4>
	{
		Vec4(glm::vec4* vec) : ManagedObject(vec) {}
		Vec4() : ManagedObject(new glm::vec4()) {}
		Vec4(float value) : ManagedObject(new glm::vec4(value)) {}
		Vec4(Vec3 vec, float w) : ManagedObject(new glm::vec4(*vec.GetInstance(), w)) {}
		Vec4(float x, float y, float z, float w) : ManagedObject(new glm::vec4(x, y, z, w)) {}

		static Vec4^ operator +(Vec4 right, Vec4 left) { return gcnew Vec4(&(*right.GetInstance() + *left.GetInstance())); }
		static Vec4^ operator -(Vec4 right, Vec4 left) { return gcnew Vec4(&(*right.GetInstance() - *left.GetInstance())); }
		static Vec4^ operator *(Vec4 right, Vec4 left) { return gcnew Vec4(&(*right.GetInstance() * *left.GetInstance())); }
		static Vec4^ operator /(Vec4 right, Vec4 left) { return gcnew Vec4(&(*right.GetInstance() / *left.GetInstance())); }
	};

	public ref struct Mat4 : ManagedObject<glm::mat4>
	{
		Mat4(glm::mat4* mat) : ManagedObject(mat) {}
		Mat4() : ManagedObject(new glm::mat4()) {}
		Mat4(float value) : ManagedObject(new glm::mat4(value)) {}
		Mat4(float x1, float y1, float z1, float w1,
			float x2, float y2, float z2, float w2,
			float x3, float y3, float z3, float w3,
			float x4, float y4, float z4, float w4) : ManagedObject(new glm::mat4(x1, y1, z1, w1, x2, y2, z2, w2, x3, y3, z3, w3, x4, y4, z4, w4)) {}
		Mat4(Vec4 row1, Vec4 row2, Vec4 row3, Vec4 row4) :
			ManagedObject(new glm::mat4(*row1.GetInstance(), *row2.GetInstance(), *row3.GetInstance(), *row4.GetInstance())) {}

		static Mat4^ operator +(Mat4 right, Mat4 left) { return gcnew Mat4(&(*right.GetInstance() + *left.GetInstance())); }
		static Mat4^ operator -(Mat4 right, Mat4 left) { return gcnew Mat4(&(*right.GetInstance() - *left.GetInstance())); }
		static Mat4^ operator *(Mat4 right, Mat4 left) { return gcnew Mat4(&(*right.GetInstance() * *left.GetInstance())); }
		static Mat4^ operator /(Mat4 right, Mat4 left) { return gcnew Mat4(&(*right.GetInstance() / *left.GetInstance())); }
	};

	public ref class Shader : ManagedObject<Engine::Shader>
	{
		Shader(String^ filepath) : ManagedObject(Engine::Shader::Create(stringsToCStrings(filepath)).get()) {}
		Shader(String^ vertexSource, String^ fragmentSource, String^ name) : 
			ManagedObject(Engine::Shader::Create(stringsToCStrings(vertexSource), stringsToCStrings(fragmentSource), stringsToCStrings(name)).get()) {}
		Shader(String^ vertexSource, String^ fragmentSource) :
			ManagedObject(Engine::Shader::Create(stringsToCStrings(vertexSource), stringsToCStrings(fragmentSource)).get()) {}

		void Bind() { mInstance->Bind(); }
		void Unbind() { mInstance->Unbind(); }

		void SetMat4(String^ name, Mat4 value) { mInstance->SetMat4(stringsToCStrings(name), *value.GetInstance()); }
		void SetVecF4(String^ name, Vec4 value) { mInstance->SetVecF4(stringsToCStrings(name), *value.GetInstance()); }
		void SetVecF3(String^ name, Vec3 value) { mInstance->SetVecF3(stringsToCStrings(name), *value.GetInstance()); }
		void SetInt(String^ name, int value) { mInstance->SetInt(stringsToCStrings(name), value); }
		void SetIntArray(String^ name, int* values, unsigned int count) { mInstance->SetIntArray(stringsToCStrings(name), values, count); }
		void SetFloat(String^ name, float value) { mInstance->SetFloat(stringsToCStrings(name), value); }

		const String^ GetName() { return gcnew String(mInstance->GetName().c_str()); }
	};

}