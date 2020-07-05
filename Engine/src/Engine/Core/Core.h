#pragma once
#include <memory>

#ifdef EN_BUILD_DLL
	#define ENGINE_API __declspec(dllexport)
#else
	#define ENGINE_API __declspec(dllimport)
#endif

#define BIT(x) (1 << x)

#ifdef EN_DEBUG
	#define EN_ENABLE_ASSERTS
#endif

#ifdef EN_ENABLE_ASSERTS
	#define EN_ASSERT(x,...) {if(!(x)) {EN_ERROR("Assertion Failed: {0}", __VA_ARGS__); __debugbreak(); } }
	#define	EN_CORE_ASSERT(x,...) {if(!(x)) {EN_CORE_ERROR("Assertion Failed: {0}", __VA_ARGS__); __debugbreak(); } }
#else
	#define EN_ASSERT(x,...)
	#define	EN_CORE_ASSERT(x,...)
#endif // EN_ENABLE_ASSERTS

#define EN_BIND_EVENT_FN(fn) std::bind(&fn, this, std::placeholders::_1)

namespace Engine
{
	template<typename T>
	using Scope = std::unique_ptr<T>;
	template<typename T, typename ... Args>
	constexpr Scope<T> CreateScope(Args&& ... args)
	{
		return std::make_unique<T>(std::forward<Args>(args)...);
	}

	template<typename T>
	using Ref = std::shared_ptr<T>;
	template<typename T, typename ... Args>
	constexpr Ref <T> CreateRef(Args && ... args)
	{
		return std::make_shared<T>(std::forward<Args>(args)...);
	}
}