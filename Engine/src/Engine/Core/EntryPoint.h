#pragma once
#include "Engine/Core/Core.h"
extern Engine::Application* Engine::CreateApplication();

int main(int argc, char** argv)
{
	Engine::Log::Init();
	EN_CORE_WARN("Initialised Logger!");
	int a = 0;
	EN_TRACE("Initialised Logger!{0}", a);

	auto app = Engine::CreateApplication();
	app->Run();
	delete app;
}