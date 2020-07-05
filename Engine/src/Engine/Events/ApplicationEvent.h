#pragma once

#include "ENPH.h"
#include "EventController.h"

namespace Engine
{
	class ENGINE_API WindowResizeEvent : public Event 
	{
	public:
		WindowResizeEvent(unsigned int width, unsigned int height)
			:mWidth(width), mHeight(height) {};

		inline unsigned int GetWidth() { return mWidth; }
		inline unsigned int GetHeight() { return mHeight; }

		std::string ToString() const override
		{
			std::stringstream ss;
			ss << "WindowResizeEvent: Width = " << mWidth << ", Heght = " << mHeight;
			return ss.str();
		}

		EVENT_CLASS_TYPE(WindowResize)
		EVENT_CLASS_CATEGORY(EventCategoryApplication)
	private:
		unsigned int mWidth, mHeight;
	};

	class ENGINE_API WindowClosedEvent : public Event
	{
	public:
		WindowClosedEvent() = default;

		std::string ToString() const override
		{
			std::stringstream ss;
			ss << "WindowClosedEvent";
			return ss.str();
		}

		EVENT_CLASS_TYPE(WindowClosed)
		EVENT_CLASS_CATEGORY(EventCategoryApplication)
	};
}