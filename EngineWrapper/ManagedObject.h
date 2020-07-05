#pragma once
using namespace System;

namespace Wrapper
{
	template<class T>
	public ref class ManagedObject
	{
	protected:
		T* mInstance;
	public:
		ManagedObject(T* Instance):mInstance(Instance) {}
		
		virtual ~ManagedObject()
		{
			if (mInstance != nullptr)
				delete mInstance;
		}

		!ManagedObject()
		{
			if (mInstance != nullptr)
				delete mInstance;
		}

		T* GetInstance()
		{
			return mInstance;
		}
	};

	using namespace System::Runtime::InteropServices;
	static const char* stringsToCStrings(String^ string)
	{
		return (const char*)(Marshal::StringToHGlobalAnsi(string)).ToPointer();
	}
}
