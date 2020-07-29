#pragma once

namespace Wrapper
{
	public ref class TimeStep
	{
	protected:
		Engine::TimeStep* mInstance;
	public:
		TimeStep() : mInstance(new Engine::TimeStep()) {}
		TimeStep(Engine::TimeStep* timeStep) : mInstance(timeStep) {}

		float GetSeconds() { return mInstance->GetSeconds(); }
		float GetMilliseconds() { return mInstance->GetMilliseconds(); }
		
		static operator float(TimeStep^ timeStep) { return timeStep->GetSeconds(); }
		
		Engine::TimeStep* GetTimeStep() { return mInstance; }
		
		!TimeStep() {}
		~TimeStep() {}
	};

	public ref class Event abstract
	{
	public:
		Event() { }
		~Event() { this->!Event(); }
		!Event() { }

		virtual Engine::EventType GetEventType() = 0;
	};

	public ref class KeyPressedEvent : Event
	{
		Engine::KeyPressedEvent& e;
	public:
		KeyPressedEvent(Engine::Event& e) : e(static_cast<Engine::KeyPressedEvent&>(e)), Event() { }
		KeyPressedEvent(Engine::KeyPressedEvent& e) : e(e), Event() {}
		KeyPressedEvent::KeyPressedEvent(KeyPressedEvent% e) : e(e.GetRaw()), Event() {}

		~KeyPressedEvent() { this->!KeyPressedEvent(); }
		!KeyPressedEvent() { }

		bool IsHandled() { return e.IsHandled(); }
		int GetKeyCode() { return e.GetKeyCode(); }
		int GetRepeatedCount() { return e.GetRepeatedCount(); }

		virtual Engine::EventType GetEventType() override { return e.GetEventType(); }

		Engine::KeyPressedEvent& GetRaw() { return e; }
		static operator Engine::KeyPressedEvent(KeyPressedEvent^ val) { return val->GetRaw(); }
		static operator KeyPressedEvent ^ (Engine::Event& e) { return gcnew KeyPressedEvent(e); }
		static operator KeyPressedEvent(Engine::Event& e) { return KeyPressedEvent(e); }
	};

	public ref class KeyReleasedEvent : Event
	{
		Engine::KeyReleasedEvent& e;
	public:
		KeyReleasedEvent(Engine::Event& e) : e(static_cast<Engine::KeyReleasedEvent&>(e)), Event() { }
		KeyReleasedEvent(Engine::KeyReleasedEvent& e) : e(e), Event() {}
		KeyReleasedEvent::KeyReleasedEvent(KeyReleasedEvent% e) : e(e.GetRaw()), Event() {}

		~KeyReleasedEvent() { this->!KeyReleasedEvent(); }
		!KeyReleasedEvent() { }

		bool IsHandled() { return e.IsHandled(); }
		int GetKeyCode() { return e.GetKeyCode(); }

		virtual Engine::EventType GetEventType() override { return e.GetEventType(); }

		Engine::KeyReleasedEvent& GetRaw() { return e; }
		static operator Engine::KeyReleasedEvent(KeyReleasedEvent^ val) { return val->GetRaw(); }
		static operator KeyReleasedEvent ^ (Engine::Event& e) { return gcnew KeyReleasedEvent(e); }
		static operator KeyReleasedEvent(Engine::Event& e) { return KeyReleasedEvent(e); }
	};

	public ref class KeyTypedEvent : Event
	{
		Engine::KeyTypedEvent& e;
	public:
		KeyTypedEvent(Engine::Event& e) : e(static_cast<Engine::KeyTypedEvent&>(e)), Event() { }
		KeyTypedEvent(Engine::KeyTypedEvent& e) : e(e), Event() {}
		KeyTypedEvent::KeyTypedEvent(KeyTypedEvent% e) : e(e.GetRaw()), Event() {}

		~KeyTypedEvent() { this->!KeyTypedEvent(); }
		!KeyTypedEvent() { }

		bool IsHandled() { return e.IsHandled(); }
		int GetKeyCode() { return e.GetKeyCode(); }

		virtual Engine::EventType GetEventType() override { return e.GetEventType(); }

		Engine::KeyTypedEvent& GetRaw() { return e; }
		static operator Engine::KeyTypedEvent(KeyTypedEvent^ val) { return val->GetRaw(); }
		static operator KeyTypedEvent ^ (Engine::Event& e) { return gcnew KeyTypedEvent(e); }
		static operator KeyTypedEvent(Engine::Event& e) { return KeyTypedEvent(e); }
	};

