#include "EngineWrapper.h"

namespace  Wrapper
{
	void LayerRedirector::OnAttach()
	{
		mOwner->callOnAttach();
	}

	void LayerRedirector::OnDetach()
	{
		mOwner->callOnDetach();
	}

	void LayerRedirector::OnUpdate(Engine::TimeStep time)
	{
		Engine::RenderCommand::Clear();
		mOwner->callOnUpdate(gcnew TimeStep(&time));
	}

	void LayerRedirector::OnEvent(Engine::Event& e)
	{
		switch (e.GetEventType())
		{
		case Engine::EventType::KeyPressed: mOwner->callOnEvent(gcnew KeyPressedEvent(e)); break;
		case Engine::EventType::KeyReleased: mOwner->callOnEvent(gcnew KeyReleasedEvent(e)); break;
		case Engine::EventType::KeyTyped: mOwner->callOnEvent(gcnew KeyTypedEvent(e)); break;
		case Engine::EventType::MouseButtonPressed: mOwner->callOnEvent(gcnew MouseButtonPressedEvent(e)); break;
		case Engine::EventType::MouseButtonReleased: mOwner->callOnEvent(gcnew MouseButtonReleasedEvent(e)); break;
		case Engine::EventType::MouseMoved: mOwner->callOnEvent(gcnew MouseMovedEvent(e)); break;
		case Engine::EventType::MouseScrolled: mOwner->callOnEvent(gcnew MouseScrolledEvent(e)); break;
		case Engine::EventType::WindowClosed: mOwner->callOnEvent(gcnew WindowClosedEvent(e)); break;
		case Engine::EventType::WindowResize: mOwner->callOnEvent(gcnew WindowResizeEvent(e)); break;
		case Engine::EventType::Undefinded: throw gcnew ArgumentNullException("Unknown Event Type"); break;
		}
	}

	void LayerRedirector::OnImGUIRender()
	{
		mOwner->callOnImGUIRender();
	}
}