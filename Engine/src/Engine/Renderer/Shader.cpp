#include "ENPH.h"
#include "Shader.h"
#include "OpenGL/OpenGLShader.h"
#include "Engine/Renderer/Render.h"

namespace Engine
{
	Ref<Shader> Shader::Create(const std::string& vertexSrc, const std::string& fragmentSrc, const std::string& name)
	{
		switch (Renderer::GetCurrenAPI())
		{
		case RendererAPI::API::None: EN_CORE_ASSERT(false, "No Renderer API has been selected"); return nullptr;
		case RendererAPI::API::OpenGL: return CreateRef<OpenGLShader>(name, vertexSrc, fragmentSrc);
		default:
			break;
		}
		EN_CORE_ASSERT(false, "Unknown RenderAPI");
		return nullptr;
	}

	Ref<Shader> Shader::Create(const std::string& filePath)
	{
		switch (Renderer::GetCurrenAPI())
		{
		case RendererAPI::API::None: EN_CORE_ASSERT(false, "No Renderer API has been selected"); return nullptr;
		case RendererAPI::API::OpenGL: return CreateRef<OpenGLShader>(filePath);
		default:
			break;
		}
		EN_CORE_ASSERT(false, "Unknown RenderAPI");
		return nullptr;
	}

	void ShaderLibrary::Add(const Ref<Shader>& shader)
	{
		auto& name = shader->GetName();
		EN_CORE_ASSERT(mShaders.find(name) == mShaders.end(), "Shader Already Exists");
		mShaders[name] = shader;
	}

	void ShaderLibrary::Add(const std::string& name, const Ref<Shader>& shader)
	{
		EN_CORE_ASSERT(mShaders.find(name) == mShaders.end(), "Shader Already Exists");
		mShaders[name] = shader;
	}

	Ref<Shader> ShaderLibrary::Load(const std::string& name, const std::string& filePath)
	{
		auto shader = Shader::Create(filePath);
		Add(name, shader);
		return shader;
	}

	Ref<Shader> ShaderLibrary::Load(const std::string& filePath)
	{
		auto shader = Shader::Create(filePath);
		Add(shader);
		return shader;
	}

	Ref<Shader> ShaderLibrary::Get(const std::string& name)
	{
		EN_CORE_ASSERT(mShaders.find(name) != mShaders.end(), "Shader Not Found")
		return mShaders[name];
	}
}