	public ref class MouseButtonPressedEvent : Event
	{
		Engine::MouseButtonPressedEvent& e;
	public:
		MouseButtonPressedEvent(Engine::Event& e) : e(static_cast<Engine::MouseButtonPressedEvent&>(e)), Event() { }
		MouseButtonPressedEvent(Engine::MouseButtonPressedEvent& e) : e(e), Event() {}
		MouseButtonPressedEvent::MouseButtonPressedEvent(MouseButtonPressedEvent% e) : e(e.GetRaw()), Event() {}

		~MouseButtonPressedEvent() { this->!MouseButtonPressedEvent(); }
		!MouseButtonPressedEvent() { }

		bool IsHandled() { return e.IsHandled(); }
		int GetMouseButton() { return e.GetMouseButton(); }

		virtual Engine::EventType GetEventType() override { return e.GetEventType(); }

		Engine::MouseButtonPressedEvent& GetRaw() { return e; }
		static operator Engine::MouseButtonPressedEvent(MouseButtonPressedEvent^ val) { return val->GetRaw(); }
		static operator MouseButtonPressedEvent ^ (Engine::Event& e) { return gcnew MouseButtonPressedEvent(e); }
		static operator MouseButtonPressedEvent(Engine::Event& e) { return MouseButtonPressedEvent(e); }
	};

	public ref class MouseButtonReleasedEvent : Event
	{
		Engine::MouseButtonReleasedEvent& e;
	public:
		MouseButtonReleasedEvent(Engine::Event& e) : e(static_cast<Engine::MouseButtonReleasedEvent&>(e)), Event() { }
		MouseButtonReleasedEvent(Engine::MouseButtonReleasedEvent& e) : e(e), Event() {}
		MouseButtonReleasedEvent::MouseButtonReleasedEvent(MouseButtonReleasedEvent% e) : e(e.GetRaw()), Event() {}

		~MouseButtonReleasedEvent() { this->!MouseButtonReleasedEvent(); }
		!MouseButtonReleasedEvent() { }

		bool IsHandled() { return e.IsHandled(); }
		int GetMouseButton() { return e.GetMouseButton(); }

		virtual Engine::EventType GetEventType() override { return e.GetEventType(); }

		Engine::MouseButtonReleasedEvent& GetRaw() { return e; }
		static operator Engine::MouseButtonReleasedEvent(MouseButtonReleasedEvent^ val) { return val->GetRaw(); }
		static operator MouseButtonReleasedEvent ^ (Engine::Event& e) { return gcnew MouseButtonReleasedEvent(e); }
		static operator MouseButtonReleasedEvent(Engine::Event& e) { return MouseButtonReleasedEvent(e); }
	};

	public ref class MouseMovedEvent : Event
	{
		Engine::MouseMovedEvent& e;
	public:
		MouseMovedEvent(Engine::Event& e) : e(static_cast<Engine::MouseMovedEvent&>(e)), Event() { }
		MouseMovedEvent(Engine::MouseMovedEvent& e) : e(e), Event() {}
		MouseMovedEvent::MouseMovedEvent(MouseMovedEvent% e) : e(e.GetRaw()), Event() {}

		~MouseMovedEvent() { this->!MouseMovedEvent(); }
		!MouseMovedEvent() { }

		virtual Engine::EventType GetEventType() override { return e.GetEventType(); }

		bool IsHandled() { return e.IsHandled(); }
		float GetMouseX() { return e.GetMouseX(); }
		float GetMouseY() { return e.GetMouseY(); }

		Engine::MouseMovedEvent& GetRaw() { return e; }
		static operator Engine::MouseMovedEvent(MouseMovedEvent^ val) { return val->GetRaw(); }
		static operator MouseMovedEvent ^ (Engine::Event& e) { return gcnew MouseMovedEvent(e); }
		static operator MouseMovedEvent(Engine::Event& e) { return MouseMovedEvent(e); }
	};

