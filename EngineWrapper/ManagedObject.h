#pragma once
using namespace System;
#include <memory>

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
			this->!ManagedObject();
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

	static int* intArrayToPointer(array<System::Int32>^ data)
	{
		pin_ptr<int> ptr = &data[0];
		return ptr;
	}

	static unsigned int* uintArrayToPointer(array<System::UInt32>^ data)
	{
		pin_ptr<unsigned int> ptr = &data[0];
		return ptr;
	}

	static float* floatArrayToPointer(array<System::Single>^ data)
	{
		pin_ptr<float> ptr = &data[0];
		return ptr;
	}

	template<typename T>
	static T* CLIPointerToNativePointer(T% value)
	{
		pin_ptr<T> ptr = &value;
		return ptr;
	}
}