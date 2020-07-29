#pragma once
#include "Engine/Core/Core.h"
#include "Engine/Core/Log.h"

namespace Engine
{
	class __declspec(dllexport) EntryPoint
	{
	public:
		EntryPoint()
		{
			try
			{
				Logger->Init();
			}
			catch (std::exception&)
			{
				EN_CORE_ASSERT(false, "Failed Initialising Logger");
			}
			EN_CORE_INFO("Logger Initialised");
			EN_CORE_CRITICAL("Test");
		}

		void main(Application* app)
		{
			app->Run();
		}
	};
}