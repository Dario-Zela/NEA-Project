#pragma once

#include "Engine/Core/Core.h"

namespace Engine
{
	class ENGINE_API Input
	{
	public:
		Input(const Input&) = delete;
		Input& operator=(const Input&) = delete;

		inline static bool IsKeyPressed(int keycode) { return sInstance->IsKeyPressedImpl(keycode); }
		inline static bool IsMouseButtonPressed(int button) { return sInstance->IsMouseButtonPressedImpl(button); }
		inline static float GetMouseX() { return sInstance->GetMouseXImpl(); }
		inline static float GetMouseY() { return sInstance->GetMouseYImpl(); }
		inline static std::pair<float, float> GetMousePosition() { return sInstance->GetMousePositionImpl(); }
	protected:
		Input() = default;
		virtual bool IsKeyPressedImpl(int keycode) = 0;
		virtual bool IsMouseButtonPressedImpl(int button) = 0;
		virtual float GetMouseXImpl() = 0;
		virtual std::pair<float, float> GetMousePositionImpl() = 0;
		virtual float GetMouseYImpl() = 0;
	private:
		static Scope<Input> sInstance;
	};
}