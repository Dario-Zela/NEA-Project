#pragma once
#include "Engine/Core/Core.h"

namespace Engine
{
	class ENGINE_API EntryPoint 
	{
	public:
		EntryPoint() = default;

		int main(Application* app)
		{
			Engine::Logger->Init();
			EN_CORE_WARN("Initialised Logger!");
			int a = 0;
			EN_TRACE("Initialised Logger!{0}", a);

			app->Run();
			delete app;
		}
	};
}