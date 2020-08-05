#include "ENPH.h"
#include "OpenGLRendererAPI.h"
#include <glad/glad.h>

namespace Engine
{
	void OpenGLMessageCallback(
		unsigned source,
		unsigned type,
		unsigned id,
		unsigned severity,
		int lenght,
		const char* message,
		const void* userParams
	)
	{
		switch (severity)
		{
		case GL_DEBUG_SEVERITY_HIGH: EN_CORE_CRITICAL(message); return;
		case GL_DEBUG_SEVERITY_MEDIUM: EN_CORE_ERROR(message); return;
		case GL_DEBUG_SEVERITY_LOW: EN_CORE_WARN(message); return;
		case GL_DEBUG_SEVERITY_NOTIFICATION: EN_CORE_INFO(message); return;
		}
		EN_CORE_CRITICAL(message);
		EN_CORE_ASSERT(false, "Unknown severity level");
	}

	void OpenGLRendererAPI::Init()
	{
	#ifdef EN_DEBUG
		glEnable(GL_DEBUG_OUTPUT);
		glEnable(GL_DEBUG_OUTPUT_SYNCHRONOUS);

		glDebugMessageCallback(OpenGLMessageCallback, nullptr);
		glDebugMessageControl(GL_DONT_CARE, GL_DONT_CARE, GL_DEBUG_SEVERITY_NOTIFICATION, 0, NULL, GL_FALSE);
	#endif // EN_DEBUG

		glEnable(GL_BLEND);
		glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);

		glEnable((GL_DEPTH_TEST));
	}

	void OpenGLRendererAPI::DrawIndexed(const Ref<VertexArray>& vertexArray, unsigned int count)
	{
		count = count == 0 ? vertexArray->GetIndexBuffer()->GetCount() : count;
		glDrawElements(GL_TRIANGLES, count, GL_UNSIGNED_INT, nullptr);
	}

	void OpenGLRendererAPI::Clear(const glm::vec4& color)
	{
		glClearColor(color.r, color.g, color.b, color.a);
		glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
	}

	void OpenGLRendererAPI::SetViewport(unsigned int x, unsigned int y, unsigned int width, unsigned int height)
	{
		glViewport(x, y, width, height);
	}
}