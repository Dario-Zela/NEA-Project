#pragma once
#include "Core.h"
#include <chrono>
#include <ctime>
#include <fstream>
#include <fmt.h>
#include "bundled/color.h"

namespace Engine 
{
	class ENGINE_API Log
	{
	public:
		void Init();

		template<typename ...Args>
		void Add(int WarningLevel, std::string Name, const std::string& fmt, const Args& ...args)
		{
			fmt::color color = fmt::v6::color::white;
			switch (WarningLevel)
			{
			case 0: color = fmt::color::black; break;
			case 1: color = fmt::color::green; break;
			case 2: color = fmt::color::yellow; break;
			case 3: color = fmt::color::red; break;
			case 4: color = fmt::color::crimson; break;
			}

			std::string string = fmt::format(fg(color),
				"[" + GetTime() + "]" + "[" + Name + "] " + fmt, args...);

			std::ofstream file("../../../Log.txt", std::ofstream::app);
			file << string + "\n";
			file.close();
		}

	private:
		std::string GetTime();
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