	public ref class MouseScrolledEvent : Event
	{
		Engine::MouseScrolledEvent& e;
	public:
		MouseScrolledEvent(Engine::Event& e) : e(static_cast<Engine::MouseScrolledEvent&>(e)), Event() { }
		MouseScrolledEvent(Engine::MouseScrolledEvent& e) : e(e), Event() {}
		MouseScrolledEvent::MouseScrolledEvent(MouseScrolledEvent% e) : e(e.GetRaw()), Event() {}

		~MouseScrolledEvent() { this->!MouseScrolledEvent(); }
		!MouseScrolledEvent() { }

		bool IsHandled() { return e.IsHandled(); }
		float GetMouseXOffset() { return e.GetMouseXOffset(); }
		float GetMouseYOffset() { return e.GetMouseYOffset(); }

		virtual Engine::EventType GetEventType() override { return e.GetEventType(); }

		Engine::MouseScrolledEvent& GetRaw() { return e; }
		static operator Engine::MouseScrolledEvent(MouseScrolledEvent^ val) { return val->GetRaw(); }
		static operator MouseScrolledEvent ^ (Engine::Event& e) { return gcnew MouseScrolledEvent(e); }
		static operator MouseScrolledEvent(Engine::Event& e) { return MouseScrolledEvent(e); }
	};

	public ref class WindowClosedEvent : Event
	{
		Engine::WindowClosedEvent& e;
	public:
		WindowClosedEvent(Engine::Event& e) : e(static_cast<Engine::WindowClosedEvent&>(e)), Event() { }
		WindowClosedEvent(Engine::WindowClosedEvent& e) : e(e), Event() {}
		WindowClosedEvent::WindowClosedEvent(WindowClosedEvent% e) : e(e.GetRaw()), Event() {}

		~WindowClosedEvent() { this->!WindowClosedEvent(); }
		!WindowClosedEvent() { }

		bool IsHandled() { return e.IsHandled(); }

		virtual Engine::EventType GetEventType() override { return e.GetEventType(); }

		Engine::WindowClosedEvent& GetRaw() { return e; }
		static operator Engine::WindowClosedEvent(WindowClosedEvent^ val) { return val->GetRaw(); }
		static operator WindowClosedEvent ^ (Engine::Event& e) { return gcnew WindowClosedEvent(e); }
		static operator WindowClosedEvent(Engine::Event& e) { return WindowClosedEvent(e); }
	};

	public ref class WindowResizeEvent : Event
	{
		Engine::WindowResizeEvent& e;
	public:
		WindowResizeEvent(Engine::Event& e) : e(static_cast<Engine::WindowResizeEvent&>(e)), Event() { }
		WindowResizeEvent(Engine::WindowResizeEvent& e) : e(e), Event() {}
		WindowResizeEvent::WindowResizeEvent(WindowResizeEvent% e) : e(e.GetRaw()), Event() {}

		~WindowResizeEvent() { this->!WindowResizeEvent(); }
		!WindowResizeEvent() { }

		bool IsHandled() { return e.IsHandled(); }
		int GetWidth() { return e.GetWidth(); }
		int GetHeight() { return e.GetHeight(); }

		virtual Engine::EventType GetEventType() override { return e.GetEventType(); }

		Engine::WindowResizeEvent& GetRaw() { return e; }
		static operator Engine::WindowResizeEvent(WindowResizeEvent^ val) { return val->GetRaw(); }
		static operator WindowResizeEvent ^ (Engine::Event& e) { return gcnew WindowResizeEvent(e); }
		static operator WindowResizeEvent(Engine::Event& e) { return WindowResizeEvent(e); }
	};

