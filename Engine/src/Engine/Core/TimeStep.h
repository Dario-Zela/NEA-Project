#pragma once

namespace Engine
{
	class ENGINE_API TimeStep
	{
	public:
		TimeStep(float time = 0.0f)
			:mTime(time)
		{
		}

		operator float() const { return mTime; }

		inline float GetSeconds() const { return mTime; }
		inline float GetMilliseconds() const { return mTime * 1000.0f; }

	private:
		float mTime;
	};
}