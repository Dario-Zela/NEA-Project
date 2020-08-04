#include "ENPH.h"
#include "Application.h"
#include "Engine/Core/Input.h"
#include "Engine/Renderer/Render.h"
#include <GLFW/glfw3.h>

namespace Engine
{ 
	Application* Application::sInstance = nullptr;

	bool Application::OnWindowClosed(WindowClosedEvent& e)
	{
		mRunning = false;
		return true;
	}

	bool Application::OnWindowResize(WindowResizeEvent& e)
	{
		if (e.GetWidth() == 0 || e.GetHeight() == 0)
		{
			mMinimised = true;
			return false;
		}
		mMinimised = false;
		Renderer::OnWindowResise(e.GetWidth(), e.GetHeight());
		
		return false;
	}

	Application::Application(std::string title, unsigned int width, unsigned int height)
	{
		EN_CORE_ASSERT(!sInstance, "Application already exists");
		sInstance = this;
		mWindow = Scope<Window>(Window::Create(WindowProps(title, width, height)));
		mWindow->SetEventCallback(EN_BIND_EVENT_FN(Application::OnEvent));
		Renderer::Init();

		mImGUILayer = new ImGUILayer();
		PushOverlay(mImGUILayer);
	}

	void Application::OnEvent(Event& e) 
	{
		EventDispatcher dispatcher(e);
		dispatcher.Dispatch<WindowClosedEvent>(EN_BIND_EVENT_FN(Application::OnWindowClosed));
		dispatcher.Dispatch<WindowResizeEvent>(EN_BIND_EVENT_FN(Application::OnWindowResize));

		for (auto it = mLayerStack.rbegin(); it != mLayerStack.rend(); it++)
		{
			(*it)->OnEvent(e);
			if (e.IsHandled())
				break;
		}
	}

	void Application::Run()
	{
		while (mRunning)
		{
			float time = (float)glfwGetTime();
			TimeStep timeStep = time - mLastFrameTime ;
			mLastFrameTime = time;

			if(!mMinimised)
			for (Layer* layer : mLayerStack)
			{
				layer->OnUpdate(timeStep);
			}

			mImGUILayer->Begin();
			for (Layer* layer : mLayerStack)
			{
				layer->OnImGUIRender();
			}
			mImGUILayer->End();

			mWindow->OnUpdate();
		}
	}

	void Application::PushLayer(Layer* layer)
	{
		mLayerStack.PushLayer(layer);
	}

	void Application::PushOverlay(Layer* overlay)
	{
		mLayerStack.PushOverlay(overlay);
	}

	void Application::PopLayer(Layer* layer)
	{
		mLayerStack.PopLayer(layer);
	}

	void Application::PopOverlay(Layer* overlay)
	{
		mLayerStack.PopOverlay(overlay);
	}

}