	public ref class EventDispatcher : public ManagedObject<Engine::EventDispatcher>
	{
	public:
		EventDispatcher(Event^ e) : ManagedObject(new Engine::EventDispatcher(GetEvent(e))) {}

		static Engine::Event& GetEvent(Event^ e)
		{
			switch (e->GetEventType())
			{
			case Engine::EventType::KeyPressed: return ((KeyPressedEvent^)e)->GetRaw();
			case Engine::EventType::KeyReleased: return ((KeyReleasedEvent^)e)->GetRaw();
			case Engine::EventType::KeyTyped: return ((KeyTypedEvent^)e)->GetRaw();
			case Engine::EventType::MouseButtonPressed: return ((MouseButtonPressedEvent^)e)->GetRaw();
			case Engine::EventType::MouseButtonReleased: return ((MouseButtonReleasedEvent^)e)->GetRaw();
			case Engine::EventType::MouseMoved: return ((MouseMovedEvent^)e)->GetRaw();
			case Engine::EventType::MouseScrolled: return ((MouseScrolledEvent^)e)->GetRaw();
			case Engine::EventType::WindowClosed: return ((WindowClosedEvent^)e)->GetRaw();
			case Engine::EventType::WindowResize: return ((WindowResizeEvent^)e)->GetRaw();
			case Engine::EventType::Undefinded: throw gcnew ArgumentNullException("Unknown Event Type");
			}
			throw gcnew ArgumentNullException("No Event Type");
		}

		bool Dispatch(Func<KeyPressedEvent^, bool>^ func) { return mInstance->Dispatch<Engine::KeyPressedEvent, KeyPressedEvent^>(func); }
		bool Dispatch(Func<KeyReleasedEvent^, bool>^ func) { return mInstance->Dispatch<Engine::KeyReleasedEvent, KeyReleasedEvent^>(func); }
		bool Dispatch(Func<KeyTypedEvent^, bool>^ func) { return mInstance->Dispatch<Engine::KeyTypedEvent, KeyTypedEvent^>(func); }
		bool Dispatch(Func<MouseButtonPressedEvent^, bool>^ func) { return mInstance->Dispatch<Engine::MouseButtonPressedEvent, MouseButtonPressedEvent^>(func); }
		bool Dispatch(Func<MouseButtonReleasedEvent^, bool>^ func) { return mInstance->Dispatch<Engine::MouseButtonReleasedEvent, MouseButtonReleasedEvent^>(func); }
		bool Dispatch(Func<MouseMovedEvent^, bool>^ func) { return mInstance->Dispatch<Engine::MouseMovedEvent, MouseMovedEvent^>(func); }
		bool Dispatch(Func<MouseScrolledEvent^, bool>^ func) { return mInstance->Dispatch<Engine::MouseScrolledEvent, MouseScrolledEvent^>(func); }
		bool Dispatch(Func<WindowClosedEvent^, bool>^ func) { return mInstance->Dispatch<Engine::WindowClosedEvent, WindowClosedEvent^>(func); }
		bool Dispatch(Func<WindowResizeEvent^, bool>^ func) { return mInstance->Dispatch<Engine::WindowResizeEvent, WindowResizeEvent^>(func); }

		//Separate Keys
		/*
		bool DispatchKP(Func<KeyPressedEvent^, bool>^ func) { return mInstance->Dispatch<Engine::KeyPressedEvent, KeyPressedEvent^>(func); }
		bool DispatchKR(Func<KeyReleasedEvent^, bool>^ func) { return mInstance->Dispatch<Engine::KeyReleasedEvent, KeyReleasedEvent^>(func); }
		bool DispatchKT(Func<KeyTypedEvent^, bool>^ func) { return mInstance->Dispatch<Engine::KeyTypedEvent, KeyTypedEvent^>(func); }
		bool DispatchMP(Func<MouseButtonPressedEvent^, bool>^ func) { return mInstance->Dispatch<Engine::MouseButtonPressedEvent, MouseButtonPressedEvent^>(func); }
		bool DispatchMR(Func<MouseButtonReleasedEvent^, bool>^ func) { return mInstance->Dispatch<Engine::MouseButtonReleasedEvent, MouseButtonReleasedEvent^>(func); }
		bool DispatchMM(Func<MouseMovedEvent^, bool>^ func) { return mInstance->Dispatch<Engine::MouseMovedEvent, MouseMovedEvent^>(func); }
		bool DispatchMS(Func<MouseScrolledEvent^, bool>^ func) { return mInstance->Dispatch<Engine::MouseScrolledEvent, MouseScrolledEvent^>(func); }
		bool DispatchWC(Func<WindowClosedEvent^, bool>^ func) { return mInstance->Dispatch<Engine::WindowClosedEvent, WindowClosedEvent^>(func); }
		bool DispatchWR(Func<WindowResizeEvent^, bool>^ func) { return mInstance->Dispatch<Engine::WindowResizeEvent, WindowResizeEvent^>(func); }
		*/
	};
}