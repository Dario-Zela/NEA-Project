#pragma once

#include "Engine/Core/TimeStep.h"
#include "Engine/Core/Core.h"
#include "Engine/Events/EventController.h"

namespace Engine
{
	class ENGINE_API Layer
	{
	public:
		Layer(const std::string& name = "Layer");
		virtual ~Layer() = default;

		virtual void OnAttach() {}
		virtual void OnDetach() {}
		virtual void OnUpdate(TimeStep time) {}
		virtual void OnEvent(Event& e) {}
		virtual void OnImGUIRender() {}

		inline const std::string& GetName() const { return mDebugName; }

	private:
		std::string mDebugName;
	};
}