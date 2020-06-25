#include "ENPH.h"
#include "OpenGLShader.h"

#include <fstream>
#include <glad/glad.h>
#include <glm/gtc/type_ptr.hpp>

namespace Engine
{
	OpenGLShader::OpenGLShader(const std::string& filePath)
	{
		std::string shaderSrc = ReadFile(filePath);
		auto shaderSources = PreProccess(shaderSrc);
		Compile(shaderSources);
		
		//Extracts name from path
		auto lastSlash = filePath.find_last_of("/\\");
		lastSlash = lastSlash == std::string::npos ? 0 : lastSlash + 1;
		auto lastDot = filePath.rfind('.');
		auto count = lastDot == std::string::npos ? filePath.size() - lastSlash : lastDot - lastSlash;
		mName = filePath.substr(lastSlash, count);
	}

	OpenGLShader::OpenGLShader(const std::string& name, const std::string& vertexSrc, const std::string& fragmentSrc)
		:mName(name)
	{
		std::unordered_map<GLenum, std::string> sources;
		sources[GL_VERTEX_SHADER] = vertexSrc;
		sources[GL_FRAGMENT_SHADER] = fragmentSrc;
		Compile(sources);
	}

	OpenGLShader::~OpenGLShader()
	{
		glDeleteProgram(mRendererID);
	}

	void OpenGLShader::Bind() const
	{
		glUseProgram(mRendererID);
	}

	void OpenGLShader::Unbuind() const
	{
		glUseProgram(0);
	}

	void OpenGLShader::UploadUniformMat4(const std::string& name, const glm::mat4& matrix)
	{
		GLint location = glGetUniformLocation(mRendererID, name.c_str());
		glUniformMatrix4fv(location, 1, GL_FALSE, glm::value_ptr(matrix));
	}

	void OpenGLShader::UploadUniformMat3(const std::string& name, const glm::mat3& matrix)
	{
		GLint location = glGetUniformLocation(mRendererID, name.c_str());
		glUniformMatrix3fv(location, 1, GL_FALSE, glm::value_ptr(matrix));
	}

	void OpenGLShader::UploadUniformVecF4(const std::string& name, const glm::vec4& values)
	{
		GLint location = glGetUniformLocation(mRendererID, name.c_str());
		glUniform4f(location, values.x, values.y, values.z, values.w);
	}

	void OpenGLShader::UploadUniformVecF3(const std::string& name, const glm::vec3& values)
	{
		GLint location = glGetUniformLocation(mRendererID, name.c_str());
		glUniform3f(location, values.x, values.y, values.z);
	}

	void OpenGLShader::UploadUniformVecF2(const std::string& name, const glm::vec2& values)
	{
		GLint location = glGetUniformLocation(mRendererID, name.c_str());
		glUniform2f(location, values.x, values.y);
	}

	void OpenGLShader::UploadUniformFloat(const std::string& name, float value)
	{
		GLint location = glGetUniformLocation(mRendererID, name.c_str());
		glUniform1f(location, value);
	}

	void OpenGLShader::UploadUniformInt(const std::string& name, int value)
	{
		GLint location = glGetUniformLocation(mRendererID, name.c_str());
		glUniform1i(location, value);
	}

	void OpenGLShader::UploadUniformIntArray(const std::string& name, int* values, unsigned int count)
	{
		GLint location = glGetUniformLocation(mRendererID, name.c_str());
		glUniform1iv(location, count, values);
	}

	std::string OpenGLShader::ReadFile(const std::string& filePath)
	{
		std::string result;
		std::ifstream in(filePath, std::ios::in | std::ios::binary);
		if (in)
		{
			in.seekg(0, std::ios::end);
			if (in.tellg() != -1)
			{
				result.resize(in.tellg());
				in.seekg(0, std::ios::beg);
				in.read(&result[0], result.size());
				in.close();
			}
			else
			{
				EN_CORE_ERROR("Couldn't read from the file at {0}", filePath);
			}
		}
		else
		{
			EN_CORE_ERROR("Couldn't open the shader file at {0}", filePath);
		}
		return result;
	}
	
