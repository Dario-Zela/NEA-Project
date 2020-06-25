#include "ENPH.h"
#include "OpenGLContext.h"
#include <GLFW/glfw3.h>
#include <glad/glad.h>

namespace Engine
{
	OpenGLContext::OpenGLContext(GLFWwindow* windowHandle)
		:mWindowHandle(windowHandle) 
	{
		EN_CORE_ASSERT(windowHandle, "Window Handle is null");
	}

	void OpenGLContext::Init()
	{
		glfwMakeContextCurrent(mWindowHandle);
		int status = gladLoadGLLoader((GLADloadproc)glfwGetProcAddress);
		EN_CORE_ASSERT(status, "Failed to Initialise OpenGL");

		EN_CORE_INFO("Vendor: {0}", glGetString(GL_VENDOR));
		EN_CORE_INFO("Renderer: {0}", glGetString(GL_RENDERER));
		EN_CORE_INFO("Version: {0}", glGetString(GL_VERSION));
	}

	void OpenGLContext::SwapBuffers()
	{
		glfwSwapBuffers(mWindowHandle);
	}
}