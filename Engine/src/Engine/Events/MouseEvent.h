#pragma once

#include "EventController.h"

namespace Engine
{
	class ENGINE_API MouseMovedEvent : public Event
	{
	public:
		MouseMovedEvent(float x, float y)
			: mMouseX(x), mMouseY(y) { }

		inline float GetMouseX() const { return mMouseX; }
		inline float GetMouseY() const { return mMouseY; }

		std::string ToString() const override
		{
			std::stringstream ss;
			ss << "MouseMovedEvent: X = " << mMouseX << ", Y = " << mMouseY;
			return ss.str();
		}


		EVENT_CLASS_CATEGORY(EventCategoryMouse | EventCategoryInput)
		EVENT_CLASS_TYPE(MouseMoved)

	protected:
		float mMouseX, mMouseY;
	};

	class ENGINE_API MouseScrolledEvent : public Event
	{
	public:
		MouseScrolledEvent(float xOffset, float yOffset)
			:mXOffset(xOffset), mYOffset(yOffset) { }

		inline float GetMouseXOffset() const { return mXOffset; }
		inline float GetMouseYOffset() const { return mYOffset; }

		std::string ToString() const override
		{
			std::stringstream ss;
			ss << "MouseScrolledEvent: XOffset = " << mXOffset << ", YOffset = " << mYOffset;
			return ss.str();
		}

		EVENT_CLASS_CATEGORY(EventCategoryMouse | EventCategoryInput)
		EVENT_CLASS_TYPE(MouseScrolled)

	protected:
		float mXOffset, mYOffset;
	};

	class ENGINE_API MouseButtonEvent : public Event
	{
	public:
		inline int GetMouseButton() const { return mButtonPressed; }

		EVENT_CLASS_CATEGORY(EventCategoryMouse | EventCategoryInput | EventCategoryMouseButton)

	protected:
		MouseButtonEvent(int button)
			:mButtonPressed(button) { }

		int mButtonPressed;
	};

	class ENGINE_API MouseButtonPressedEvent : public MouseButtonEvent
	{
	public:
		MouseButtonPressedEvent(int button)
			:MouseButtonEvent(button)
		{
		}

		std::string ToString() const override
		{
			std::stringstream ss;
			ss << "MouseButtonPressedEvent: " << mButtonPressed;
			return ss.str();
		}

		EVENT_CLASS_TYPE(MouseButtonPressed)

	private:
	};

	class ENGINE_API MouseButtonReleasedEvent : public MouseButtonEvent
	{
	public:
		MouseButtonReleasedEvent(int button)
			:MouseButtonEvent(button) { }

		std::string ToString() const override
		{
			std::stringstream ss;
			ss << "MouseButtonReleasedEvent: " << mButtonPressed;
			return ss.str();
		}

		EVENT_CLASS_TYPE(MouseButtonReleased)
	};
}