#pragma once
#include "Engine/Core/Core.h"

#include "Engine/Events/ApplicationEvent.h"
#include "Engine/Events/EventController.h"
#include "Engine/Core/LayerStack.h"
#include "Engine/Core/Window.h"

#include "Engine/Core/TimeStep.h"

#include "Engine/ImGUI/ImGUILayer.h"
#include "Engine/Renderer/Render.h"

namespace Engine 
{
	class ENGINE_API Application
	{
	public:
		Application();
		virtual ~Application() { Renderer::Shutdown(); }

		void OnEvent(Event& e);
		void Run();

		void PushLayer(Layer* layer);
		void PushOverlay(Layer* overlay);

		inline static Application& Get() { return *sInstance; }
		inline Window& GetWindow() { return *mWindow; }

	private:
		static Application* sInstance;
		bool OnWindowClosed(WindowClosedEvent& e);
		bool OnWindowResize(WindowResizeEvent& e);
		Scope<Window> mWindow;
		ImGUILayer* mImGUILayer;
		bool mRunning = true, mMinimised = false;
		LayerStack mLayerStack;
		float mLastFrameTime = 0;
	};
}
