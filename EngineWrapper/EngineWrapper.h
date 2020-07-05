#pragma once
#include "Engine.h"
#include "ManagedObject.h"

namespace Wrapper
{
	public ref class Application : public ManagedObject<Engine::Application>
	{
	public:
		Application() : ManagedObject(new Engine::Application()) {}

		void PushLayer(Layer^ layer) { mInstance->PushLayer(layer); }
		void PushOverlay(Layer^ overlay) { mInstance->PushOverlay(overlay); }

		Engine::Application* GetRaw() { return mInstance; }
	};

	public ref class Layer : public ManagedObject<Engine::Layer>
	{
	public:
		Layer(std::string name) : ManagedObject(new Engine::Layer(name)) {}
		virtual ~Layer() = default;

		virtual void OnAttach() {}
		virtual void OnDetach() {}
		virtual void OnUpdate(TimeStep time) {}
		virtual void OnEvent(Event & e) {}
		virtual void OnImGUIRender() {}

		inline const std::string& GetName() const { return mDebugName; }
	};

	public ref class EntryPoint : ManagedObject<Engine::EntryPoint>
	{
	public:
		EntryPoint():ManagedObject(new Engine::EntryPoint()) {}

		void Enter(Application app) { mInstance->main(app.GetRaw()); }
	};

	/*
	public ref class BufferElement;

	public ref class BufferLayout;

	public ref class Event;

	public ref class EventDispatcher;

	public ref class ImGUI;

	public ref class IndexBuffer;

	public ref class Input;

	public ref class KeyPressedEvent;

	public ref class KeyReleasedEvent;

	public ref class KeyTypedEvent;

	

	public ref class Log;

	public ref class MouseButtonPressedEvent;

	public ref class MouseButtonReleasedEvent;

	public ref class MouseMovedEvent;

	public ref class MouseScrolledEvent;

	public ref class OrthographicCamera;

	public ref class OrthographicCameraController;

	public ref class Renderer;

	public ref class Renderer2D;

	public ref class Shader;

	public ref class Texture2D;

	public ref class Texture;

	public ref class TimeStep;

	public ref class VertexArray;

	public ref class VertexBuffer;

	public ref class Window;

	public ref class WindowClosedEvent;

	public ref class WindowResizeEvent;

	public ref class ShaderDataType;
	

	static unsigned int ShaderDataTypeSize(ShaderDataType type) { return Engine::ShaderDataTypeSize(type); }
	*/

	Application CreateApplicationWrapper();

	extern Wrapper::Application Wrapper::CreateApplicationWrapper();
}

Engine::Application* Engine::CreateApplication()
{
	return Wrapper::CreateApplicationWrapper().GetRaw();
}