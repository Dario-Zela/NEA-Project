#pragma once
#include <msclr/gcroot.h>

#include "../Engine/src/Engine.h"
#include "../Engine/src/Engine/Core/EntryPoint.h"

#include "ManagedObject.h"
#include "EventWrapper.h"
#include "DrawDataWrapper.h"
#include "RendererWrapper.h"
#include "InputWrapper.h"

namespace Wrapper
{
	ref class Layer;

	class LayerRedirector : public Engine::Layer
	{
	public:
		LayerRedirector(Wrapper::Layer^ owner, std::string name) :Layer(name), mOwner(owner) {}

		virtual void OnAttach();
		virtual void OnDetach();
		virtual void OnUpdate(Engine::TimeStep time);
		virtual void OnEvent(Engine::Event& e);
		virtual void OnImGUIRender();

	private:
		msclr::gcroot<Wrapper::Layer^> mOwner;
	};

	public ref class Layer abstract
	{
	protected:
		Engine::Layer* mInstance;
	public:
		Layer(String^ name) { mInstance = new LayerRedirector(this, stringsToCStrings(name)); }
		~Layer() { this->!Layer(); }
		!Layer() { }

		virtual void OnAttach() = 0;
		virtual void OnDetach() = 0;
		virtual void OnUpdate(TimeStep^ time) = 0;
		virtual void OnEvent(Event^ e) = 0;
		virtual void OnImGUIRender() = 0;

		Engine::Layer* GetLayer() { return mInstance; }
	internal:
		void callOnAttach()
		{
			if (!mInstance) throw gcnew ObjectDisposedException("Layer");
			OnAttach();
		}
		
		void callOnDetach()
		{
			if (!mInstance) throw gcnew ObjectDisposedException("Layer");
			OnDetach();
		}

		void callOnUpdate(TimeStep^ time)
		{
			if (!mInstance) throw gcnew ObjectDisposedException("Layer");
			OnUpdate(time);
		}

		void callOnEvent(Event^ e)
		{
			if (!mInstance) throw gcnew ObjectDisposedException("Layer");
			OnEvent(e);
		}

		void callOnImGUIRender()
		{
			if (!mInstance) throw gcnew ObjectDisposedException("Layer");
			OnImGUIRender();
		}
	};

	public ref class Application : public ManagedObject<Engine::Application>
	{
	public:
		Application() : 
			Application("Engine", 1280, 720) { }
		
		Application(String^ Title, unsigned int Width, unsigned int Height) :
			ManagedObject(new Engine::Application(stringsToCStrings(Title), Width, Height)) { }


		void PushLayer(Layer^ layer) { mInstance->PushLayer(layer->GetLayer()); }
		void PushOverlay(Layer^ overlay) { mInstance->PushOverlay(overlay->GetLayer()); }
		void PopLayer(Layer^ layer) { mInstance->PopLayer(layer->GetLayer()); }
		void PopOverlay(Layer^ overlay) { mInstance->PopOverlay(overlay->GetLayer()); }

		Engine::Application* GetRaw() { return mInstance; }
	};

	public ref class EntryPoint : public ManagedObject<Engine::EntryPoint>
	{
	public:
		EntryPoint() : ManagedObject(new Engine::EntryPoint()) {}

		void Enter(Application^ app) { mInstance->main(app->GetRaw()); }
	};

	public ref struct Log
	{
		static void Trace(String^ string) { EN_TRACE(stringsToCStrings(string)) }

		static void Info(String^ string) { EN_INFO(stringsToCStrings(string)) }

		static void Waring(String^ string) { EN_WARN(stringsToCStrings(string)) }

		static void Error(String^ string) { EN_ERROR(stringsToCStrings(string)) }

		static void Critical(String^ string) { EN_CRITICAL(stringsToCStrings(string)) }
	};
}