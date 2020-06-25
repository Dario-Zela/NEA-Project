#pragma once
#include "Core.h"
#include <spdlog/spdlog.h>
#include <spdlog/fmt/ostr.h>

namespace Engine 
{
	class ENGINE_API Log
	{
	public:
		static void Init();

		inline static Ref<spdlog::logger>& GetCoreLogger() { return mCoreLogger; }
		inline static Ref<spdlog::logger>& GetClientLogger() { return mClientLogger; }
	private:
		static Ref<spdlog::logger> mCoreLogger;
		static Ref<spdlog::logger> mClientLogger;
	};
}

//CoreLogger macros
#define EN_CORE_CRITICAL(...)	::Engine::Log::GetCoreLogger()->critical(__VA_ARGS__);
#define EN_CORE_ERROR(...)		::Engine::Log::GetCoreLogger()->error(__VA_ARGS__);
#define EN_CORE_WARN(...)		::Engine::Log::GetCoreLogger()->warn(__VA_ARGS__);
#define EN_CORE_INFO(...)		::Engine::Log::GetCoreLogger()->info(__VA_ARGS__);
#define EN_CORE_TRACE(...)		::Engine::Log::GetCoreLogger()->trace(__VA_ARGS__);

//ClientLogger Macros
#define EN_CRITICAL(...)		::Engine::Log::GetClientLogger()->critical(__VA_ARGS__);
#define EN_ERROR(...)			::Engine::Log::GetClientLogger()->error(__VA_ARGS__);
#define EN_WARN(...)			::Engine::Log::GetClientLogger()->warn(__VA_ARGS__);
#define EN_INFO(...)			::Engine::Log::GetClientLogger()->info(__VA_ARGS__);
#define EN_TRACE(...)			::Engine::Log::GetClientLogger()->trace(__VA_ARGS__);