#pragma once
#include <string>
#include <glm/glm.hpp>
#include "Engine/Renderer/Shader.h"

//To Remove
#include "Engine/Core/Core.h"
typedef unsigned int GLenum;

namespace Engine
{
	class ENGINE_API OpenGLShader : public Shader
	{
	public:
		OpenGLShader(const std::string& filePath);
		OpenGLShader(const std::string& name, const std::string& vertexSrc, const std::string& fragmentSrc);
		virtual ~OpenGLShader();

		inline virtual const std::string& GetName() const override { return mName; }
		virtual void Bind() const override;
		virtual	void Unbind() const override;

		void UploadUniformMat4(const std::string& name, const glm::mat4& matrix);
		void UploadUniformMat3(const std::string& name, const glm::mat3& matrix);
		
		void UploadUniformVecF4(const std::string& name, const glm::vec4& values);
		void UploadUniformVecF3(const std::string& name, const glm::vec3& values);
		void UploadUniformVecF2(const std::string& name, const glm::vec2& values);
		void UploadUniformFloat(const std::string& name, float value);
		
		void UploadUniformInt(const std::string& name, int value);
		void UploadUniformIntArray(const std::string& name, int* values, unsigned int count);

		inline virtual void SetMat4(const std::string& name, const glm::mat4& value) override {	UploadUniformMat4(name, value);}
		inline virtual void SetVecF4(const std::string& name, const glm::vec4& value) override { UploadUniformVecF4(name, value);}
		inline virtual void SetVecF3(const std::string& name, const glm::vec3& value) override { UploadUniformVecF3(name, value);}
		inline virtual void SetInt(const std::string& name, int value) override { UploadUniformInt(name, value); }
		inline virtual void SetFloat(const std::string& name, float value) override { UploadUniformFloat(name, value); }
		inline virtual void SetIntArray(const std::string& name, int* values, unsigned int count) override { UploadUniformIntArray(name, values, count); }

	private:
		std::string ReadFile(const std::string& filePath);
		std::unordered_map<GLenum, std::string> PreProccess(const std::string& shaderSrc);
		void Compile(const std::unordered_map<GLenum, std::string>& shaderSources);
		GLenum ShaderTypeFromString(const std::string& type);

		std::string mName;
		unsigned int mRendererID;
	};
}