#include "ENPH.h"

namespace Engine
{
	void Log::Init() 
	{
		std::ofstream file("../../../Log.txt", std::ofstream::app);
		time_t now = std::chrono::system_clock::to_time_t(std::chrono::system_clock::now());
		tm time = *std::localtime(&now);
		file << "\n\n--------------------------------------------" + 
			std::to_string(time.tm_mday) + "\\" + std::to_string(time.tm_mon + 1) + "\\" + std::to_string(time.tm_year + 1900) + "\n";
		file.close();
	}

	std::string Log::GetTime()
	{
		time_t now = std::chrono::system_clock::to_time_t(std::chrono::system_clock::now());
		tm time = *std::localtime(&now);
		return std::to_string(time.tm_hour) + ":" + std::to_string(time.tm_min) + ":" + std::to_string(time.tm_sec);
	}
}