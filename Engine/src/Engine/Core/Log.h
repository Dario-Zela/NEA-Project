#pragma once
#include "Core.h"
#include <chrono>
#include <ctime>
#include <fstream>
#include <fmt.h>

namespace Engine 
{
	class ENGINE_API Log
	{
	public:
		void Init();

		template<typename ...Args>
		void Add(int WarningLevel, std::string Name, const std::string& fmt, const Args& ...args)
		{
			std::string Warning;
			switch (WarningLevel)
			{
			case 0: Warning = "Trace"; break;
			case 1: Warning = "Info"; break;
			case 2: Warning = "Warning"; break;
			case 3: Warning = "Error"; break;
			case 4: Warning = "Critical"; break;
			}

			std::string string = fmt::format("[" + GetTime() + "]" + "[" + Name + "]"+ "[" + Warning + "]" + fmt, args...);

			std::cout << string << std::endl;
			if (atomic)
				atomic = false;
			std::ofstream file("../../../.Log/Log.txt", std::ofstream::app);
			file << string + "\n";
			file.close();
			atomic = true;
		}

	private:
		std::string GetTime();
		bool atomic = true;
	};

	static Ref<Log> Logger = CreateRef<Log>();
}

//CoreLogger macros
#define EN_CORE_CRITICAL(...)	::Engine::Logger->Add(4, "ENGINE", __VA_ARGS__);
#define EN_CORE_ERROR(...)		::Engine::Logger->Add(3, "ENGINE", __VA_ARGS__);
#define EN_CORE_WARN(...)		::Engine::Logger->Add(2, "ENGINE", __VA_ARGS__);
#define EN_CORE_INFO(...)		::Engine::Logger->Add(1, "ENGINE", __VA_ARGS__);
#define EN_CORE_TRACE(...)		::Engine::Logger->Add(0, "ENGINE", __VA_ARGS__);
										  		
//ClientLogger Macros					  		
#define EN_CRITICAL(...)		::Engine::Logger->Add(4, "APP", __VA_ARGS__);
#define EN_ERROR(...)			::Engine::Logger->Add(3, "APP", __VA_ARGS__);
#define EN_WARN(...)			::Engine::Logger->Add(2, "APP", __VA_ARGS__);
#define EN_INFO(...)			::Engine::Logger->Add(1, "APP", __VA_ARGS__);
#define EN_TRACE(...)			::Engine::Logger->Add(0, "APP", __VA_ARGS__);
