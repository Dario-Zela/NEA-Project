#pragma once
#include "Engine/Core/Core.h"

namespace Engine
{
	class __declspec(dllexport) EntryPoint
	{
	public:
		EntryPoint() = default;

		void main(Application* app)
		{
			Engine::Logger->Init();
			EN_CORE_WARN("Initialised Logger!");
			int a = 0;
			EN_TRACE("Initialised Logger!{0}", a);

			app->Run();
		}
	};
}