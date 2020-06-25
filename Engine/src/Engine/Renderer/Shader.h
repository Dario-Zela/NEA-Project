#pragma once
#include <string>
#include <unordered_map>
#include <glm/glm.hpp>

namespace Engine 
{
	class ENGINE_API Shader
	{
	public:
		virtual ~Shader() = default;

		virtual void Bind() const = 0;
		virtual void Unbuind() const = 0;

		virtual void SetMat4(const std::string& name, const glm::mat4& value) = 0;
		virtual void SetVecF4(const std::string& name, const glm::vec4& value) = 0;
		virtual void SetVecF3(const std::string& name, const glm::vec3& value) = 0;
		virtual void SetInt(const std::string& name, int value) = 0;
		virtual void SetIntArray(const std::string& name, int* values, unsigned int count) = 0;
		virtual void SetFloat(const std::string& name, float value) = 0;

		virtual const std::string& GetName() const = 0;
		
		static Ref<Shader> Create(const std::string& vertexSrc, const std::string& fragmentSrc, const std::string& name = "Shader");
		static Ref<Shader> Create(const std::string& filePath);
	};

	class ENGINE_API ShaderLibrary
	{
	public:
		void Add(const Ref<Shader>& shader);
		void Add(const std::string& name, const Ref<Shader>& shader);
		Ref<Shader> Load(const std::string& name, const std::string& filePath);
		Ref<Shader> Load(const std::string& filePath);
		Ref<Shader> Get(const std::string& name);
	private:
		std::unordered_map<std::string, Ref<Shader>> mShaders;
	};
}