	std::unordered_map<GLenum, std::string> OpenGLShader::PreProccess(const std::string& shaderSrc)
	{
		std::unordered_map<GLenum, std::string> shaderSources;
		const char* separationToken = "#type";
		size_t separationTokenLenght = strlen(separationToken);
		size_t position = shaderSrc.find(separationToken, 0);
		while (position != std::string::npos)
		{
			size_t endOfLine = shaderSrc.find_first_of("\r\n", position);
			EN_CORE_ASSERT(endOfLine != std::string::npos, "Syntax Error in shader");
			size_t begin = position + separationTokenLenght + 1;
			std::string type = shaderSrc.substr(begin, endOfLine - begin);
			EN_CORE_ASSERT(ShaderTypeFromString(type), "Unknown shader type");

			size_t  nextLinePos = shaderSrc.find_first_not_of("\r\n", endOfLine);
			EN_CORE_ASSERT(nextLinePos != std::string::npos, "SintaxError");
			position = shaderSrc.find(separationToken, nextLinePos);

			shaderSources[ShaderTypeFromString(type)] = (position == std::string::npos) ? shaderSrc.substr(nextLinePos) 
				: shaderSrc.substr(nextLinePos, position - nextLinePos);
		}
		return shaderSources;
	}

	void OpenGLShader::Compile(const std::unordered_map<GLenum, std::string>& shaderSources)
	{
		// Get a program object.
		GLuint program = glCreateProgram();
		EN_CORE_ASSERT(shaderSources.size() < 3, "Too many shaders being uploaded at once");
		std::array<GLenum, 2> glShaderIds;
		int shaderIndex = 0;
		for (auto [key, value] : shaderSources)
		{
			GLenum type = key;
			const std::string& shaderSource = value;

			// Create an empty vertex shader handle
			GLuint Shader = glCreateShader(type);

			// Send the vertex shader source code to GL
			// Note that std::string's .c_str is NULL character terminated.
			const GLchar* source = (const GLchar*)shaderSource.c_str();
			glShaderSource(Shader, 1, &source, 0);

			// Compile the vertex shader
			glCompileShader(Shader);

			GLint isCompiled = 0;
			glGetShaderiv(Shader, GL_COMPILE_STATUS, &isCompiled);
			if (isCompiled == GL_FALSE)
			{
				GLint maxLength = 0;
				glGetShaderiv(Shader, GL_INFO_LOG_LENGTH, &maxLength);

				// The maxLength includes the NULL character
				std::vector<GLchar> infoLog(maxLength);
				glGetShaderInfoLog(Shader, maxLength, &maxLength, &infoLog[0]);

				// We don't need the shader anymore.
				glDeleteShader(Shader);

				// Use the infoLog as you see fit.
				EN_CORE_ERROR("{0} Shader Compilation failure!\n{1}", type == GL_VERTEX_SHADER? "Vertex" : "Fragment", infoLog.data());

				EN_CORE_ASSERT(false, "");
				return;
			}

			// Attach our shaders to our program
			glAttachShader(program, Shader);
			glShaderIds[shaderIndex++] = Shader;
		}

		mRendererID = program;
		// Link our program
		glLinkProgram(program);

		// Note the different functions here: glGetProgram* instead of glGetShader*.
		GLint isLinked = 0;
		glGetProgramiv(program, GL_LINK_STATUS, (int*)&isLinked);
		if (isLinked == GL_FALSE)
		{
			GLint maxLength = 0;
			glGetProgramiv(program, GL_INFO_LOG_LENGTH, &maxLength);

			// The maxLength includes the NULL character
			std::vector<GLchar> infoLog(maxLength);
			glGetProgramInfoLog(program, maxLength, &maxLength, &infoLog[0]);

			// We don't need the program anymore.
			glDeleteProgram(program);
			// Don't leak shaders either.
			for(GLenum shader : glShaderIds)
				glDeleteShader(shader);

			// Use the infoLog as you see fit.

			EN_CORE_ERROR("Program Compilation failure!\n{0}", infoLog.data());

			EN_CORE_ASSERT(false, "");
			return;
		}

		// Always detach shaders after a successful link.
		for (GLenum shader : glShaderIds)
		{
			glDetachShader(program, shader);
			glDeleteShader(shader);
		}
	}

	GLenum OpenGLShader::ShaderTypeFromString(const std::string& type)
	{
		if (type == "vertex") return GL_VERTEX_SHADER;
		if (type == "fragment") return GL_FRAGMENT_SHADER;
		return 0;
	}
}